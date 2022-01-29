using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Authentication;
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
   // [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppUserRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public AppUserRoleService appUserRoleService = null;

        public RoleController(RoleManager<AppUserRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.appUserRoleService = new AppUserRoleService(userManager, roleManager);
        }

        // GET: RoleController
        public ActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        // GET: RoleController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var appUserRole = await roleManager.FindByIdAsync(id);

            if (appUserRole != null)
            {
                TempData["Role"] = appUserRole;
                var appUsers = await userManager.GetUsersInRoleAsync(appUserRole.Name);
                List<AppUser> usersForRole = new List<AppUser>();
                foreach (var user in appUsers)
                {
                    usersForRole.Add(user);
                }
                return View(usersForRole);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"A role defined for Id: {id} could not be found.");
                return RedirectToAction("Index");
            }

        }
        public async Task<ActionResult> RemoveRoleFromUser(string userId, string role)
        {
            AppUser appUser = await userManager.FindByIdAsync(userId);
            AppUserRole appUserRole = await roleManager.FindByNameAsync(role);
            if (appUser != null)
            {
                var result = await userManager.RemoveFromRoleAsync(appUser, role);
                if (result.Succeeded)
                {
                    TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"User named {appUser.UserName} has been deauthorized for role: {role}.");
                    return RedirectToAction("Details", new { id = appUserRole.Id.ToString() });
                }
                else
                {
                    TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "An error has occured.");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No user with Id {userId} could be found.");
                return RedirectToAction("Details", userId);
            }
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            List<string> unCreatedRoles = appUserRoleService.UnCreatedRoleList();
            if (unCreatedRoles.Count > 0)
            {
                TempData["roles"] = new SelectList(unCreatedRoles);
                return View();
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.info.ToString(), "All roles are already defined.");
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUserRole appUserRole)
        {
            if (appUserRole != null)
            {
                try
                {
                    var result = await roleManager.CreateAsync(appUserRole);
                    if (result.Succeeded)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"Role: {appUserRole.Name} has been created successfully.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        TempData["roles"] = new SelectList(appUserRoleService.UnCreatedRoleList());
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
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity cannot be null!");
                TempData["roles"] = new SelectList(appUserRoleService.UnCreatedRoleList());
                return View();
            }
        }

        // GET: RoleController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            AppUserRole appUserRole = await roleManager.FindByIdAsync(id);
            if (appUserRole != null)
            {
                return View(appUserRole);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No role with Id {id} could be found.");
                return RedirectToAction("Index");
            }
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AppUserRole appUserRole)
        {
            if (appUserRole != null)
            {
                try
                {
                    AppUserRole updatedRole = await roleManager.FindByIdAsync(appUserRole.Id.ToString());
                    if (updatedRole != null)
                    {
                        updatedRole.Name = appUserRole.Name;
                        updatedRole.Description = appUserRole.Description;

                        var result = await roleManager.UpdateAsync(updatedRole);
                        if (result.Succeeded)
                        {
                            TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"Role: {appUserRole.Name} has been updated successfully.");
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
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No role with Id {appUserRole.Id} could be found!");
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
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity cannot be null!");
                return View();
            }
        }

        // GET: RoleController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            AppUserRole appUserRole = await roleManager.FindByIdAsync(id);
            if (appUserRole != null)
            {
                Task<bool> isAssigned = appUserRoleService.IsRoleAssignedForAnUser(appUserRole);
                if (isAssigned.Result)
                {
                    TempData["Notification"] = Notification.Message(NotificationType.warning.ToString(), "To be able to delete the role, you need to remove it from the users it is assigned to.");
                    return RedirectToAction("Index");
                }
                return View(appUserRole);
            }
            else
            {
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), $"No role with Id {id} could be found!.");
                return RedirectToAction("Index");
            }
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AppUserRole appUserRole)
        {
            if (appUserRole != null)
            {
                try
                {
                    AppUserRole deletedRole = await roleManager.FindByIdAsync(appUserRole.Id.ToString());
                    var result = await roleManager.DeleteAsync(deletedRole);
                    if (result.Succeeded)
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.success.ToString(), $"Role: {deletedRole} has been deleted successfully.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "An error occurred.");
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
                TempData["Notification"] = Notification.Message(NotificationType.error.ToString(), "Entity cannot be null!");
                return View();
            }
        }
    }
}
