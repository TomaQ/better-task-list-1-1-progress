using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public class ActivityFeedRepository
    {
        BetterTaskListDataContext db = new BetterTaskListDataContext();


        public IEnumerable<ActivityFeed> Feed()
        {
            return (from r in db.ActivityFeeds orderby r.FeedActionTimeStamp descending select r).AsEnumerable();
        }

        public void Add(ActivityFeed activityFeed)
        {
            db.ActivityFeeds.InsertOnSubmit(activityFeed);
        }

        public void Save ()
        {
            db.SubmitChanges();
        }


    }
}