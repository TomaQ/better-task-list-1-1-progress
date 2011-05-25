using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public class CoWorkerRepository
    {
        BetterTaskListDataContext db = new BetterTaskListDataContext();

        public IEnumerable<Profile> GetMyCoWorkers(Guid userId)
        {
            //var uid = Helpers.UserHelpers.GetUserId(userName);

            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(userId) && r.AreFriends.Equals(true) select r.CoWorkerUserId);
            var friendProfiles = (from r in db.Profiles where listOfFriends.Contains(r.UserId) select r);

            return friendProfiles;
            //return (from r in db.Profiles select r);
        }

        public IEnumerable<Profile> GetMyCoWorkers(string userName)
        {
            var uid = Helpers.UserHelpers.GetUserId(userName);
            
            var listOfFriends = (from r in db.CoWorkers where r.UserId.Equals(uid) && r.AreFriends.Equals(true) select r.CoWorkerUserId);
            var friendProfiles = (from r in db.Profiles where listOfFriends.Contains(r.UserId) select r);
            
            return friendProfiles;
            //return (from r in db.Profiles select r);
        }

        public bool CoWorkersAreFriends(Guid userId, Guid coWorkerUserId)
        {
            int count = (from r in db.CoWorkers where r.UserId.Equals(userId) && r.CoWorkerUserId.Equals(coWorkerUserId) && r.AreFriends.Equals(true) select r).Count();
            return count == 0 ? false : true; //if count is not zero then they already friends
        }

        public bool CoWorkerRequestPending(Guid userId, Guid coWorkerUserId)
        {
            int count = (from r in db.CoWorkers where r.UserId.Equals(userId) && r.CoWorkerUserId.Equals(coWorkerUserId) && r.AreFriends.Equals(false) select r).Count();
            return count == 0 ? false : true; //if count is not zero then they already friends            
        }

        public CoWorker GetCoWorkerRequest(int id)
        {
            return (from r in db.CoWorkers where r.CoWorkerId.Equals(id) select r).SingleOrDefault();
        }

        public void Add(CoWorker coWorker)
        {
            db.CoWorkers.InsertOnSubmit(coWorker);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }
}