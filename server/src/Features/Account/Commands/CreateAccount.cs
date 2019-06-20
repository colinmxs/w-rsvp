using Amazon.DynamoDBv2;
using Google.Apis.Auth;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace w_rsvp.Api.Features.Account.Commands
{
    public class CreateAccount
    {
        public class Command : IRequest<Result>
        {
            public string IdToken { get; set; }
        }

        public class Result
        {

        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private IAmazonDynamoDB _dbClient;

            public CommandHandler(IAmazonDynamoDB dbClient)
            {
                _dbClient = dbClient;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                throw new System.NotImplementedException();

            }
        }
    }
}
