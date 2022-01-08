using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkTracker.Queries;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private Guid UserId => Guid.Parse("26ad541c-ec75-4b78-b784-2a4eb6d668e0");

        private readonly IMediator _mediator;

        public TimeSheetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<TimeSheetController>/AauTimeSheet
        [HttpGet("AauTimeSheet/{employerId}")]
        public async Task<FileContentResult> Get(Guid employerId, DateTime startDate, DateTime endDate, double maxMonthlyHours, double extraHours, bool ShouldAddInvoice)
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