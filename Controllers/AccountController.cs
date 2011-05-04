using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BetterTaskList.Models;
using BetterTaskList.Helpers;
using System.Security.Principal;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BetterTaskList.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                file.SaveAs("/Uploads/" + file.FileName);
            }
            return null;
        }



        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.Email, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // the user registration was a success now create their profile
                    using (var db = new BetterTaskListDataContext())
                    {
                        BetterTaskList.Models.Profile profile = new BetterTaskList.Models.Profile();

                        profile.UserId = UserHelpers.GetUserId(model.Email);
                        profile.FirstName = "";
                        profile.LastName = "";
                        profile.FullName = "";
                        profile.TimeZone = TimeZoneInfo.Local.Id;

                        db.Profiles.InsertOnSubmit(profile);
                        db.SubmitChanges();
                    }

                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);
                    // TODO: redirect to first time user area rather then home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/Welcome
        // **************************************
        [Authorize]
        public ActionResult Welcome()
        {
            ProfileRepository profileRepository = new ProfileRepository();
            Profile profile = profileRepository.GetUserProfile(User.Identity.Name);
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Welcome(FormCollection formCollection)
        {
            ProfileRepository profileRepository = new ProfileRepository();
            Profile profile = profileRepository.GetUserProfile(User.Identity.Name);

            try
            {
                UpdateModel(profile);
                profileRepository.Save();

                TempData["message"] = "Owesome! your profile has been updated. Thank you for making it easier for others to communicate with you.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }


        // **************************************
        // URL: /Account/ResetPassword
        // **************************************

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection formCollection)
        {
            string userEmailAddress = formCollection["UserEmailAddress"];

            if (!string.IsNullOrEmpty(userEmailAddress))
            {
                string passwordRecovered = UserHelpers.GetResetUserPassword(userEmailAddress);

                if (!string.IsNullOrEmpty(passwordRecovered))
                {
                    EmailNotificationHelpers.ForgotMyPasswordEmail(userEmailAddress, passwordRecovered);
                    TempData["message"] = "Your new password has been emailed to the provided email address. Have a great day.";
                }
                else
                {
                    TempData["errorMessage"] = "Sorry, the email address provided does not appear in our records.";
                }
                return View();
            }
            else
            {
                TempData["errorMessage"] = "Sorry, the email address provided does not appear in our records.";
                return View();
            }
        }


        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
