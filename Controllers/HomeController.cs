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

        [HttpGet,Authorize]
        public ActionResult Profile(Guid? id)
        {
            if (!id.HasValue)
            {
                // if no value is provided redirect to the currently authenticated user wall
                return RedirectToAction("Wall");
            }

            return View(new ProfileRepository().GetUserProfile(id.Value));

        }

        [HttpGet, Authorize]
        public ActionResult Wall()
        {
            return View(new ProfileRepository().GetUserProfile(User.Identity.Name));
        }

    }
}
