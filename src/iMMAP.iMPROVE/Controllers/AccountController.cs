using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models.ViewModels;
using iMMAP.iMPROVE.Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iMMAP.iMPROVE.Controllers
{
    public class AccountController : Controller
    {
        public IUsersService usersService { get; private set; }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public AccountController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        #region login business logic

        //
        // GET: /Manage/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Skin = "red";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Manage/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await usersService.UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            ViewBag.Skin = "red";
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await usersService.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Manage/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}