using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CityOfIdeasDbContext _ctx;
        private UserStore<IdentityUser> _userStore;
        
        public UsersRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
           
            _ctx = unitOfWork.Ctx;
            _userStore = new UserStore<IdentityUser>(_ctx);
        }

        public IdentityUser GetUser(string id)
        {
            return _userStore.FindByIdAsync(id).Result;
        }
        
        public async void DeleteUser(IdentityUser identityUser)
        {
            await _userStore.DeleteAsync(identityUser);
        }

        public async void DeleteRole(IdentityUser identityUser, string role)
        {
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.RemoveFromRoleAsync(identityUser, role);
        }
        
        public IEnumerable<IdentityUser> GetUsers(string role)
        {
            return _userStore.GetUsersInRoleAsync(role).Result;
        }
        public async void GiveRole(IdentityUser identityUser, string role)
        {
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.AddToRoleAsync(identityUser, role);
        }
        
        public async void CreateUser(IdentityUser identityUser)
        {
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.CreateAsync(identityUser);
        }
        
        
        #region VerificationRequest

        public IEnumerable<VerificationRequest> GetVerificationRequests()
        {
            return _ctx.VerificationRequests.AsEnumerable();
        }

        public void CreateVerificationRequest(VerificationRequest verificationRequest)
        {
            _ctx.VerificationRequests.Add(verificationRequest);
        }
        
        #endregion
    }
}