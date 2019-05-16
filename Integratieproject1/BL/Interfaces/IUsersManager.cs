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
        string GetSurname(CustomUser customUser);
        string GetName(CustomUser customUser);
        string GetSex(CustomUser customUser);
        int GetAge(CustomUser customUser);
        string GetZipcode(CustomUser customUser);
        IList<CustomUser> GetUsers(string role);
        void CreateUser(CustomUser identityUser);
        void DeleteUser(string userId);
        void BlockUser(string userId, int days);

        #endregion

        #region Role

        void DeleteRole(string userId, string role);
        void GiveRole(string userId, string role);
        bool IsInRole(string userId, string role);

        #endregion

        #region VerificationRequest

        IEnumerable<VerificationRequest> GetVerificationRequests();
        void CreateVerificationRequest(VerificationRequest verificationRequest);
        VerificationRequest CreateVerificationRequest(CustomUser user, string request);
        void HandleVerificationRequest(VerificationRequest verificationRequest, bool accepted);

        #endregion
    }
}