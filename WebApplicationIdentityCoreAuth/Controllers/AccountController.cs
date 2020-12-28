using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationIdentityCoreAuth.Models;
using WebApplicationIdentityCoreAuth.Models.ViewModel;

namespace WebApplicationIdentityCoreAuth.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _usermanager;
        private SignInManager<ApplicationUser> _signinmanager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _usermanager = userManager;
            _signinmanager = signInManager;
        }
        public async Task<IActionResult> Logout()
        {
           await _signinmanager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
      //  [HttpGet][HttpPost]
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {

           var user = await _usermanager.FindByEmailAsync(Email);
            if (user==null)
            {
                return Json(true);

            }
            return Json($"Email {Email} Is already In Use");
        }

         [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,City = model.City };
                var result = await _usermanager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signinmanager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("index", "Home");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);


        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
        

            if (ModelState.IsValid)
            {
                var result = await _signinmanager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,true);
                if (result.Succeeded)
                {

                    if (string.IsNullOrEmpty(returnUrl)&&Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                       //return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("index", "Home");

                }

                
                    ModelState.AddModelError("", "Invalid Login");
                

            }
            return View(model);


        }
    }
}
