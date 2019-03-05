using System;
using Integratieproject1.DAL.Interfaces;

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
        
    }
}