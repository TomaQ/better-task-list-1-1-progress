using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BetterTaskList.Models;
using BetterTaskList.Helpers;
using System.Collections.Generic;
using BetterTaskList.Models.Tickets;

namespace BetterTaskList.Areas.Tickets.Controllers
{
    public class TicketController : Controller
    {
        TicketRepository ticketRepository = new TicketRepository();

        //
        // GET: /Tickets/Ticket/
        [Authorize, HttpGet]
        public ActionResult Create()
        {
            Ticket ticket = new Ticket();
            ticket.TicketCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            ticket.TicketTags = "";
            ticket.TicketStatus = "Draft";
            ticket.TicketPriority = "Normal";
            ticket.TicketSubject = "";
            ticket.TicketDueDate = DateTime.Now.AddDays(2);
            ticket.TicketDescription = "";
            ticket.TicketLastUpdated = DateTime.Now;
            ticket.TicketStartTimeStamp = DateTime.Now;
            ticket.TicketFinishTimeStamp = DateTime.Now;
            ticket.TicketOwnersEmailList = "";
            ticket.TicketResolutionDetails = "";
            ticket.TicketEmailNotificationList = "";

            TicketRepository ticketRepository = new TicketRepository();
            ticketRepository.Add(ticket);
            ticketRepository.Save();

            // redirect to edit mode
            return RedirectToAction("EditDraft", new { id = ticket.TicketId });

        }

        [HttpGet, Authorize]
        public ActionResult EditDraft(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            return View(ticket);
        }

        [HttpPost, Authorize]
        public ActionResult EditDraft(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "New";

            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();

                // Share this activity with your peers (abstract this code to the activityFeedRepository if possible)
                ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
                ActivityFeed activityFeed = new ActivityFeed();

                activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
                activityFeed.FeedActionDescription = "Created ticket #" + ticket.TicketId;

                int stringLenth = ticket.TicketDescription.Length;
                if (stringLenth > 180) { activityFeed.FeedActionDetails = ticket.TicketDescription.Substring(0, 179); }
                else { activityFeed.FeedActionDetails = ticket.TicketDescription.Substring(0, stringLenth); }

                activityFeed.FeedActionTimeStamp = DateTime.UtcNow;

                //TODO: Update code below to dynamically determine the Url
                activityFeed.FeedMoreUrl = "/BetterTaskList/Tickets/Ticket/Details/" + ticket.TicketId;

                activityFeedRepository.Add(activityFeed);
                activityFeedRepository.Save();

                TempData["message"] = "That is all there is to it. Your ticket has been submited and those that need be have been notified via email.";
                return RedirectToAction("Queue");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet, Authorize]
        public ActionResult Edit(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            return View(ticket);
        }

        [HttpPost, Authorize]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "New";

            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();

                // Share this activity with your peers (abstract this code to the activityFeedRepository if possible)
                ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
                ActivityFeed activityFeed = new ActivityFeed();

                activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
                activityFeed.FeedActionDescription = "Updated ticket #" + ticket.TicketId;

                int stringLenth = ticket.TicketDescription.Length;
                if (stringLenth > 180) { activityFeed.FeedActionDetails = ticketRepository.GetTicket(id).TicketDescription.Substring(0, 179); }
                else { activityFeed.FeedActionDetails = ticketRepository.GetTicket(id).TicketDescription.Substring(0, stringLenth); }


                activityFeed.FeedActionTimeStamp = DateTime.UtcNow;

                //TODO: Update code below to dynamically determine the Url
                activityFeed.FeedMoreUrl = "/BetterTaskList/Tickets/Ticket/Details/" + ticket.TicketId;

                activityFeedRepository.Add(activityFeed);
                activityFeedRepository.Save();

                TempData["message"] = "Ticket #" + ticket.TicketId + " changes have been successfully saved. We also went ahead and notified the proper parties involved.";
                return RedirectToAction("Queue");

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            //TODO: 1 - Make sure the ticket ID being provided exist
            //      2 - Make sure the end user has rights to view ticket

            return View(ticketRepository.GetTicket(id));
        }

        public ActionResult Queue()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostComment(int id, FormCollection formCollection)
        {
            //if (string.IsNullOrEmpty(formCollection["TicketComment"]))
            //{
            //    return RedirectToAction("Edit", new { Id = 1 });
            //}

            //return RedirectToAction("Edit", id = id);

            TempData["message"] = "Comment was successfully posted to ticket #" + id;
            return View();
        }

        [HttpGet]
        public ActionResult MembersEmailList(string q)
        {
            string[] emailList = new TicketRepository().GetProjectMembersEmailList(q);
            return Content(string.Join("\n", emailList));
        }

    }
}
