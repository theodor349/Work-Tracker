using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using WorkTracker.Queries;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private Guid UserId => Guid.Parse("26ad541c-ec75-4b78-b784-2a4eb6d668e0");

        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Invoice/{employerId}
        [HttpGet("{employerId}")]
        public async Task<IEnumerable<InvoiceModel>> Get(Guid employerId, DateTime start, DateTime end)
        {
            start = ClampDateToSQL(start);
            end = ClampDateToSQL(end);

            var data = await _mediator.Send(new GetInvoicesCreatedBetweenQuery(employerId, UserId, start, end));
            return data;
        }

        private static DateTime ClampDateToSQL(DateTime date)
        {
            if (date.Year < 1990)
                date = new DateTime(1990, date.Month, date.Day);
            return date;
        }

        // GET: api/Invoice/{employerId}/AauStudentProgrammerTimeSheet?
        [HttpGet("{employerId}/AauStudentProgrammerTimeSheet")]
        public async Task<FileContentResult> GetAauInvoice(Guid employerId, DateTime startDate, DateTime endDate, double maxMonthlyHours, double extraHours, bool ShouldAddInvoice)
        {
            var filePath = await _mediator.Send(new GetAauTimeSheetQuery(employerId, UserId, startDate, endDate, TimeSpan.FromHours(maxMonthlyHours), TimeSpan.FromHours(extraHours), ShouldAddInvoice));
            return File(
                System.IO.File.ReadAllBytes(filePath),
                "application/msword",
                "TimeSheet_" + startDate.ToString("yyyy-MM-dd") + "_" + endDate.ToString("yyyy-MM-dd") + ".doc"
                );
        }
    }
}