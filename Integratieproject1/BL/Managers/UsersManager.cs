using System;
using System.Collections;
using System.Collections.Generic;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.BL.Managers
{
    public class UsersManager : IUsersManager
    {
        private readonly UsersRepository _usersRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public UsersManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _usersRepository = new UsersRepository(_unitOfWorkManager.UnitOfWork);
        }

        public UsersManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            _unitOfWorkManager = unitOfWorkManager;
            _usersRepository = new UsersRepository(_unitOfWorkManager.UnitOfWork);
        }

        public IdentityUser GetUser(string userId)
        {
            return _usersRepository.GetUser(userId);
        }

        /*public LoggedInUser GetLoggedInUser(string userId)
        {
            IdentityUser user = usersRepository.GetUser(userId);
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
        }*/

        /*public IList<LoggedInUser> GetLoggedInUsers()
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
        }*/
    }
}