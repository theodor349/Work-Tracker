using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.DTOs;
using WorkTracker.Commands;
using WorkTracker.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private Guid UserId => new Guid(User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value); // Guid.Parse("26ad541c-ec75-4b78-b784-2a4eb6d668e0");

        private readonly IMediator _mediator;

        public EmployerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<EmployerController>
        [HttpGet]
        public async Task<IEnumerable<EmployerModel>> Get()
        {
            var data = await _mediator.Send(new GetEmployerListQuery(UserId));
            return data;
        }

        // GET api/<EmployerController>/5
        [HttpGet("{id}")]
        public async Task<EmployerModel> Get(Guid id)
        {
            var data = await _mediator.Send(new GetEmployerByIdQuery(id, UserId));
            return data;
        }

        [HttpGet("Balance")]
        public async Task<IEnumerable<EmployerDisplayModel>> GetBalance(DateTime beforeDate)
        {
            var res = new List<EmployerDisplayModel>();
            var employers = await _mediator.Send(new GetEmployerListQuery(UserId));
            foreach (var employer in employers)
            {
                var display = new EmployerDisplayModel();
                display.Name = employer.Name;
                display.Id = employer.Id;
                await AddLatests(employer, display);
                await AddTimeThisMonth(employer, display);
                display.Balance = await _mediator.Send(new GetEmployerBalanceQuery(employer.Id, UserId, beforeDate));
                res.Add(display);
            }
            return res;
        }

        private async Task AddLatests(EmployerModel employer, EmployerDisplayModel display)
        {
            var latest = await _mediator.Send(new GetLatestWorkEntryQuery(employer.Id, UserId, true));
            if (latest != null && latest.EndTime == null)
                display.StartTime = latest.StartTime;
        }

        private async Task AddTimeThisMonth(EmployerModel employer, EmployerDisplayModel display)
        {
            var currentTime = DateTime.Now;
            var startDate = new DateTime(currentTime.Year, currentTime.Month, 1);
            var endDate = new DateTime(currentTime.Year, currentTime.Month + 1, 1).AddSeconds(-1);
            var entires = await _mediator.Send(new GetWorkEntryBetweenListQuery(employer.Id, UserId, startDate, endDate, true));
            var time = new TimeSpan(0);
            foreach (var entry in entires)
                time = time.Add(entry.Duration);
            display.TimeThisMonth = time;
        }

        [HttpGet("Balance/{id}")]
        public async Task<EmployerBalanace> GetBalance(Guid id, DateTime beforeDate)
        {
            var data = await _mediator.Send(new GetEmployerBalanceQuery(id, UserId, beforeDate));
            return data;
        }

        // POST api/<EmployerController>
        [HttpPost]
        public async Task<EmployerModel> Post([FromBody] string name)
        {
            var data = await _mediator.Send(new CreateEmployerCommand(name, UserId));
            return data;
        }

        // PUT api/<EmployerController>/5
        [HttpPut("{id}")]
        public async Task<EmployerModel> Put(string id, [FromBody] string name)
        {
            var data = await _mediator.Send(new UpdateEmployerCommand(name, Guid.Parse(id), UserId));
            return data;
        }

        // DELETE api/<EmployerController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteEmployerCommand(Guid.Parse(id), UserId));
            return data;
        }
    }
}
