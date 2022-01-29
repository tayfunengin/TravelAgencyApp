using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Repository.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.AppUserRoleService
{
    public class AppUserRoleService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppUserRole> roleManager;

        public AppUserRoleService(UserManager<AppUser> userManager, RoleManager<AppUserRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<bool> IsRoleAssignedForAnUser(AppUserRole appUserRole)
        {
           
            var users =  userManager.Users;
            foreach (var user in users)
            {
               bool IsInRole = await userManager.IsInRoleAsync(user, appUserRole.Name);
                if (IsInRole)
                    return true;                
            }
            return false;
        }

        public List<string> UnCreatedRoleList()
        {
            List<string> roles = new List<string>();
            List<AppUserRole> createdRoles = roleManager.Roles.ToList();
            foreach (var role in Enum.GetNames(typeof(Role)))
            {                
                if (!createdRoles.Any(x => x.Name == role))
                    roles.Add(role);                       
            }
            return roles;
        }

        public async Task<string> ReturnRoleNameForUser(AppUser appUser)
        {
            string role = string.Empty;
            var roles = await userManager.GetRolesAsync(appUser);
            if (roles.Count > 0)
                role = roles[0];

            return role;
        }
    }
}
