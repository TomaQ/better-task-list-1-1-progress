using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterTaskList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          return  RedirectToAction("Queue", "Ticket", new {area = "Tickets"});
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}
