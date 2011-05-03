using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public class ProfileRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();


        public Profile GetUserProfile(string userName)
        {
            Guid userId = (from u in db.Users where u.LoweredUserName.Equals(userName) select u.UserId).SingleOrDefault();
            return (from r in db.Profiles where r.UserId.Equals(userId) select r).SingleOrDefault();
        }

        public Profile GetUserProfile(Guid userId)
        {
            return (from u in db.Profiles where u.UserId.Equals(userId) select u).SingleOrDefault();
        }

        public Profile GetUserProfile(long id)
        {
            return (from u in db.Profiles where u.ProfileId.Equals(id) select u).SingleOrDefault();
        }


        public void Add(Profile profile)
        {
            db.Profiles.InsertOnSubmit(profile);
        }

        public void Delete(Profile profile)
        {
            db.Profiles.DeleteOnSubmit(profile);
        }

        public void Save ()
        {
            db.SubmitChanges();
        }

    }
}