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

        public string FeedPostedTimeFrame
        {

            get
            {
                TimeSpan timeDifference = DateTime.UtcNow.Subtract(FeedActionTimeStamp);

                int sec = timeDifference.Seconds;
                int min = timeDifference.Minutes;
                int hours = timeDifference.Hours;
                int days = timeDifference.Days;

                //// if you dont take in to account the amount of hours you may end up 
                //// with something like Today @ 4:30PM for something that was posted
                //// yesterday afternoon asuming a total of 24hrs have not passed.
                
                if(days == 0 && hours < 1)
                {
                    if(min < 1)
                    {
                        return sec + " seconds ago";
                    }

                    return  min + " minutes ago";
                }
                
                if (days == 0 && hours < 12)
                {
                    return "Today @ " + FeedActionTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if (days == 0 && hours > 12)
                {
                    return "Yesterday @ " + FeedActionTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if (days == -1)
                {
                    return "Yesterday @ " + FeedActionTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else
                {
                    return FeedActionTimeStamp.ToLocalTime().ToString("MMMM dd h:mm tt");
                }
            }

        }


    }
}