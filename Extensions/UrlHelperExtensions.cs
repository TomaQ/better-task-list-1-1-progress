using System.Web.Mvc;

namespace BetterTaskList.Extensions
{
    public static class UrlHelpers
    {
        //TODO: Question the existance of this class (could do with Url.Content(""))
        public static string AccountPicture(this UrlHelper helper, string name, string size)
        {
            if (string.IsNullOrEmpty(name))
                name = "default";

            return helper.Content(string.Format("~/Content/Avatars/Pictures/{0}_{1}.png", name, size));
        }
    }


}