using System;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;

namespace BetterTaskList.Models
{
    public class MembershipRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();

        public IEnumerable<RegisteredUserModel> GetUsers()
        {
            var registeredUsers = (from profile in db.Profiles
                                   join User in db.Users on profile.UserId equals User.UserId
                                   join userMembership in db.Memberships on User.UserId equals userMembership.UserId
                                   orderby profile.FullName
                                   select new RegisteredUserModel
                                   {
                                       UserId = User.UserId,
                                       FirstName = profile.FirstName,
                                       LastName = profile.LastName,
                                       FullName = profile.FullName,
                                       Email = User.LoweredUserName,
                                       IsLockedOut = userMembership.IsLockedOut,
                                       LastLoginDate = userMembership.LastLoginDate
                                   });
            return registeredUsers;
        }

        //public MembershipUser GetUser(Guid userId)
        //{
        //    var result = (from r in db.Memberships where r.UserId.Equals(userId) select r).Single();
        //    return result;
        //}

    }
}