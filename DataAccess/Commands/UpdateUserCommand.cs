using DataAccess.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Commands
{
    public class UpdateUserCommand : IRequest<Task>
    {
        public UserModel User { get; set; }


        public UpdateUserCommand(string id, UserModel newUser)
        {
            User= newUser;
            User.Id = id;
        }

    }
}
