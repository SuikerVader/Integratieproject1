using System;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.BL.Managers
{
    public class UsersManager : IUsersManager
    {
        private UsersRepository usersRepository;
        private UnitOfWorkManager unitOfWorkManager;

        public UsersManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            usersRepository = new UsersRepository(unitOfWorkManager.UnitOfWork);
        }
        public UsersManager( UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException("unitOfWorkManager");

            this.unitOfWorkManager = unitOfWorkManager;
            usersRepository = new UsersRepository(this.unitOfWorkManager.UnitOfWork);
        }

        public User GetUser(int userId)
        {
            return usersRepository.GetUser(userId);
        }

        public LoggedInUser GetLoggedInUser(int userId)
        {
            User user = usersRepository.GetUser(userId);
            try
            {
                LoggedInUser loggedInUser = (LoggedInUser) user;
                return loggedInUser;
            }
            catch (Exception e)
            {
                Console.WriteLine("not a loggedInUser exception");
                throw;
            }
        }
    }
}