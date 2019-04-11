using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CityOfIdeasDbContext _ctx;
        public UsersRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
                
            _ctx = unitOfWork.Ctx;
        }

        public IdentityUser GetUser(string id)
        {
            IdentityUser identityUser = _ctx.Users.Find(id);
            return identityUser;
        }

        
    }
}