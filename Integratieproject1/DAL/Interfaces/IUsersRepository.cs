using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IUsersRepository
    {
        IdentityUser GetUser(string id);
        void DeleteUser(IdentityUser identityUser);
        void DeleteRole(IdentityUser identityUser, string role);
        IEnumerable<IdentityUser> GetUsers(string role);
        void GiveRole(IdentityUser identityUser, string role);
        void CreateUser(IdentityUser identityUser);

    }
}