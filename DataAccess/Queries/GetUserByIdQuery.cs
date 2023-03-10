using DataAccess.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Queries
{
    public class GetUserByIdQuery : IRequest<UserModel>
    {
        public string _id { get; set; }

        public GetUserByIdQuery(string id) 
        {
            _id = id;
        }
    }
}
