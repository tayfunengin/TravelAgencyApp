using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Authentication;
using Repository.ViewModels;
using Service.AppUserRoleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Filters;
using UI.Tools;

namespace UI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [TypeFilter(typeof(AcFilter))]
    //[TypeFilter(typeof(AuthFilter))]
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<AppUserRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public AppUserRoleService appUserRoleService = null;

        public UserController(RoleManager<AppUserRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.appUserRoleService = new AppUserRoleService(userManager, roleManager);
        }

        public async Task<ActionResult> Index()
        {
            List<UserAndRoleVM> usersAndRoles = new List<UserAndRoleVM>();
            foreach (var user in userManager.Users.ToList())
            {
                UserAndRoleVM userAndRole = new UserAndRoleVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email
                };
                userAndRole.Role = await appUserRoleService.ReturnRoleNameForUser(user);           

                usersAndRoles.Add(userAndRole);
            }
            return View(usersAndRoles);
        }

        public async Task<ActionResult> Details(string id)
        {
            var appUser = await userManager.FindByIdAsync(id);

            if (appUser != null)
            {
                TempData["User"] = appUser;
                var roles = await userManager.GetRolesAsync(appUser);
                List<string> appUserRoleNames = new List<string>();
                foreach (var role in roles)
                {
                    appUserRoleNames.Add(role);
                }
                return View(appUserRoleNames);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No user with Id {id} could be found!");
                return RedirectToAction("Index");
            }
        }
     
        public ActionResult Create()
        {
            List<AppUserRole> appUserRoles = roleManager.Roles.ToList();
            ViewBag.users = new SelectList(appUserRoles, "Name");
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterVM registerVM)
        {
            List<AppUserRole> appUserRoles = roleManager.Roles.ToList();
            ViewBag.users = JsonConvert.SerializeObject(new SelectList(appUserRoles, "Name"));

            if (ModelState.IsValid)
            {
                AppUser userByEmail = await userManager.FindByEmailAsync(registerVM.Email);
                AppUser userByUserName = await userManager.FindByNameAsync(registerVM.UserName);
                if (userByEmail == null && userByUserName == null)
                {
                    try
                    {
                        AppUser user = new AppUser
                        {
                            FirstName = registerVM.FirstName,
                            LastName = registerVM.LastName,
                            UserName = registerVM.UserName,
                            Email = registerVM.Email
                        };

                        var result = await userManager.CreateAsync(user, registerVM.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, registerVM.Role);
                            TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"User named {user.UserName} has been created successfully with role: {registerVM.Role}.");
                            ViewData["roles"] = JsonConvert.DeserializeObject(ViewBag.users.ToString());
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index", "Error", ex);
                    }
                }
                else
                {
                    if (userByEmail != null)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"There is already a defined role for E-mail:{userByEmail.Email}.");
                        ViewBag.users = new SelectList(appUserRoles, "Name");
                        return View();
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"There is already a defined role for username:{userByUserName.UserName}.");
                        ViewBag.users = new SelectList(appUserRoles, "Name");
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.users = new SelectList(appUserRoles, "Name");
                return View();
            }

        }
 
        public async Task<ActionResult> Edit(string id)
        {
            List<AppUserRole> appUserRoles = roleManager.Roles.ToList();
            TempData["roles"] = new SelectList(appUserRoles, "Name");
            TempData["userRole"] = "";
            UserAndRoleVM userAndRoleVM = new UserAndRoleVM();
            AppUser appuser = await userManager.FindByIdAsync(id);

            if (appuser != null)
            {
                userAndRoleVM.Id = appuser.Id;
                userAndRoleVM.UserName = appuser.UserName;
                userAndRoleVM.FirstName = appuser.FirstName;
                userAndRoleVM.LastName = appuser.LastName;
                userAndRoleVM.Email = appuser.Email;
                var roles = await userManager.GetRolesAsync(appuser);
                if (roles.Count > 0)
                    userAndRoleVM.Role = roles[0];
                else
                    userAndRoleVM.Role = string.Empty;              
                TempData["userRole"] = userAndRoleVM.Role;
                return View(userAndRoleVM);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No user with Id {id} could be found.");
                TempData["roles"] = JsonConvert.SerializeObject(new SelectList(appUserRoles, "Name"));
                ViewData["roles"] = JsonConvert.DeserializeObject(TempData["roles"].ToString());
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserAndRoleVM userAndRoleVM)
        {
            List<AppUserRole> appUserRoles = roleManager.Roles.ToList();
            TempData["roles"] = JsonConvert.SerializeObject(new SelectList(appUserRoles, "Name"));

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedUser = await userManager.FindByIdAsync(userAndRoleVM.Id.ToString());
                    AppUser emailExist = await userManager.FindByEmailAsync(userAndRoleVM.Email);
                    AppUser userNameExist = await userManager.FindByNameAsync(userAndRoleVM.UserName);

                    if (
                        (emailExist == null && userNameExist == null) ||
                        (emailExist == null && userAndRoleVM.UserName == updatedUser.UserName) ||
                        (emailExist != null && userAndRoleVM.Email == updatedUser.Email)
                        )
                    {
                        updatedUser.UserName = userAndRoleVM.UserName;
                        updatedUser.Email = userAndRoleVM.Email;
                        updatedUser.FirstName = userAndRoleVM.FirstName;
                        updatedUser.LastName = userAndRoleVM.LastName;

                        bool roleExist = await roleManager.RoleExistsAsync(userAndRoleVM.Role);

                        if (roleExist)
                        {                            
                            var result = await userManager.UpdateAsync(updatedUser);

                            if (result.Succeeded)
                            {
                                var roles = await userManager.GetRolesAsync(updatedUser);
                                if (roles.Count>0)
                                {
                                    foreach (var role in roles)
                                    {
                                        if (role != userAndRoleVM.Role)
                                        {
                                            await userManager.RemoveFromRoleAsync(updatedUser, TempData["userRole"] as string);
                                            await userManager.AddToRoleAsync(updatedUser, userAndRoleVM.Role);
                                        }
                                    }
                                }
                                else
                                {
                                    await userManager.AddToRoleAsync(updatedUser, userAndRoleVM.Role);
                                }
                               
                                TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), "User has been updated successfully.");
                                ViewData["roles"] = JsonConvert.DeserializeObject(TempData["roles"].ToString());
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(error.Code, error.Description);
                                }
                                TempData["roles"] = new SelectList(appUserRoles, "Name");
                                return View();
                            }
                        }
                        else
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"Role {userAndRoleVM.Role} is not defined!");
                            TempData["roles"] = new SelectList(appUserRoles, "Name");
                            return View();
                        }
                    }
                    else
                    {
                        if (emailExist != null)
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"There is already another user with e-mail: {emailExist.Email}.");
                            TempData["roles"] = new SelectList(appUserRoles, "Name");
                            return View();
                        }
                        else
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"There is already another user with username:{userNameExist.UserName}.");
                            TempData["roles"] = new SelectList(appUserRoles, "Name");
                            return View();
                        }
                    }
                }

                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["roles"] = new SelectList(appUserRoles, "Name");
                return View();
            }

        }

        public async Task<ActionResult> Delete(string id)
        {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                return View(appUser);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No user with Id {id} could be found.");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AppUser appUser)
        {
            if (appUser != null)
            {
                try
                {
                    AppUser deletedUser = await userManager.FindByIdAsync(appUser.Id.ToString());
                    var result = await userManager.DeleteAsync(deletedUser);
                    if (result.Succeeded)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"User with username: {deletedUser.UserName} has been deleted successfully.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"An error occurred.");
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", ex);
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity cannot be empty.");
                return View();
            }

        }
    }
}
