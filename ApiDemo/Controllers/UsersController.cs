using DataAccess.Commands;
using DataAccess.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {
                return Results.Ok(await _mediator.Send(new GetUsersQuery()));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            try
            {
                var results = await _mediator.Send(new GetUserByIdQuery(id));
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        // POST api/Users
        [HttpPost]
        public async Task<IResult> Post(
            [FromBody] UserModel user)
        {
            try
            {
                await _mediator.Send(new InsertUserCommand(user));
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public async Task<IResult> Put([FromBody] UserModel user, [FromRoute] string id)
        {
            try
            {
                user.Id = id;
                await _mediator.Send(new UpdateUserCommand(id,user));
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
