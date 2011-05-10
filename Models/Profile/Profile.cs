using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class Profile
    {
        public string Picture128x128Url
        {
            get
            {
                if (string.IsNullOrEmpty(PictureName))
                {
                    return string.Format("~/Content/Avatars/{0}_128x128.png", "Default");
                }

                return string.Format("~/Content/Avatars/Pictures/{0}_128x128.png", PictureName);
            }
        }


    }
}