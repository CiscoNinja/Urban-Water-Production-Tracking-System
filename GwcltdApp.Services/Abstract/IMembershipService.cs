using GwcltdApp.Entities;
using GwcltdApp.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwcltdApp.Services
{
    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);
        User CreateUser(string username, string email, string password, int gwclarea, int gwclstation, int[] roles);
        User GetUser(int userId);
        User GetUserStation(string username);
        User GetUserRegion(string username);
        List<Role> GetUserRoles(string username);
    }
}
