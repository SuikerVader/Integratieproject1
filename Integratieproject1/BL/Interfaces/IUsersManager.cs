using System;
using System.Collections.Generic;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.BL.Interfaces
{
    public interface IUsersManager
    {
        #region User

        CustomUser GetUser(string userId);
        CustomUser GetUserByEmail(string email);
        CustomUser GetUserByUsername(string username);
        string GetSurname(CustomUser customUser);
        string GetName(CustomUser customUser);
        string GetSex(CustomUser customUser);
        int GetAge(CustomUser customUser);
        string GetZipcode(CustomUser customUser);
        IList<CustomUser> GetUsers(string role);
        IList<CustomUser> GetUsersBySort(string role, string sortOrder);
        void CreateUser(CustomUser identityUser);
        void UpdateUser(CustomUser identityUser);
        void DeleteUser(string userId);
        void BlockUser(string userId, int days);

        #endregion

        #region Role

        void DeleteRole(string userId, string role);
        void GiveRole(string userId, string role);
        bool IsInRole(string userId, string role);

        #endregion
        
        #region VerificationRequest

        void AskVerify(string userId);
        void Verify(string userId);
        IList<CustomUser> GetRequests();
        
        #endregion

    }
}