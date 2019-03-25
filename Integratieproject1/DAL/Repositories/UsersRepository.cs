using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public UsersRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
                
            ctx = unitOfWork.ctx;
        }

        public User GetUser(int userId)
        {
          return  ctx.Users
              .Include(p => p.Platform)
              .Single(u => u.UserId == userId);
        }

        public IEnumerable<User> GetLoggedInUsers()
        {
          return ctx.Users.Where(u => u.GetType() == typeof(LoggedInUser)).AsEnumerable();
        }
    }
}