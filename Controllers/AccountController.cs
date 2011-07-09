using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Web.Routing;
using System.Web.Security;
using BetterTaskList.Models;
using System.Drawing.Imaging;
using BetterTaskList.Helpers;
using BetterTaskList.Extensions;

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
                        return RedirectToAction("Profile", "Home");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "The username or password provided is incorrect. Please try again.";
                    // ModelState.AddModelError("", "The user name or password provided is incorrect.");
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

            return RedirectToAction("Logon");
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
                        Profile profile = new Profile();

                        profile.UserId = UserHelpers.GetUserId(model.Email);
                        profile.FirstName = model.FirstName;
                        profile.LastName = model.LastName;
                        profile.FullName = model.FirstName + " " + model.LastName;
                        profile.TimeZone = TimeZoneInfo.Local.Id;

                        db.Profiles.InsertOnSubmit(profile);
                        db.SubmitChanges();
                    }

                    FormsService.SignIn(model.Email, false /* createPersistentCookie */);
                    
                    // Send out a welcome aboard email to the registrant
                    new EmailNotificationHelpers().UserRegistrationEmail(model.Email);

                    return RedirectToAction("Queue", "Ticket", new { area = "Projects" });
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
        // URL: /Account/MyAccount
        // **************************************
        [Authorize]
        public ActionResult MyAccount()
        {
            ProfileRepository profileRepository = new ProfileRepository();
            Profile profile = profileRepository.GetUserProfile(User.Identity.Name);
            return View(profile);
        }

        [HttpPost, Authorize, ValidateInput(false)]
        public ActionResult MyAccount(FormCollection formCollection)
        {
            ProfileRepository profileRepository = new ProfileRepository();
            Profile profile = profileRepository.GetUserProfile(User.Identity.Name);

            if (!string.IsNullOrEmpty(formCollection["FirstName"]))
            {
                profile.FullName = formCollection["FirstName"] + " " + formCollection["LastName"];
            }

            try
            {
                UpdateModel(profile);
                profileRepository.Save();

                TempData["message"] = "Awesome! your profile has been updated. Thank you for making it easier for others to communicate with you.";
                return View(profile); 
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
                    new EmailNotificationHelpers().ForgotMyPasswordEmail(userEmailAddress, passwordRecovered);
                    TempData["message"] = "Your new password has been emailed to the provided email address. Have a great day.";
                    return RedirectToAction("LogOn");
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

        [HttpPost, Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    TempData["message"] = "Your password has been changed successfully";
                    return RedirectToAction("Queue", "Ticket", new {area = "Tickets"});
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

        // **************************************
        // URL: /Account/ChangePicture
        // **************************************


        [HttpPost, Authorize]
        public ActionResult ChangePicture()
        {
            try
            {
                if (Request.Files.Count == 1 && Request.Files[0].ContentLength < 562144)
                {

                    ProfileRepository profileRepository = new ProfileRepository();

                    Guid userId = UserHelpers.GetUserId(User.Identity.Name);
                    Profile userProfile = profileRepository.GetUserProfile(userId);
                    var pictureName = new StringBuilder(12).AppendRandomString(12).ToString();

                    var default128x128 = Server.MapPath(Url.AccountPicture(pictureName, "128x128"));
                    var default64x64 = Server.MapPath(Url.AccountPicture(pictureName, "64x64"));
                    var default32x32 = Server.MapPath(Url.AccountPicture(pictureName, "32x32"));
                    var default16x16 = Server.MapPath(Url.AccountPicture(pictureName, "16x16"));

                    while (System.IO.File.Exists(default128x128) || System.IO.File.Exists(default64x64) || System.IO.File.Exists(default32x32) || System.IO.File.Exists(default16x16))
                    {
                        pictureName = new StringBuilder(12).AppendRandomString(12).ToString();
                        default128x128 = Server.MapPath(Url.AccountPicture(pictureName, "128x128"));
                        default64x64 = Server.MapPath(Url.AccountPicture(pictureName, "64x64"));
                        default32x32 = Server.MapPath(Url.AccountPicture(pictureName, "32x32"));
                        default16x16 = Server.MapPath(Url.AccountPicture(pictureName, "16x16"));
                    }

                    using (var srcImage = Image.FromStream(Request.Files[0].InputStream))
                    {
                        using (var destImage = srcImage.ResizeTo(128, 128)) { destImage.Save(default128x128, ImageFormat.Png); }
                        using (var destImage = srcImage.ResizeTo(64, 64)) { destImage.Save(default64x64, ImageFormat.Png); }
                        using (var destImage = srcImage.ResizeTo(32, 32)) { destImage.Save(default32x32, ImageFormat.Png); }
                        using (var destImage = srcImage.ResizeTo(16, 16)) { destImage.Save(default16x16, ImageFormat.Png); }
                    }

                    if (userProfile.PictureName != null)
                    {
                        DeletePicture(userProfile.PictureName, "128x128");
                        DeletePicture(userProfile.PictureName, "64x64");
                        DeletePicture(userProfile.PictureName, "32x32");
                        DeletePicture(userProfile.PictureName, "16x16");
                    }

                    userProfile.PictureName = pictureName;
                    profileRepository.Save();
                }

                return RedirectToAction("MyAccount");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCurrentPicture()
        {
            ProfileRepository profileRepository = new ProfileRepository();

            //TODO: why is the userHelpers being used below if an isntance of profileRepository exist?
            Guid userId = UserHelpers.GetUserId(User.Identity.Name);
            Profile userProfile = profileRepository.GetUserProfile(userId);

            if (userProfile.PictureName != null)
            {
                userProfile.PictureName = null;
                profileRepository.Save();

                DeletePicture(userProfile.PictureName, "128x128");
                DeletePicture(userProfile.PictureName, "64x64");
                DeletePicture(userProfile.PictureName, "32x32");
                DeletePicture(userProfile.PictureName, "16x16");
            }

            return RedirectToAction("MyAccount");
        }

        void DeletePicture(string name, string size)
        {
            var path = Server.MapPath(Url.AccountPicture(name, size));
            try { System.IO.File.Delete(path); }
            catch { }
        }



    }
}
