using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class TicketComment
    {
        BetterTaskListDataContext db = new BetterTaskListDataContext();

        public string CommentSubmitterFullName
        {
            get { return (from r in db.Profiles where r.UserId.Equals(TicketCommentSubmitterUserId) select r.FirstName + " " + r.LastName).Single(); }
        }

        public string CommentPostedTimeFrame
        {

            get
            {
                TimeSpan timeDifference = DateTime.Now.Subtract(TicketCommentTimeStamp);

                int days = timeDifference.Days;
                int hours = timeDifference.Hours;

                //// if you dont take in to account the amount of hours you may end up 
                //// with simething like Today @ 4:30PM for something that was posted
                //// yesterday afternoon asuming a total of 24hrs have not passed.
                if (days == 0 && hours < 12)
                {
                    return "Today @ " + TicketCommentTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if( days == 0 && hours > 12)
                {
                    return "Yesterday @ " + TicketCommentTimeStamp.ToShortTimeString();  
                }
                else if (days == -1)
                {
                    return "Yesterday @ " + TicketCommentTimeStamp.ToShortTimeString();
                }
                else
                {
                    //return TaskCommentTimeStamp.ToString("dddd MMMM dd h:mm tt");
                    return TicketCommentTimeStamp.ToLocalTime().ToString("MMMM dd h:mm tt");

                }
            }

        }
    }

}

