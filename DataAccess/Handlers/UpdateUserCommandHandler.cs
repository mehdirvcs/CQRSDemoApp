using DataAccess.Commands;
using DataAccess.DbAccess;
using DataAccess.Models;
using DataAccess.Queries;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Task>
    {
        private readonly ICosmosDataAccess _data;
        public UpdateUserCommandHandler(ICosmosDataAccess data)
        {
            _data = data;
        }

        public async Task<Task> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _data.ReplaceFamilyItemAsync(request.User);
            return Task.CompletedTask;
        }

    }
}
