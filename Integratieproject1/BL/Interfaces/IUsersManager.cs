using System;
using System.Collections.Generic;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.BL.Interfaces
{
    public interface IUsersManager
    {
        CustomUser GetUser(string userId);
        void DeleteUser(string userId);
        void DeleteRole(string userId, string role);
        IList<CustomUser> GetUsers(string role);
        void GiveRole(string userId, string role);
        void CreateUser(CustomUser identityUser);
        IEnumerable<VerificationRequest> GetVerificationRequests();
        void CreateVerificationRequest(VerificationRequest verificationRequest);
        VerificationRequest CreateVerificationRequest(CustomUser user, string request);
        void HandleVerificationRequest(VerificationRequest verificationRequest, bool accepted);
    }
}