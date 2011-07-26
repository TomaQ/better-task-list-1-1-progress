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
            var registeredUsers = (from p in db.Profiles
                                   join u in db.Users on p.UserId equals u.UserId
                                   join um in db.Memberships on u.UserId equals um.UserId
                                   orderby p.FullName
                                   select new RegisteredUserModel
                                   {
                                       UserId = u.UserId,
                                       FirstName = p.FirstName,
                                       LastName = p.LastName,
                                       FullName = p.FullName,
                                       Email = u.LoweredUserName,
                                       IsLockedOut = um.IsLockedOut,
                                       IsApproved = um.IsApproved,
                                       LastLoginDate = um.LastLoginDate
                                   });
            return registeredUsers;
        }

    }
}