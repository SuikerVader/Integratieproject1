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
        
        public void DeleteUser(string userId)
        {
            IdentityUser identityUser = GetUser(userId);
            _usersRepository.DeleteUser(identityUser);
        }

        public void DeleteRole(string userId, string role)
        {
            IdentityUser identityUser = GetUser(userId);
            _usersRepository.DeleteRole(identityUser, role);
        }
        
        public IList<IdentityUser> GetUsers(string role)
        {
            return _usersRepository.GetUsers(role).ToList();
        }
        
        public void GiveRole(string userId, string role)
        {
            IdentityUser identityUser = GetUser(userId);
            _usersRepository.DeleteRole(identityUser, "USER");
            _usersRepository.GiveRole(identityUser, role);
        }
        
        public void CreateUser(IdentityUser identityUser)
        {
            _usersRepository.CreateUser(identityUser);
        }

        public void BlockUser(string userId, int days)
        {
            IdentityUser identityUser = GetUser(userId);
            _usersRepository.BlockUser(identityUser, days);
        }
    }
}