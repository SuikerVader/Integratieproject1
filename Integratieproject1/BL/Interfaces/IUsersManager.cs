using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.BL.Interfaces
{
    public interface IUsersManager
    {
        IdentityUser GetUser(string userId);
        void DeleteUser(string userId);
        void DeleteRole(string userId, string role);
        IList<IdentityUser> GetUsers(string role);
        void GiveRole(string userId, string role);
        void CreateUser(IdentityUser identityUser);
        void BlockUser(IdentityUser identityUser);
    }
}