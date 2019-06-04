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

        #region User

        #region Gets

        // Returns user based on ID
        public CustomUser GetUser(string userId)
        {
            return _usersRepository.GetUser(userId);
        }

        // Returns a list of all users in given role
        public IList<CustomUser> GetUsers(string role)
        {
            return _usersRepository.GetUsers(role).ToList();
        }

        // Return list of users in given role sorted by: username, surname, name, email, age
        public IList<CustomUser> GetUsersBySort(string role, string sortOrder)
        {
            IEnumerable<CustomUser> users = GetUsers(role).ToList();
            switch (sortOrder)
            {
                case "username_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "Surname":
                    users = users.OrderBy(u => u.Surname);
                    break;
                case "surname_desc":
                    users = users.OrderByDescending(u => u.Surname);
                    break;
                case "Name":
                    users = users.OrderBy(u => u.Name);
                    break;
                case "name_desc":
                    users = users.OrderByDescending(u => u.Name);
                    break;
                case "Email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(p => p.Email);
                    break;
                case "Age":
                    users = users.OrderBy(u => u.Age);
                    break;
                case "age_desc":
                    users = users.OrderByDescending(u => u.Age);
                    break;
                default:
                    users = users.OrderBy(u => u.UserName);
                    break;
            }
            return users.ToList();
        }

        // Get user based on email
        public CustomUser GetUserByEmail(string email)
        {
            return _usersRepository.GetUserByEmail(email);
        }

        // Get user based on username
        public CustomUser GetUserByUsername(string username)
        {
            return _usersRepository.GetUserByUsername(username);
        }

        // Get surname of user based on ID
        public string GetSurname(CustomUser customUser)
        {
            return _usersRepository.GetSurname(customUser);
        }

        // Get name of user based on ID
        public string GetName(CustomUser customUser)
        {
            return _usersRepository.GetName(customUser);
        }

        // Get sex of user based on ID
        public string GetSex(CustomUser customUser)
        {
            return _usersRepository.GetSex(customUser);
        }

        // Get age of user based on ID
        public int GetAge(CustomUser customUser)
        {
            return _usersRepository.GetAge(customUser);
        }

        // Get zipcode of user based on ID
        public string GetZipcode(CustomUser customUser)
        {
            return _usersRepository.GetZipcode(customUser);
        }

        #endregion
        
        // Blocks user based on ID by given days
        public void BlockUser(string userId, int days)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.BlockUser(identityUser, days);
        }

        // Creates new user based on given user
        public void CreateUser(CustomUser identityUser)
        {
            _usersRepository.CreateUser(identityUser);
        }

        // Updates user based on given user
        public void UpdateUser(CustomUser identityUser)
        {
            _usersRepository.UpdateUser(identityUser);
        }

        // Deletes user based on ID
        public void DeleteUser(string userId)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.DeleteUser(identityUser);
        }

        #endregion

        #region Roles

        // Get user based on ID and assign the given role to user
        public void GiveRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            if (IsInRole(userId, "USER"))
            {
                _usersRepository.DeleteRole(identityUser, "USER");
            }
            _usersRepository.GiveRole(identityUser, role);
        }
        
        // Check if user based on ID is in given role
        // Returns true if user is in given role
        // Returns false if user is not in given role
        public bool IsInRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            return _usersRepository.IsInRole(identityUser, role);
        }

        // Delete a role based on role from a user based on ID
        public void DeleteRole(string userId, string role)
        {
            CustomUser identityUser = GetUser(userId);
            _usersRepository.DeleteRole(identityUser, role);
        }

        #endregion

        #region VerificationRequest

        // Let user be ready to be verified
        public void AskVerify(string userId)
        {
            CustomUser user = GetUser(userId);
            _usersRepository.AskVerifyAsync(user);
        }

        // Verifies a user based on ID
        public void Verify(string userId)
        {
            CustomUser user = GetUser(userId);
            _usersRepository.Verify(user);
        }

        // Returns a list of all users who need to be verified
        public IList<CustomUser> GetRequests()
        {
            return _usersRepository.GetRequests();
        }
        #endregion

        

       
    }
}