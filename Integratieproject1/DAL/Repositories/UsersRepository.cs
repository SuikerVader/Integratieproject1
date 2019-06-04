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

        #region Users
        
        #region Gets

        // Return user based on ID
        public CustomUser GetUser(string id)
        {
            return _userStore.FindByIdAsync(id).Result;
        }

        // Return a list of all users in given role
        public IEnumerable<CustomUser> GetUsers(string role)
        {
            return _userStore.GetUsersInRoleAsync(role).Result;
        }

        // Return user based on email
        public CustomUser GetUserByEmail(string email)
        {            
            return _userStore.FindByEmailAsync(email.ToUpper()).Result;
        }

        // Return user based on username
        public CustomUser GetUserByUsername(string username)
        {
            return _userStore.FindByNameAsync(username.ToUpper()).Result;
        }

        // Get surname of user
        public string GetSurname(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Surname;
        }

        // Get name of user
        public string GetName(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Name;
        }

        // Get sex of user
        public string GetSex(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Sex;
        }

        // Get age of user
        public int GetAge(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Age;
        }

        // Get zipcode of user
        public string GetZipcode(CustomUser customUser)
        {
            return _ctx.Users.Single(u => u.Id == customUser.Id).Zipcode;
        }

        #endregion

        // Blocks given user by given days
        public async void BlockUser(CustomUser identityUser, int days)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            await userManager.SetLockoutEndDateAsync(identityUser, DateTime.Now.AddDays(days));
        }

        // Creates new user based on given user
        public async void CreateUser(CustomUser identityUser)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            await userManager.CreateAsync(identityUser);
        }

        // Updates user based on given user
        public async void UpdateUser(CustomUser identityUser)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            await userManager.UpdateAsync(identityUser);
        }

        // Deletes given user from database
        public async void DeleteUser(CustomUser identityUser)
        {
            await _userStore.DeleteAsync(identityUser);
        }


        #endregion

        #region Roles

        // Check if given user is in given role
        // Returns true if user is in given role
        // Returns false if user is not in given role
        public bool IsInRole(CustomUser user, string role)
        {
            return _userStore.IsInRoleAsync(user, role).Result;
        }

        // Assign the given role to given user
        public async void GiveRole(CustomUser identityUser, string role)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            await userManager.AddToRoleAsync(identityUser, role);
        }

        // Delete a role based on role from given user
        public async void DeleteRole(CustomUser identityUser, string role)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore,null,null,null,null,null,null,null,null);
            await userManager.RemoveFromRoleAsync(identityUser, role);
        }

        #endregion

        #region VerificationRequest

        // Let user be ready to be verified
        public async void AskVerifyAsync(CustomUser user)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            user.AskVerify = true;
            await userManager.UpdateAsync(user);
        }

        // Verifies a given user
        public void Verify(CustomUser user)
        {
            UserManager<CustomUser> userManager = new UserManager<CustomUser>(_userStore, null, null, null, null, null, null, null, null);
            user.Verified = true;
            user.AskVerify = false;
            userManager.UpdateAsync(user);
        }

        // Returns a list of all users who need to be verified
        public IList<CustomUser> GetRequests()
        {
            return _ctx.Users
                .Where(u => u.AskVerify == true)
                .AsEnumerable().ToList();
        }

        #endregion

    }
}