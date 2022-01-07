using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.DTOs.WorkEntry;
using WorkTracker.Commands;
using WorkTracker.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkEntryController : ControllerBase
    {
        private readonly IMediator _mediator;

        private Guid UserId => Guid.Parse("26ad541c-ec75-4b78-b784-2a4eb6d668e0");

        public WorkEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<WorkEntryController>
        [HttpGet("{employerId}")]
        public async Task<IEnumerable<WorkEntryModel>> Get(Guid employerId)
        {
            var data = await _mediator.Send(new GetWorkEntryListQuery(employerId, UserId));
            return data;
        }

        // GET api/<WorkEntryController>/5/At
        [HttpGet("{employerId}/At{startDate}")]
        public async Task<WorkEntryModel> GetAt(Guid employerId, DateTime startDate)
        {
            var data = await _mediator.Send(new GetWorkEntryByStartTimeQuery(employerId, startDate, UserId));
            return data;
        }

        // GET api/<WorkEntryController>/5/Latests
        [HttpGet("{employerId}/Latests")]
        public async Task<WorkEntryModel> GetLatests(Guid employerId)
        {
            var data = await _mediator.Send(new GetLatestWorkEntryQuery(employerId, UserId));
            return data;
        }

        // POST api/<WorkEntryController>
        [HttpPost]
        public async Task<WorkEntryModel> Start([FromBody] CreateWorkEntryDto value)
        {
            var data = await _mediator.Send(new CreateWorkEntryCommand(value.EmployerId, value.Start, value.End, UserId));
            return data;
        }

        // POST api/<WorkEntryController>/Start
        [HttpPost]
        [Route("Start")]
        public async Task<WorkEntryModel> Start([FromBody] StartWorkEntryDto value)
        {
            var data = await _mediator.Send(new StartWorkEntryCommand(value.EmployerId, value.Start, UserId));
            return data;
        }

        // POST api/<WorkEntryController>/EndLatest
        [HttpPost]
        [Route("EndLatest")]
        public async Task<WorkEntryModel> End([FromBody] EndLatestWorkEntryDto value)
        {
            var data = await _mediator.Send(new EndLatestWorkEntryCommand(value.EmployerId, value.End, UserId));
            return data;
        }

        // PUT api/<WorkEntryController>/5
        [HttpPut("{employerId}")]
        public async Task<WorkEntryModel> Put(Guid employerId, [FromBody] UpdateWorkEntryDto value)
        {
            var data = await _mediator.Send(new UpdateWorkEntryCommand(employerId, value.OldStartTime, value.NewStartTime, value.NewEndTime, UserId));
            return data;
        }

        // DELETE api/<WorkEntryController>/5
        [HttpDelete("{employerId}")]
        public async Task Delete(Guid employerId, [FromBody] DateTime startDate)
        {
            await _mediator.Send(new DeleteWorkEntryCommand(employerId, startDate, UserId));
        }
    }
}
