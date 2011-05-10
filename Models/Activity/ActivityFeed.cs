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

        public string FeedCreator128x128AvatarUrl
        {
            get
            {
                string pictureName = (from r in db.Profiles where r.UserId.Equals(FeedActionCreatorUserId) select r.PictureName).Single();
                if (string.IsNullOrEmpty(pictureName))
                {
                    return string.Format("~/Content/Avatars/{0}_128x128.png", "Default");
                }

                return string.Format("~/Content/Avatars/Pictures/{0}_128x128.png", pictureName);
            }
        }

        public string FeedCreator64x64AvatarUrl
        {
            get
            {
                string pictureName = (from r in db.Profiles where r.UserId.Equals(FeedActionCreatorUserId) select r.PictureName).Single();
                if (string.IsNullOrEmpty(pictureName))
                {
                    return string.Format("~/Content/Avatars/{0}_64x64.png", "Default");
                }

                return string.Format("~/Content/Avatars/Pictures/{0}_64x64.png", pictureName);
            }
        }

        public string FeedCreator32x32AvatarUrl
        {
            get
            {
                string pictureName = (from r in db.Profiles where r.UserId.Equals(FeedActionCreatorUserId) select r.PictureName).Single();
                if (string.IsNullOrEmpty(pictureName))
                {
                    return string.Format("~/Content/Avatars/{0}_32x32.png", "Default");
                }

                return string.Format("~/Content/Avatars/Pictures/{0}_32x32.png", pictureName);
            }
        }

        public string FeedCreator16x16AvatarUrl
        {
            get
            {
                string pictureName = (from r in db.Profiles where r.UserId.Equals(FeedActionCreatorUserId) select r.PictureName).Single();
                if (string.IsNullOrEmpty(pictureName))
                {
                    return string.Format("~/Content/Avatars/{0}_16x16.png", "Default");
                }

                return string.Format("~/Content/Avatars/Pictures/{0}_16x16.png", pictureName);
            }
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

                if (days == 0 && hours < 1)
                {
                    if (min < 1)
                    {
                        return sec + " seconds ago";
                    }

                    return min + " minutes ago";
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