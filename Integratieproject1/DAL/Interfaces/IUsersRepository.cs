using System.Collections.Generic;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IUsersRepository
    {
       
        #region Users
        
        #region Gets

        CustomUser GetUser(string id);
        IEnumerable<CustomUser> GetUsers(string role);
        CustomUser GetUserByEmail(string email);
        CustomUser GetUserByUsername(string username);
        string GetSurname(CustomUser customUser);
        string GetName(CustomUser customUser);
        string GetSex(CustomUser customUser);
        int GetAge(CustomUser customUser);
        string GetZipcode(CustomUser customUser);

        #endregion

        void BlockUser(CustomUser identityUser, int days);
        void CreateUser(CustomUser identityUser);

        void UpdateUser(CustomUser identityUser);
        void DeleteUser(CustomUser identityUser);

        #endregion
        
        #region Roles

        bool IsInRole(CustomUser user, string role);
        void GiveRole(CustomUser identityUser, string role);
        void DeleteRole(CustomUser identityUser, string role);
        
        #endregion
        
        #region VerificationRequest

        void AskVerifyAsync(CustomUser user);
        void Verify(CustomUser user);
        IList<CustomUser> GetRequests();

        #endregion
    }
}