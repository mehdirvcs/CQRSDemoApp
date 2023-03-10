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
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Task>
    {
        private readonly ICosmosDataAccess _data;
        public DeleteUserCommandHandler(ICosmosDataAccess data)
        {
            _data = data;
        }

        public async Task<Task> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _data.DeleteFamilyItemAsync(request._id, request._id);
            return Task.CompletedTask;
        }

    }
}
