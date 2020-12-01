using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdministrationController> _logger;

        public UserManager<ApplicationUser> _userManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, ILogger<AdministrationController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.Error = $"User with Id {userId} cannot be found.";
                return View("NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (var claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if(existingUserClaims.Any(x => x.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.Error = $"User with Id {model.UserId} cannot be found.";
                return View("NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims.");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user, model.Claims.Where(x => x.IsSelected).Select(x => new Claim(x.ClaimType, x.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims.");
                return View(model);
            }



            return RedirectToAction("EditUser", new { Id = model.UserId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ViewBag.Error = $"Use with Id = {Id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            EditUserViewModel model = new EditUserViewModel
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = userClaims.Select(x => x.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.Error = $"Use with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.Name;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return RedirectToAction("ListUsers");

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ViewBag.Error = $"User with Id = {Id} cannot be found.";

                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction("ListUsers");

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }

            return View("ListUsers");
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} was not found.";
                return RedirectToAction("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} was not found.";
                return RedirectToAction("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId, string userName)
        {
            ViewBag.userId = userId;
            ViewBag.userName = userName;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.Error = $"User {user.UserName} cannot be found.";
                return View("NotFound");
            }

            var model = new List<ManageUserRolesViewModel>();

            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var role in _roleManager.Roles)
            {
                var tmpModel = new ManageUserRolesViewModel();
                if (rolesForUser.Contains(role.Name))
                {
                    tmpModel.IsChecked = true;
                }
                else
                {
                    tmpModel.IsChecked = false;
                }
                tmpModel.RoleId = role.Id;
                tmpModel.RoleName = role.Name;
                model.Add(tmpModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Error = $"User {user.UserName} cannot be found.";
                return View("NotFound");
            }


            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", $"Cannot remove {user.UserName} from current roles.");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsChecked).Select(x => x.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", $"Cannot add {user.UserName} to selected roles.");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                ViewBag.Error = $"Role with Id = {Id} cannot be found.";

                return View("NotFound");
            }

            try
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("ListRoles");

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return View("ListRoles");
            }
            catch (Exception ex)
            {
                ViewBag.role = role.Name;
                _logger.LogError($"Error deleting role: {ex.Message}");

                return View("Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found.";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsChecked = true;
                }
                else
                {
                    userRoleViewModel.IsChecked = false;
                }
                model.Add(userRoleViewModel);

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.Error = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsChecked && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsChecked && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
