using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class ActivityFeed
    {
        BetterTaskListDataContext db = new BetterTaskListDataContext();

        public string FeedCreatorFullName
        {
            get { return (from r in db.Profiles where r.UserId.Equals(FeedActionCreatorUserId) select r.FirstName + " " + r.LastName).Single(); }
        }

    }
}