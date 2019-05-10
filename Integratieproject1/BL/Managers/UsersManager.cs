using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public CustomUser GetUser(string userId)
        {
            return _usersRepository.GetUser(userId);
        }
        
        public void DeleteUser(string userId)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.DeleteUser(identityUser);
        }

        public void DeleteRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.DeleteRole(identityUser, role);
        }
        
        public IList<CustomUser> GetUsers(string role)
        {
            return _usersRepository.GetUsers(role).ToList();
        }
        
        public void GiveRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.DeleteRole(identityUser, "USER");
            _usersRepository.GiveRole(identityUser, role);
        }
        
        public bool IsInRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            return _usersRepository.IsInRole(identityUser, role);
        }

        public void CreateUser(CustomUser identityUser)
        {
            _usersRepository.CreateUser(identityUser);
        }
        
        #region VerificationRequest

        public IEnumerable<VerificationRequest> GetVerificationRequests()
        {
            return _usersRepository.GetVerificationRequests();
        }

        public void CreateVerificationRequest(VerificationRequest verificationRequest)
        {
            _usersRepository.CreateVerificationRequest(verificationRequest);
        }

        public VerificationRequest CreateVerificationRequest(CustomUser user, string request)
        {
            VerificationRequest verificationRequest = new VerificationRequest();
            verificationRequest.user = user;
            verificationRequest.request = request;
            verificationRequest.handled = false;
            return verificationRequest;
        }

        public void HandleVerificationRequest(VerificationRequest verificationRequest, bool accepted)
        {
            if (accepted)
            {
                GiveRole(verificationRequest.user.Id, "ORGANISATION");
            }
            _usersRepository.SetVerificationRequestHandled(verificationRequest);
        }
        #endregion

        public void BlockUser(string userId, int days)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.BlockUser(identityUser, days);
        }

        public CustomUser GetUserByEmail(string email)
        {
            return _usersRepository.GetUserByEmail(email);
        }

        public string GetSurname(CustomUser customUser)
        {
            return _usersRepository.GetSurname(customUser);
        }

        public string GetName(CustomUser customUser)
        {
            return _usersRepository.GetName(customUser);
        }

        public string GetSex(CustomUser customUser)
        {
            return _usersRepository.GetSex(customUser);
        }

        public int GetAge(CustomUser customUser)
        {
            return _usersRepository.GetAge(customUser);
        }

        public string GetZipcode(CustomUser customUser)
        {
            return _usersRepository.GetZipcode(customUser);
        }
    }
}