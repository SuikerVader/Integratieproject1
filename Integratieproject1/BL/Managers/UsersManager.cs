using System;
using System.Collections;
using System.Collections.Generic;
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

        public UsersManager(UnitOfWorkManager unitOfWorkManager)
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
                Console.WriteLine("not a loggedInUser exception: " + e);
                throw;
            }
        }

        public IList<LoggedInUser> GetLoggedInUsers()
        {
            IEnumerable<User> users = usersRepository.GetLoggedInUsers();
            IList<LoggedInUser> loggedInUsers = new List<LoggedInUser>();
            foreach (var user in users)
            {
                try
                {
                    LoggedInUser loggedInUser = (LoggedInUser) user;
                    loggedInUsers.Add(loggedInUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine("not a loggedinuser exception: " + e);
                    throw;
                }

            }

            return loggedInUsers;
        }
    }
}