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

                int seconds = timeDifference.Seconds;
                int min = timeDifference.Minutes;
                int days = timeDifference.Days;
                int hours = timeDifference.Hours;

                //// if you dont take in to account the amount of hours you may end up 
                //// with simething like Today @ 4:30PM for something that was posted
                //// yesterday afternoon asuming a total of 24hrs have not passed.
                
                if(days == 0 && hours < 1)
                {
                    if(min < 1)
                    {
                        return seconds + " seconds ago";
                    }

                    return  min + " minutes ago";
                }
                
                if (days == 0 && hours < 12)
                {
                    return "Today @ " + FeedActionTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if (days == 0 && hours > 12)
                {
                    return "Yesterday @ " + FeedActionTimeStamp.ToShortTimeString();
                }
                else if (days == -1)
                {
                    return "Yesterday @ " + FeedActionTimeStamp.ToShortTimeString();
                }
                else
                {
                    //return TaskCommentTimeStamp.ToString("dddd MMMM dd h:mm tt");
                    return FeedActionTimeStamp.ToLocalTime().ToString("MMMM dd h:mm tt");

                }
            }

        }


    }
}