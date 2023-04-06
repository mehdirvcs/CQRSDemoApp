using DataAccess.Commands;
using DataAccess.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiDemo.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Users
        [MapToApiVersion("2.0")]
        [HttpGet]
        public string Get()
        {
            return "Get all Items";
        }

        // GET api/Users/5
        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "Get Item 1";
        }

        // POST api/Users
        [MapToApiVersion("2.0")]
        [HttpPost]
        public string Post(
            [FromBody] UserModel user)
        {
            return "Inserted";
        }

        // PUT api/Users/5
        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public string Put([FromBody] UserModel user, [FromRoute] string id)
        {
            return "Updated";
        }

        // DELETE api/Users/5
        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            return "Deleted";
        }
    }
}
