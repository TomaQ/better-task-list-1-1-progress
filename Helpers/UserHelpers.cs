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

        public static string GetFirstName(string userName)
        {
            Guid userId = GetUserId(userName);
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FirstName).SingleOrDefault();
        }

        public static string GetFirstName(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FirstName).SingleOrDefault();
        }

        public static string GetUserEmailAddress(Guid userId)
        {
            // NOTE: we use the email address value as the username as well
            return (from u in db.Users where u.UserId.Equals(userId) select u.LoweredUserName).SingleOrDefault();
        }


        //****************************************************
        // GetEmailAddreses
        //****************************************************
        public static string GetEmailAddress(Guid userId)
        {
            return (from r in db.Users where r.UserId.Equals(userId) select r.LoweredUserName).SingleOrDefault();
        }

        public static string[] GetEmailAddresses()
        {
            return (from r in db.Users select r.LoweredUserName).ToArray();

        }

        public static string[] GetUserFriendsEmailAdresses(string userName)
        {
            // pull the userId
            var uid = (from r in db.Users where r.LoweredUserName.Equals(userName) select r.UserId).SingleOrDefault();

            // Get a list of the user friends
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);
            
            // pull the friends email addresses
            var friendEmailAddresses = (from r in db.Users where listOfFriends.Contains(r.UserId) select r.LoweredUserName).ToArray();

            return friendEmailAddresses;
        }

        public static IEnumerable<Profile> GetProfiles(string currentUserUserName)
        {
            // grab the currently logged in user userId
            var uid = (from r in db.Users where r.LoweredUserName.Equals(currentUserUserName) select r.UserId).SingleOrDefault();
 
            // grab the list of users this person is friends with
            var friendsList = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);

            // compile the list of users this person is not friends with and exclude himself (no one wants to friend themselves)
            var notFriendsWith = (from r in db.Profiles where !friendsList.Contains(r.UserId) &&  !r.UserId.Equals(uid) select r);

            return notFriendsWith;
        }

        public static string GetResetUserPassword(string userName)
        {
            MembershipUser mu = Membership.GetUser(userName);
            if (mu != null)
            {
                // unlock the user first then reset password. r.
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
