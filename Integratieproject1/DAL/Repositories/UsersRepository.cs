using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private UserStore<IdentityUser> _userStore;
        private readonly CityOfIdeasDbContext ctx = null;
        public UsersRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            ctx = unitOfWork.Ctx;
            _userStore = new UserStore<IdentityUser>(ctx);
        }

        public IdentityUser GetUser(string id)
        {
            IdentityUser identityUser = _userStore.FindByIdAsync(id).Result;
            return identityUser;
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
    }
}