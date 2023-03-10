using DataAccess.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Commands
{
    public class DeleteUserCommand : IRequest<Task>
    {
        public string _id;
        public DeleteUserCommand(string id)
        {
            _id = id;
        }
    }
}
