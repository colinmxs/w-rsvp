using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using w_rsvp.Api.Features.Account.Commands;

namespace w_rsvp.Api.Features.Account
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CreateAccount.Result> Post([FromBody]CreateAccount.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
