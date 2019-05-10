using System.Collections.Generic;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IUsersRepository
    {
        CustomUser GetUser(string id);
        void DeleteUser(CustomUser identityUser);
        void DeleteRole(CustomUser identityUser, string role);
        IEnumerable<CustomUser> GetUsers(string role);
        void GiveRole(CustomUser identityUser, string role);
        void CreateUser(CustomUser identityUser);
        IEnumerable<VerificationRequest> GetVerificationRequests();
        void CreateVerificationRequest(VerificationRequest verificationRequest);
        void SetVerificationRequestHandled(VerificationRequest verificationRequest);
    }
}