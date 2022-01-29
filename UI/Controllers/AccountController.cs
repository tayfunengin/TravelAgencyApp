using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Authentication;
using Repository.ViewModels;
using Service.AppUserRoleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Filters;
using UI.Tools;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppUserRole> roleManager;
        public AppUserRoleService appUserRoleService = null;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppUserRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.appUserRoleService = new AppUserRoleService(userManager, roleManager);
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            TempData["url"] = returnUrl;
            TempData["referer"] = Request.Headers["Referer"].ToString();
            AppUser user = await userManager.GetUserAsync(User);

            if (user == null || HttpContext.Session.GetString("login") == null)
            {
               
                return View();
            }
            else
            {              
               return RedirectToAction("Index", "Home", new { area = "Dashboard" });      
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string referer = TempData["referer"] as string;
                    AppUser user = await userManager.FindByNameAsync(loginVM.UserName);
                    if (user != null)
                    {                        
                        await signInManager.SignOutAsync();
                        var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                        if (result.Succeeded)
                        {
                            string roleName = await appUserRoleService.ReturnRoleNameForUser(user);
                            HttpContext.Session.SetString("login", user.UserName + "," + roleName); 
                            if (TempData["url"] != null && Url.IsLocalUrl(TempData["url"].ToString()))
                                return Redirect(TempData["url"].ToString());
                            else
                            {
                                // eğer seesion ın süresi dolduysa ve dashboard ekranında tekrar login e yönlendirilme yapıldıysa, login başarılı olduktan sonra ilk istek gönderilen sayfaya yönlendirme yapılır. İlk kez login olunuyorsa ana sayfaya yönelendirilir.
                                if (!string.IsNullOrWhiteSpace(referer) && referer.Contains("Dashboard"))
                                {
                                    return Redirect(referer);
                                }
                                else
                                {
                                    return RedirectToAction("Login","Account");                                
                                }
                            }                                
                        }
                        else
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Username and password do not match.");
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"User named {loginVM.UserName} could not be found.");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }

            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Remove("login");
            TempData["login"] = null;
            TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "You have logged out successfully.");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            string referer = Request.Headers["Referer"].ToString();
            TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "You are not authorized to the page.");
            if (string.IsNullOrEmpty(referer))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(referer);
        }
      
    }
}
