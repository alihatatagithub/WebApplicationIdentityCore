using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationIdentityCoreAuth.Models;
using WebApplicationIdentityCoreAuth.Models.ViewModel;

namespace WebApplicationIdentityCoreAuth.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<ApplicationUser> _usermanager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _rolemanager = roleManager;
            _usermanager = userManager;
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
                IdentityRole newRole = new IdentityRole { Name = model.RoleName };

                IdentityResult result = await _rolemanager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _rolemanager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }

            var model = new EditRoleViewModel { Id = role.Id, RoleName = role.Name };
            foreach (var user in _usermanager.Users)
            {
                if (await _usermanager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                };

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _rolemanager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound();

            }
            role.Name = model.RoleName;
            var result = await _rolemanager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ListRoles));

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);

        }

        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _usermanager.FindByIdAsync(userId);
            if (user==null)
            {
                return NotFound();

            }
            var ExistingUserClaim = await _usermanager.GetClaimsAsync(user);

            var model = new UserClaimViewModel { UserId = userId };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {

                UserClaim userClaim = new UserClaim { ClaimType = claim.Type };
                if (ExistingUserClaim.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);

                
            }
            return View(model);


        }

        #region EditUserInRole

        //public async Task<IActionResult> EditUserInRole(string roleId)
        //{
        //    ViewBag.RoleId = roleId;

        //    var role = await _rolemanager.FindByIdAsync(roleId);

        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new List<UserRoleViewModel>();

        //    foreach (var user in _usermanager.Users)
        //    {
        //        var userRoleViewModel = new UserRoleViewModel { UserId = user.Id, UserName = user.UserName };
        //        if (await _usermanager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRoleViewModel.IsSelected = true;
        //        }

        //        else
        //        {
        //            userRoleViewModel.IsSelected = false;
        //        }
        //        model.Add(userRoleViewModel);

        //    }

        //    return View(model);





        //}
        //[HttpPost]
        //public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        //{

        //    var role = await _rolemanager.FindByIdAsync(roleId);
        //    if (role==null)
        //    {
        //        return NotFound();

        //    }
        //    for (int i = 0; i < model.Count; i++)
        //    {

        //        var user = await _usermanager.FindByIdAsync(model[i].UserId);

        //        IdentityResult result = null;

        //        if (model[i].IsSelected && !(await _usermanager.IsInRoleAsync(user,role.Name)))
        //        {

        //            result = await _usermanager.AddToRoleAsync(user, role.Name);

        //        }
        //    }

        //}
        #endregion




    }
}