using System;
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

        /// <summary>
        /// Returns a profile object given the UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Profile GetUserProfile(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u).SingleOrDefault();
        }

        /// <summary>
        /// Returns a profile object given the ProfileId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Profile GetUserProfile(long id)
        {
            return (from u in db.Profiles where u.ProfileId.Equals(id) select u).SingleOrDefault();
        }

        public static string GetUserEmailAddress(Guid userId)
        {
            // NOTE: we use the email address value as the username as well
            return (from u in db.Users where u.UserId.Equals(userId) select u.LoweredUserName).SingleOrDefault();
        }

        public static string GetUserFullName(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FullName).SingleOrDefault();
        }

        public static string GetUserFullName(string userName)
        {
            Guid userId = GetUserId(userName);
            return (from u in db.Profiles where u.UserId.Equals(userId) select u.FullName).SingleOrDefault();
        }

        public static string GetResetUserPassword(string userName)
        {
            MembershipUser mu = Membership.GetUser(userName);
            if(mu !=null)
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
