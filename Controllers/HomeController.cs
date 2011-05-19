using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BetterTaskList.Models;
using BetterTaskList.Helpers;
using System.Collections.Generic;


namespace BetterTaskList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }


        public ActionResult Profile(Guid? id)
        {
            if (!id.HasValue)
            {
                return View(new ProfileRepository().GetUserProfile(User.Identity.Name));
            }

            return View(new ProfileRepository().GetUserProfile(id.Value));

        }

    }
}
