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

            // Share this activity with your peers (abstract this code to the activityFeedRepository if possible)
            ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
            ActivityFeed activityFeed = new ActivityFeed();

            activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            activityFeed.FeedActionDescription = "create a ticket";
            activityFeed.FeedActionDetails = " "; //ticket.TicketDescription.Substring(0, 180);
            activityFeed.FeedActionTimeStamp = DateTime.UtcNow;

            //TODO: Update code below to dynamically determine the Url
            activityFeed.FeedMoreUrl = "/localhost/BetterTaskList/Tickets/Ticket/Details/" + ticket.TicketId;

            activityFeedRepository.Add(activityFeed);
            activityFeedRepository.Save();

            // redirect to edit mode
            return RedirectToAction("Edit", new { id = ticket.TicketId });

        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            return View(ticket);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "New";
            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();

                TempData["message"] = "That is all there is to it. Your ticket has been submited and those that need be have been notified via email.";
                return RedirectToAction("Queue");
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Queue()
        {
            return View();
        }

        //
        // GET: /Task/Create
        [HttpGet]
        public ActionResult MembersEmailList(string q)
        {
            string[] emailList = new TicketRepository().GetProjectMembersEmailList(q);
            return Content(string.Join("\n", emailList));
        }

    }
}
