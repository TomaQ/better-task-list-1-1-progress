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
        [HttpGet, Authorize]
        public ActionResult Create()
        {
            Ticket ticket = new Ticket();
            ticket.TicketCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            ticket.TicketTags = "";
            ticket.TicketStatus = "DRAFT";
            ticket.TicketPriority = "Normal";
            ticket.TicketSubject = "";
            ticket.TicketDueDate = DateTime.UtcNow.AddDays(2);
            ticket.TicketDescription = "";
            ticket.TicketLastUpdated = DateTime.UtcNow;
            ticket.TicketStartTimeStamp = DateTime.UtcNow;
            ticket.TicketFinishTimeStamp = DateTime.UtcNow;
            ticket.TicketOwnersEmailList = "";
            ticket.TicketResolutionDetails = "";
            ticket.TicketEmailNotificationList = "";

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
            ticket.TicketStatus = "NEW";

            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();

                // send out the email notifications
                new EmailNotificationHelpers().NewTicketEmail(ticket);
                // publish the new ticket feed
                new ActivityFeedHelpers().ShareNewTicketFeed(ticket);

                TempData["message"] = "That is all there is to it. Your ticket has been submited and those that need be have been notified via email.";
                return RedirectToAction("Index", "Home", new { area = "" });
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

            if (ticket != null)
            {
                return View(ticket);
            }
            // if we got this far it means the ticket id does not exist
            return RedirectToAction("NotFound", "Home", new { area = "" });
        }

        [HttpPost, Authorize]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "NEW";

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
                return RedirectToAction("Index", "Home", new { area = "" });

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet, Authorize]
        public ActionResult Details(int id)
        {
            //TODO: 1 - Make sure the ticket ID being provided exist - 5/9/2011 
            //      2 - Make sure the end user has rights to view ticket

            Ticket ticket = ticketRepository.GetTicket(id);
            if (ticket != null)
            {
                return View(ticket);
            }
            // ticket id requested was not found
            return RedirectToAction("NotFound", "Home", new { area = "" });

        }

        public ActionResult Queue()
        {
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult ResolveAndClose(int id, FormCollection formCollection)
        {

            if (string.IsNullOrEmpty(formCollection["TicketResolutionDetails"]))
            {
                TempData["errorMessage"] = "Wooooaaahh! We need at least a sentence for the ticket resolution. You see without it very little data mining can be accomplished.";
                return RedirectToAction("Details", new { id = id });
            }

            // update the ticket
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketResolvedByUserId = UserHelpers.GetUserId(User.Identity.Name);
            ticket.TicketResolutionDetails = formCollection["TicketResolutionDetails"];
            ticket.TicketStatus = "CLOSED";
            ticketRepository.Save();

            new ActivityFeedHelpers().ShareTicketResolvedFeed(ticket);
            return RedirectToAction("Index", "Home", new { area = "" });

        }

        [HttpPost, Authorize]
        public ActionResult PostComment(int id, FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["TicketCommentDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Details", new { id = id });
            }

            TicketCommentRepository ticketCommentRepository = new TicketCommentRepository();

            TicketComment ticketComment = new TicketComment();
            ticketComment.TicketId = id;
            ticketComment.TicketCommentTimeStamp = DateTime.UtcNow;
            ticketComment.TicketCommentDetails = formCollection["TicketCommentDetails"];
            ticketComment.TicketCommentSubmitterUserId = UserHelpers.GetUserId(User.Identity.Name);

            ticketCommentRepository.Add(ticketComment);
            ticketCommentRepository.Save();

            // send out email notifications
            Ticket ticket = ticketRepository.GetTicket(id);
            new EmailNotificationHelpers().TicketCommentEmail(ticket, ticketComment);

            // post to feed notifications
            new ActivityFeedHelpers().ShareTicketCommentFeed(id, ticketComment.TicketCommentDetails);


            TempData["message"] = "Your valuable input was successfully posted to ticket #" + id + ". We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost, Authorize]
        public ActionResult PostCommentReply(int id, int ticketCommentId, FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["CommentReplyDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Details", new { id = id });
            }

            TicketCommentRepository ticketCommentRepository = new TicketCommentRepository();

            TicketComment ticketCommentReply = new TicketComment();
            ticketCommentReply.TicketId = id;
            ticketCommentReply.TicketCommentParentId = ticketCommentId;
            ticketCommentReply.TicketCommentTimeStamp = DateTime.UtcNow;
            ticketCommentReply.TicketCommentDetails = formCollection["CommentReplyDetails"];
            ticketCommentReply.TicketCommentSubmitterUserId = UserHelpers.GetUserId(User.Identity.Name);

            ticketCommentRepository.Add(ticketCommentReply);
            ticketCommentRepository.Save();

            // send out email notifications
            Ticket ticket = ticketRepository.GetTicket(id);
            new EmailNotificationHelpers().TicketCommentEmail(ticket, ticketCommentReply);

            // post the activity
            new ActivityFeedHelpers().ShareTicketCommentReplyFeed(id, ticketCommentId, ticketCommentReply.TicketCommentDetails);

            TempData["message"] = "Your valuable input was successfully posted to ticket #" + id + ". We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Details", new { id = id });
        }


        [HttpGet]
        public ActionResult MembersEmailList(string q)
        {
            string[] emailList = new TicketRepository().GetProjectMembersEmailList(q);
            return Content(string.Join("\n", emailList));
        }

    }
}
