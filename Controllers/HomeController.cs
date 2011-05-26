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

        [HttpGet, Authorize]
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

        [HttpGet, Authorize]
        public ActionResult WallPost()
        {
            return View(new ProfileRepository().GetUserProfile(User.Identity.Name));
        }

        [HttpGet, Authorize]
        public ActionResult CoWorkers()
        {
            return View();
        }

        [HttpGet, Authorize]
        public ActionResult AddCoWorker(Guid id)
        {
            // make sure the user is not attempting to add himself as a friend

            if (id == UserHelpers.GetUserId(User.Identity.Name))
            {
                TempData["errorMessage"] = "It is not possible to add yourself as a coworker. Sorry, system limitation better yet system violation";
                return RedirectToAction("Coworkers");
            }

            Models.Profile profile = UserHelpers.GetUserProfile(id);
            return View(profile);
        }

        [HttpPost, Authorize]
        public ActionResult AddCoWorker(Guid? id, string confirm)
        {
            if (id.HasValue)
            {

                CoWorkerRepository coWorkerRepository = new CoWorkerRepository();
                Guid requesterUserId = UserHelpers.GetUserId(User.Identity.Name);
                string requesterFullName = UserHelpers.GetUserFullName(User.Identity.Name);
                string coWorkerEmailAddress = UserHelpers.GetUserEmailAddress(id.Value);
                string coWorkderFullName = UserHelpers.GetUserFullName(id.Value);

                //
                // first make sure no such friendship already exist)
                if (coWorkerRepository.CoWorkersAreFriends(requesterUserId, id.Value))
                {
                    TempData["errorMessage"] = "Records indicate you and " + coWorkderFullName + " have an existing friendship. There is no need to send a coworker request.";
                    return RedirectToAction("CoWorkers");
                }

                //
                // Now make sure no such previous request exist
                if (coWorkerRepository.CoWorkerRequestPending(requesterUserId, id.Value))
                {
                    TempData["errorMessage"] = "There is already a coworker request for " + coWorkderFullName + " pending approval. Upon approval by " + coWorkderFullName + " we will notify you.";
                    return RedirectToAction("CoWorkers");
                }

                //
                // Proceed to initiate a coworker request
                CoWorker coWorker = new CoWorker() { AreFriends = false, CoWorkerUserId = id.Value, UserId = requesterUserId };
                coWorkerRepository.Add(coWorker);
                coWorkerRepository.Save();


                new EmailNotificationHelpers().AddCoWorkerEmail(coWorker.CoWorkerId, requesterFullName, coWorkerEmailAddress);
                TempData["message"] = "We have sent " + coWorkderFullName + " a coworker request notification; We will notify you upon request approval.";
                return RedirectToAction("CoWorkers");

            }

            TempData["errorMessage"] = "There was an error processing your request";
            return View();
        }

        [HttpGet, Authorize]
        public ActionResult ConfirmCoWorker(int id)
        {
            CoWorkerRepository coWorkerRepository = new CoWorkerRepository();
            CoWorker firstFriendShip = coWorkerRepository.GetCoWorkerRequest(id);

            // make sure the id is actually for the proper CoWorkerUserId (logged in user) dont want people working the system
            if (firstFriendShip.CoWorkerUserId != UserHelpers.GetUserId(User.Identity.Name))
            {
                TempData["errorMessage"] = "It appears that you are not the proper recepient for the provided coworkder request";
                return RedirectToAction("Coworkers");
            }

            firstFriendShip.AreFriends = true;
            coWorkerRepository.Save();

            // we now create a second friendship so that we have a two way friendship
            CoWorker secondFriendship = new CoWorker();
            secondFriendship.UserId  = firstFriendShip.CoWorkerUserId;
            secondFriendship.CoWorkerUserId = firstFriendShip.UserId;
            secondFriendship.AreFriends = true;

            coWorkerRepository.Add(secondFriendship);
            coWorkerRepository.Save();


            var coWorkerFullName = UserHelpers.GetUserFullName(firstFriendShip.CoWorkerUserId);

            TempData["message"] = coWorkerFullName + " has been added to your coworker list. Thank you for being social.";
            return RedirectToAction("Wall");

        }

    }
}
