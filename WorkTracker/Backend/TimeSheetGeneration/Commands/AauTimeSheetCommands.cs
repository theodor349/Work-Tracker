using MediatR;
using Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetGeneration.Commands;

public record GenerateAauTimeSheetCommand(List<DayEntry> Enties, int Year, int Month, Guid UserId) : IRequest<string>;
