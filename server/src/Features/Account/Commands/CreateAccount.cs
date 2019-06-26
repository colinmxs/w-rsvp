using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Google.Apis.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static w_rsvp.Api.Startup;

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
            public string AltId { get; internal set; }
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
                var googleId = payload.Subject;
                Table table = null;
                var tableName = $"{Configuration["AWS:Tables:Account:Name"]}_{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
                try
                {
                    table = Table.LoadTable(_dbClient, tableName);
                }
                catch
                {
                    //table doesn't exist...probably.
                    //create table
                    var createTableRequest = new CreateTableRequest(
                        tableName: tableName,
                        keySchema: new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "Id",
                                KeyType = KeyType.HASH
                            }
                        },
                        attributeDefinitions: new List<AttributeDefinition>
                        {
                            new AttributeDefinition
                            {
                                AttributeName = "Id",
                                AttributeType = ScalarAttributeType.S
                            }
                        },
                        provisionedThroughput: new ProvisionedThroughput(1, 1))
                    {
                        BillingMode = BillingMode.PAY_PER_REQUEST,
                        SSESpecification = new SSESpecification()
                        {
                            Enabled = true
                        },
                        Tags = new List<Tag>
                        {
                            new Tag
                            {
                                Key = "Application",
                                Value = "w-rsvp"
                            },
                            new Tag
                            {
                                Key = "Environment",
                                Value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                            }
                        }
                    };

                    await _dbClient.CreateTableAsync(createTableRequest);
                    return await Handle(request, cancellationToken);
                }

                //check user store for subject, if exist return app id.
                Document account = null;
                account = await table.GetItemAsync(googleId);

                //if not exist, create that bish
                if(account == null)
                {
                    var randomNumberGenerator = RandomNumberGenerator.Create();
                    var bytes = new byte[10];
                    randomNumberGenerator.GetBytes(bytes);
                    var localId = BitConverter.ToUInt32(bytes).ToString();

                    //make sure the "localId" doesn't already exist in the db
                    var altAccount = await table.GetItemAsync(localId);
                    if(altAccount != null)
                    {
                        //localId already exists, restart...
                        return await Handle(request, cancellationToken);
                    }

                    var newAccount = new Dictionary<string, AttributeValue>()
                    {
                        { "Id", new AttributeValue(googleId) },
                        { "AltId", new AttributeValue(localId) }
                    };
                    var newAltAccount = new Dictionary<string, AttributeValue>()
                    {
                        { "AltId", new AttributeValue(googleId) },
                        { "Id", new AttributeValue(localId) }
                    };

                    var accountDoc = Document.FromAttributeMap(newAccount);
                    var altAccountDoc = Document.FromAttributeMap(newAltAccount);
                    await table.PutItemAsync(accountDoc);
                    await table.PutItemAsync(altAccountDoc);
                    account = accountDoc;
                }

                return new Result
                {
                    AltId = account["AltId"].AsString()
                };
            }
        }
    }
}
