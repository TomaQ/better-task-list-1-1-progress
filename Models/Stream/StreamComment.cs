using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class StreamComment
    {

        public string StreamCommentCreatedTimeSpan
        {
            get
            {
                TimeSpan timeDifference = DateTime.UtcNow.Subtract(StreamCommentTimeStamp);

                int sec = timeDifference.Seconds;
                int min = timeDifference.Minutes;
                int hours = timeDifference.Hours;
                int days = timeDifference.Days;

                //// if you dont take in to account the amount of hours you may end up 
                //// with something like Today @ 4:30PM for something that was posted
                //// yesterday afternoon asuming a total of 24hrs have not passed.

                if (days == 0 && hours < 1)
                {
                    if (min < 1)
                    {
                        return sec + " seconds ago";
                    }

                    return min + " minutes ago";
                }


                //TODO: There is an issue here say someone post something in the houurs of Yesterday at 10:45PM and someone is reading 
                // the tread at 10:10AM the following the the returned value will be Today@ 10:45PM which would be incorrect because it 
                // was yesterdays that it got posted!
                
                if (days == 0 && hours < 12)
                {
                    return "Today @ " + StreamCommentTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if (days == 0 && hours > 12)
                {
                    return "Yesterday @ " + StreamCommentTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else if (days == -1)
                {
                    return "Yesterday @ " + StreamCommentTimeStamp.ToLocalTime().ToShortTimeString();
                }
                else
                {
                    return StreamCommentTimeStamp.ToLocalTime().ToString("MMMM dd h:mm tt");
                }
            }
        }
    }
}