using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CityOfIdeasDbContext _ctx;
        private UserStore<CustomUser> _userStore;
        
        public UsersRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
           
            _ctx = unitOfWork.Ctx;
            _userStore = new UserStore<CustomUser>(_ctx);
        }

        public CustomUser GetUser(string id)
        {
            return _userStore.FindByIdAsync(id).Result;
        }
        
        public async void DeleteUser(CustomUser identityUser)
        {
            await _userStore.DeleteAsync(identityUser);
        }

        public async void DeleteRole(CustomUser identityUser, string role)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.RemoveFromRoleAsync(identityUser, role);
        }

        public IEnumerable<CustomUser> GetUsers(string role)
        {
            return _userStore.GetUsersInRoleAsync(role).Result;
        }
        public async void GiveRole(CustomUser identityUser, string role)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.AddToRoleAsync(identityUser, role);
        }
        
        public async void CreateUser(CustomUser identityUser)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            userManager.CreateAsync(identityUser);
        }

        public bool IsInRole(CustomUser user, string role)
        {
           return _userStore.IsInRoleAsync(user, role).Result;
        }
        
        
        #region VerificationRequest

        public async void AskVerifyAsync(CustomUser user)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            user.AskVerify = true;
            await userManager.UpdateAsync(user);
        }

        public void Verify(CustomUser user)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            user.Verified = true;
            user.AskVerify = false;
            userManager.UpdateAsync(user);
        }

        public IList<CustomUser> GetRequests()
        {
            return _ctx.Users
                .Where(u => u.AskVerify == true)
                .AsEnumerable().ToList();
        }

        #endregion

        public async void BlockUser(CustomUser identityUser, int days)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            await userManager.SetLockoutEndDateAsync(identityUser, DateTime.Now.AddDays(days));
        }

        public CustomUser GetUserByEmail(string email)
        {            
            return _userStore.FindByEmailAsync(email.ToUpper()).Result;
        }

        public string GetSurname(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Surname;
        }

        public string GetName(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Name;
        }

        public string GetSex(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Sex;
        }

        public int GetAge(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Age;
        }

        public string GetZipcode(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Zipcode;
        }
    }
}