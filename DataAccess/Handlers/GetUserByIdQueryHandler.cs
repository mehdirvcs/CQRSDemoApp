using DataAccess.DbAccess;
using DataAccess.Models;
using DataAccess.Queries;
using MediatR;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserModel>
    {
        private readonly ICosmosDataAccess _data;
        private readonly string _query = "SELECT * FROM c";
        private readonly IMediator _mediator;

        public GetUserByIdQueryHandler(ICosmosDataAccess data, IMediator mediator)
        {
            _data = data;
            _mediator = mediator;
        }


        public async Task<UserModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            List<UserModel> users = await _mediator.Send(new GetUsersQuery());
            var output = users.FirstOrDefault(x => x.Id == request._id);
            return output;

            //var results = await _data.LoadData<UserModel, dynamic>(
            //                storedProcedures: "dbo.spUser_Get",
            //                new { Id = request._id });
            //return results.FirstOrDefault();
        }
    }
}
