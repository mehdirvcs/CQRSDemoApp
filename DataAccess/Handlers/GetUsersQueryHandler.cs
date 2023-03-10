using DataAccess.DbAccess;
using DataAccess.Models;
using DataAccess.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
    {
        private readonly ICosmosDataAccess _data;
        private readonly string _query = "SELECT * FROM c";
        public GetUsersQueryHandler(ICosmosDataAccess data)
        {
            _data = data;
        }

        public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            List<UserModel> users = await _data.QueryItemsAsync(_query);
            return users;

        }
    }
}
