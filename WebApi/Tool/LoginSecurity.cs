using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Repository.Authentication;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Tool
{
    public class LoginSecurity : ILoginSecurity
    {
        
        //private readonly UserManager<AppUser> userManager;
        //private readonly SignInManager<AppUser> signInManager;

        //public LoginSecurity(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        //{
        //    this.userManager = userManager;
        //    this.signInManager = signInManager;
        //}
        //public async Task<bool> Login(string username, string password)
        //{
        //    var appUser = await userManager.FindByNameAsync(username);
        //    await signInManager.SignOutAsync();
        //    var result = await signInManager.PasswordSignInAsync(appUser, password, false, false);
        //    if (result.Succeeded)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        private readonly ApplicationDbContext context;

        public LoginSecurity(ApplicationDbContext context)
        {
            this.context = context;
        }
        public bool Login(string username, string password)
        {
            // NorthwindEntities db = new NorthwindEntities();

        

            //bool result = db.Users.Any(x => x.UserName == username && x.Password == password);
            bool result = this.context.Users.Any(x => x.UserName.Equals(username) && x.PasswordHash.Equals(password));
            return result;
        }
    }
}
