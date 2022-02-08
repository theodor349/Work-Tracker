using DataAccess.UnitOfWorks;
using MediatR;
using Shared.Models;
using Shared.Models.DTOs;
using TimeSheetGeneration.Commands;
using WorkTracker.Commands;
using WorkTracker.Handlers.Internal;
using WorkTracker.Queries;

namespace WorkTracker.Handlers
{
    public class GetAauTimeSheetQueryHandler : GenericEmployerAuthorizedRequestHandler<GetAauTimeSheetQuery, string, IUnitOfWork>
    {
        private readonly int _startHour = 8;
        private readonly int _maxExtraHours = 8;

        public GetAauTimeSheetQueryHandler(IUnitOfWork unitOfWork, IMediator mediator) : base(unitOfWork, mediator)
        {
        }

        internal override async Task<string> HandleTransaction(GetAauTimeSheetQuery request, CancellationToken cancellationToken)
        {
            var startDate = GetStartOfDay(request.StartDate);
            var endDate = GetEndOfDay(request.EndDate);
            request = new GetAauTimeSheetQuery(request.EmployerId, request.UserId, startDate, endDate, request.MaxMonthlyHours, request.ExtraHours, request.ShouldAddInvoice, request.IsAuthorized);

            await RequireAccessToEmployer(request);
            await CheckValidityOfRequest(request);

            var dayEnties = await GenerateDayEntries(request, startDate, endDate);
            var filePath = await CreateFile(request, startDate, dayEnties);
            await AddInvoice(request, dayEnties);
            return filePath;
        }

        private static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).AddDays(1).AddMinutes(-1);
        }

        private static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        private async Task<string> CreateFile(GetAauTimeSheetQuery request, DateTime startDate, List<DayEntry> dayEnties)
        {
            return await _mediator.Send(new GenerateAauTimeSheetCommand(dayEnties, startDate.Year, startDate.Month, request.UserId));
        }

        private async Task AddInvoice(GetAauTimeSheetQuery request, List<DayEntry> dayEnties)
        {
            if (request.ShouldAddInvoice)
            {
                TimeSpan total = GetTotalTimeSpan(dayEnties);
                await _mediator.Send(new CreateInvoiceCommand(
                    request.EmployerId,
                    request.UserId,
                    DateTime.Now,
                    request.StartDate,
                    request.EndDate,
                    total,
                    true));
            }
        }

        private async Task<List<DayEntry>> GenerateDayEntries(GetAauTimeSheetQuery request, DateTime startDate, DateTime endDate)
        {
            var workEntries = await _mediator.Send(new GetWorkEntryBetweenListQuery(request.EmployerId, request.UserId, startDate, endDate));
            var dayEnties = ConvertToDayEntries(workEntries, startDate, request.MaxMonthlyHours);
            AddOverTime(dayEnties, request.StartDate, request.MaxMonthlyHours, request.ExtraHours);
            return dayEnties;
        }

        private async Task CheckValidityOfRequest(GetAauTimeSheetQuery request)
        {
            if (request.StartDate.Year != request.EndDate.Year && request.StartDate.Month != request.EndDate.Month)
                throw new InvalidOperationException("Unable to create timesheet across multiple months");
            if (request.EndDate.Ticks < request.StartDate.Ticks)
                throw new InvalidOperationException("StartDate must be before EndDate");
            var employerBalance = await _mediator.Send(new GetEmployerBalanceQuery(request.EmployerId, request.UserId, request.StartDate, true));
            if (request.ExtraHours > employerBalance.Balance)
                throw new InvalidOperationException("You cannot add " + request.ExtraHours + " hours to your invoice when you only have a total of " + employerBalance.Balance.TotalHours + " hours on your balance");
            var timeSpanInvoices = await _mediator.Send(new GetInvoicesBetweenQuery(request.EmployerId, request.UserId, request.StartDate, request.EndDate, true));
            if (request.ShouldAddInvoice && timeSpanInvoices.Count > 0)
                throw new InvalidOperationException("You cannot make a new invoice for the same timespan as anotherone");
        }

        private TimeSpan GetTotalTimeSpan(IEnumerable<DayEntry> entries)
        {
            long totalTicks = entries.Sum(x => x.Duration.Ticks);
            return new TimeSpan(totalTicks);
        }

        private void AddOverTime(List<DayEntry> dayEnties, DateTime startDate, TimeSpan maxMonthlyHours, TimeSpan extraHours)
        {
            var totalLeft = extraHours;
            var addedTime = GetTotalTimeSpan(dayEnties);
            var maxExtraTime = new TimeSpan(_maxExtraHours, 0, 0);

            for (int d = 1; d <= 29; d++)
            {
                if (dayEnties.Where(x => x.StartTime.Day == d).Count() != 0)
                    continue;
                if (totalLeft.Ticks == 0 || addedTime == maxMonthlyHours)
                    return;

                var timeToAdd = new TimeSpan(Math.Min(new TimeSpan(_maxExtraHours, 0, 0).Ticks, totalLeft.Ticks));
                timeToAdd = new TimeSpan(Math.Min(timeToAdd.Ticks, (maxMonthlyHours - addedTime).Ticks));
                var entryStart = GetDate(startDate, d);
                dayEnties.Add(new DayEntry(entryStart, entryStart.Add(timeToAdd), "Overflow"));
                totalLeft = totalLeft.Subtract(timeToAdd);
                addedTime = addedTime.Add(timeToAdd);
            }

            foreach (var day in dayEnties)
            {
                var space = new TimeSpan(Math.Max((maxExtraTime - day.Duration).Ticks, 0));
                var timeToAdd = new TimeSpan(Math.Min(space.Ticks, totalLeft.Ticks));
                day.EndTime = day.EndTime.Add(timeToAdd);
                totalLeft = totalLeft.Subtract(timeToAdd);
            }
        }

        private List<DayEntry> ConvertToDayEntries(IReadOnlyList<WorkEntryModel> workEntries, DateTime startTime, TimeSpan maxMonthlyHours)
        {
            var days = new Dictionary<int, TimeSpan>();
            foreach (var entry in workEntries)
            {
                if (days.ContainsKey(entry.StartTime.Day))
                    days[entry.StartTime.Day] = days[entry.StartTime.Day].Add(entry.Duration);
                else
                    days.Add(entry.StartTime.Day, entry.Duration);
            }

            var res = new List<DayEntry>();
            var addedTime = TimeSpan.Zero;
            foreach (var day in days)
            {
                if (addedTime >= maxMonthlyHours)
                    break;

                var date = GetDate(startTime, day.Key);
                var timeToAdd = new TimeSpan(Math.Min(day.Value.Ticks, (maxMonthlyHours - addedTime).Ticks));
                res.Add(new DayEntry(date, date.Add(timeToAdd)));
                addedTime = addedTime.Add(timeToAdd);
                if (day.Value.TotalHours == 24 - _startHour)
                    throw new ArgumentException("This time interval will exeed the 24 hour mark. Interval: " + date.Hour + "-" + (date.Hour + day.Value.Hours));
            }
            return res;
        }

        private DateTime GetDate(DateTime startTime, int day)
        {
            return new DateTime(startTime.Year, startTime.Month, day, _startHour, 0, 0);
        }
    }
}