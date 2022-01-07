using MediatR;
using Microsoft.Extensions.Configuration;
using Shared;
using Shared.Models.DTOs;
using Spire.Doc;
using Spire.Doc.Collections;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetGeneration.Commands;

namespace TimeSheetGeneration.Handlers;

public class GenerateAauTimeSheetCommandHandler : IRequestHandler<GenerateAauTimeSheetCommand, string>
{
    private readonly IConfiguration _config;
    private readonly IFileAccessSetup _fileAccess;

    public GenerateAauTimeSheetCommandHandler(IConfiguration config, IFileAccessSetup fileAccess)
    {
        _config = config;
        _fileAccess = fileAccess;
    }

    public Task<string> Handle(GenerateAauTimeSheetCommand request, CancellationToken cancellationToken)
    {
        var templateName = _config.GetValue<string>("TimeSheets:AAU:StudentermedhjaelpPROSA");
        string templatePath = Path.Combine(_fileAccess.TemplatePath, templateName);
        string outputPath = Path.Combine(_fileAccess.TimeSheetsPath, Guid.NewGuid().ToString());

        CreateWordDocTimeSheet(templatePath, outputPath, request.Enties);

        return Task.FromResult(outputPath);
    }

    void CreateWordDocTimeSheet(string templatePath, string outputPath, List<DayEntry> entries)
    {
        var document = new Document(templatePath);
        FillDocument(document, entries);
        document.SaveToFile(outputPath, FileFormat.Doc);
    }

    void FillDocument(Document document, List<DayEntry> entries)
    {
        var fields = document.Sections[0].Body.FormFields;
        FillDates(fields);
        //FillPersonalInfo(fields);
        //FillSignature(document);
        FillHours(fields, entries);
    }

    void FillHours(FormFieldCollection fields, List<DayEntry> entries)
    {
        int totalHoursIndex = 98;
        decimal hours = GetHours(entries);
        fields[totalHoursIndex].Text = Math.Round(hours, 2).ToString(); ;

        foreach (var DayEntry in entries)
        {
            int index = GetFieldIndex(DayEntry.StartTime.Day);
            fields[index + 0].Text = DayEntry.StartTime.ToString("HH:mm") + "-" + DayEntry.EndTime.ToString("HH:mm");
            fields[index + 1].Text = DayEntry.Duration.ToString(@"hh\:mm");
            fields[index + 2].Text = DayEntry.Note;
        }
    }

    int GetFieldIndex(int day)
    {
        day--;
        int startIndex = 5;
        int rows = 16;
        int columns = 6;

        int row = day % rows * columns;
        int column = day < rows ? 0 : columns / 2;
        return startIndex + row + column;
    }

    void FillSignature(Document document, string imagePath)
    {
        var p = new Bitmap(Image.FromFile(imagePath));
        DocPicture picture = document.Sections[0].Paragraphs[0].AppendPicture(p);
        picture.HorizontalPosition = 250.0F;
        picture.VerticalPosition = 500.0F;
        picture.Width = 20;
        picture.Height = 15;
        picture.TextWrappingStyle = TextWrappingStyle.Through;

    }

    void FillPersonalInfo(FormFieldCollection fields, string fullName, string cprNumber)
    {
        int nameIndex = 2;
        int cprIndex = 3;

        fields[nameIndex].Text = fullName;
        fields[cprIndex].Text = cprNumber;
    }

    void FillDates(FormFieldCollection fields)
    {
        var timeSheetMonth = DateTime.Now;
        var creationDate = DateTime.Now;

        int monthIndex = 0;
        int yearIndex = 1;
        int signingDateIndex = 99;

        fields[monthIndex].Text = timeSheetMonth.ToString("MM");
        fields[yearIndex].Text = (timeSheetMonth.Year - 2020).ToString();
        fields[signingDateIndex].Text = creationDate.ToString("dd/MM-yyyy");

    }

    decimal GetHours(IEnumerable<DayEntry> entries)
    {
        int hours = 0;
        int minutes = 0;
        int seconds = 0;
        foreach (var entry in entries)
        {
            hours += entry.Duration.Hours;
            minutes += entry.Duration.Minutes;
            seconds += entry.Duration.Seconds;
        }

        return hours + (decimal)minutes / 60 + (decimal)seconds / 3600;
    }

}