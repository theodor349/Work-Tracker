namespace Shared
{
    public interface IFileAccessSetup
    {
        string RootPath { get; set; }
        string TemplatePath { get; }
        string TimeSheetsPath { get; }
        string SignaturePath { get; }
    }

    public class FileAccessSetup : IFileAccessSetup
    {
        public string RootPath { get; set; }

        public string TemplatePath => Path.Combine(RootPath, "Templates");
        public string TimeSheetsPath => Path.Combine(RootPath, "TimeSheets");
        public string SignaturePath => Path.Combine(RootPath, "Images", "Signatures");

        public FileAccessSetup()
        {
            RootPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            Directory.CreateDirectory(TemplatePath);
            Directory.CreateDirectory(TimeSheetsPath);
            Directory.CreateDirectory(SignaturePath);
        }
    }
}
