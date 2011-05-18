using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Security;
using BetterTaskList.Models;
using System.Collections.Generic;

namespace BetterTaskList.Helpers
{
    public class UserHelpers
    {
        static BetterTaskListDataContext db = new BetterTaskListDataContext();

        public static Guid GetUserId(string userName)
        {
            return (from u in db.Users where u.UserName.Equals(userName) select u.UserId).SingleOrDefault();
        }

        //****************************************************
        // GetUserProfile
        //****************************************************
        public static Profile GetUserProfile(string userName)
        {
            Guid userId = (from u in db.Users where u.LoweredUserName.Equals(userName) select u.UserId).SingleOrDefault();
            return (from r in db.Profiles where r.UserId.Equals(userId) select r).SingleOrDefault();

        }

        public static Profile GetUserProfile(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u).SingleOrDefault();
        }

        public static Profile GetUserProfile(long id)
        {
            return (from u in db.Profiles where u.ProfileId.Equals(id) select u).SingleOrDefault();
        }


        //****************************************************
        // GetUserAvatarUrl
        //****************************************************
        public static string GetUserAvatarUrl(Guid userId, string size)
        {
            // get the picture name from the profile table
            string pictureName = (from r in db.Profiles where r.UserId.Equals(userId) select r.PictureName).Single();

            // if we dont have a value for pictureName then return the Default (Possibly they have not uploaded one)
            if (string.IsNullOrEmpty(pictureName))
                return string.Format("~/Content/Avatars/{0}_{1}.png", "Default", "128x128");

            // confirm the file exist (so that we dont return a broken url) 
            if (!File.Exists(HttpContext.Current.Server.MapPath(string.Format("~/Content/Avatars/Pictures/{0}_{1}.png", pictureName, size))))
                return string.Format("~/Content/Avatars/{0}_{1}.png", "Default", "128x128");

            // file exist and we have a picture name so return its path
            return string.Format("~/Content/Avatars/Pictures/{0}_{1}.png", pictureName, size);

        }

        public static string GetUserAvatarUrl(string userName, string size)
        {
            // get the userId for the provided userName
            Guid userId = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).Single();

            // using the obtained userId above pull the picture name
            string pictureName = (from r in db.Profiles where r.UserId.Equals(userId) select r.PictureName).Single();

            // if we dont have a value for pictureName then return the Default (Possibly they have not uploaded one)
            if (string.IsNullOrEmpty(pictureName))
                return string.Format("~/Content/Avatars/{0}_{1}.png", "Default", "128x128");

            // confirm the file exist (so that we dont return a broken url) 
            if (!File.Exists(HttpContext.Current.Server.MapPath(string.Format("~/Content/Avatars/Pictures/{0}_{1}.png", pictureName, size))))
                return string.Format("~/Content/Avatars/{0}_{1}.png", "Default", "128x128");

            // file exist and we have a picture name so return its path
            return string.Format("~/Content/Avatars/Pictures/{0}_{1}.png", pictureName, size);

        }

        //****************************************************
        // GetUserFullName
        //****************************************************
        public static string GetUserFullName(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FullName).SingleOrDefault();
        }

        public static string GetUserFullName(string userName)
        {
            Guid userId = GetUserId(userName);
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FullName).SingleOrDefault();
        }

        
        
        public static string GetUserEmailAddress(Guid userId)
        {
            // NOTE: we use the email address value as the username as well
            return (from u in db.Users where u.UserId.Equals(userId) select u.LoweredUserName).SingleOrDefault();
        }

        public static string GetResetUserPassword(string userName)
        {
            MembershipUser mu = Membership.GetUser(userName);
            if (mu != null)
            {
                // unlock the user first then reset password. 
                // otherwise you will get a runtime error.
                mu.UnlockUser();
                return mu.ResetPassword();
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
