using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

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

        public IdentityUser GetUser(string userId)
        {
            return usersRepository.GetUser(userId);
        }
        
        public void DeleteUser(string userId)
        {
            IdentityUser identityUser = GetUser(userId);
            usersRepository.DeleteUser(identityUser);
        }

        public void DeleteRole(string userId, string role)
        {
            IdentityUser identityUser = GetUser(userId);
            usersRepository.DeleteRole(identityUser, role);
        }
        
        public IList<IdentityUser> GetUsers(string role)
        {
            return usersRepository.GetUsers(role).ToList();
        }
        
        public void GiveRole(string userId, string role)
        {
            IdentityUser identityUser = GetUser(userId);
            usersRepository.DeleteRole(identityUser, "USER");
            usersRepository.GiveRole(identityUser, role);
        }
        
        public void CreateUser(IdentityUser identityUser)
        {
            usersRepository.CreateUser(identityUser);
        }
    }
}