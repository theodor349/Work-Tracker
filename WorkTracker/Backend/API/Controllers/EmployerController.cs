using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
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
        private Guid UserId => Guid.Parse("26ad541c-ec75-4b78-b784-2a4eb6d668e0");

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

        [HttpGet("{id}/Balance")]
        public async Task<EmployerBalanace> Get(Guid id, DateTime beforeDate)
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
