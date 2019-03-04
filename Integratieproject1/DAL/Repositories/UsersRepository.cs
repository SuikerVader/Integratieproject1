using System;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository
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