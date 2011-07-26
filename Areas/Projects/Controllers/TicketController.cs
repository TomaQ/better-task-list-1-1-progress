using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using BetterTaskList.Models;
using BetterTaskList.Helpers;
using System.Collections.Generic;
using BetterTaskList.Models.Tickets;
using Stream = BetterTaskList.Models.Stream;

namespace BetterTaskList.Areas.Projects.Controllers
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
            ticket.TicketCreatedDate = DateTime.UtcNow;
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

        [HttpPost, Authorize, ValidateInput(false)]
        public ActionResult EditDraft(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "NEW";

            try
            {
                UpdateModel(ticket);

                // No longer a draft so we update the createdDate 
                ticket.TicketCreatedDate = DateTime.UtcNow;
                ticket.TicketLastUpdated = DateTime.UtcNow;

                // which ever date the user selects we modify it to be in th PM rather then
                // the default AM that is normally assigned. by default if the user select 
                // 1/1/2010 and we submit that it defaults to 1/1/2010 12:00:00 AM so we 
                // modify this to be 1/1/2010 12:00:00 PM
                ticket.TicketDueDate = ticket.TicketDueDate.Value.AddHours(24);

                ticketRepository.Save();

                // send out the email notifications
                new EmailNotificationHelpers().NewTicketEmail(ticket);

                // publish the new ticket stream
                new StreamHelpers().ShareNewTicketStream(ticket);

                TempData["message"] = "That is all there is to it. Your ticket has been submited and those that need be have been notified via email.";
                return RedirectToAction("ByCreators", "Ticket");
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

        [HttpPost, Authorize, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            ticket.TicketStatus = "NEW";

            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();


                StreamRepository streamRepository = new StreamRepository();
                Stream stream = new Stream();

                stream.StreamType = "STATUS";
                stream.StreamDetails = ticket.TicketSubject;
                stream.StreamCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
                stream.StreamCreatorFullName = UserHelpers.GetUserFullName(User.Identity.Name);
                stream.StreamCreatedTimeStamp = DateTime.UtcNow;
                stream.StreamLastUpdatedTimeStamp = DateTime.UtcNow;

                streamRepository.Add(stream);
                streamRepository.Save();

                // ActivityFeed got replaced by Stream

                //// Share this activity with your peers (abstract this code to the activityFeedRepository if possible)
                //ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
                //ActivityFeed activityFeed = new ActivityFeed();

                //activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
                //activityFeed.FeedActionDescription = "Updated ticket #" + ticket.TicketId;

                //int stringLenth = ticket.TicketDescription.Length;
                //if (stringLenth > 180) { activityFeed.FeedActionDetails = ticketRepository.GetTicket(id).TicketDescription.Substring(0, 179); }
                //else { activityFeed.FeedActionDetails = ticketRepository.GetTicket(id).TicketDescription.Substring(0, stringLenth); }


                //activityFeed.FeedActionTimeStamp = DateTime.UtcNow;

                ////TODO: Update code below to dynamically determine the Url
                //activityFeed.FeedMoreUrl = "/BetterTaskList/Tickets/Ticket/Details/" + ticket.TicketId;

                //activityFeedRepository.Add(activityFeed);
                //activityFeedRepository.Save();

                TempData["message"] = "Ticket #" + ticket.TicketId + " changes have been successfully saved. We also went ahead and notified the proper parties involved.";
                return RedirectToAction("ByCreators", "Ticket");

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet, Authorize]
        public ActionResult Discard(int id, string returnUrl)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            if (ticket != null)
            {
                ticket.TicketStatus = "DISCARDED";
                ticketRepository.Save();
            }
            return Redirect(returnUrl);
        }

        [HttpGet, Authorize]
        public ActionResult Delete(int id)
        {
            // make sure the ticekt number is valid
            Ticket ticket = ticketRepository.GetTicket(id);

            if (ticket != null)
            {
                return View(ticket);
            }

            // if we go this far then it means the ticket id does not exist
            return RedirectToAction("NotFound", "Home", new { area = "" });
        }

        [HttpPost, Authorize]
        public ActionResult Delete(int id, string confirmationButton)
        {
            Ticket ticket = ticketRepository.GetTicket(id);

            // confirm ticket existance
            if (ticket == null) { return RedirectToAction("NotFound", "Home", new { area = "" }); }

            // delete ticket
            ticketRepository.Delete(ticket);
            ticketRepository.Save();

            TempData["message"] = "Ticket #" + ticket.TicketId + " has was deleted successfully. No notifications were sent out.";

            // provide delete confirmation and redirect back to ByCreators
            return RedirectToAction("ByCreators");
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

        public ActionResult ByCreators()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateInput(false)]
        public ActionResult ResolveAndClose(int id, FormCollection formCollection)
        {

            if (string.IsNullOrEmpty(formCollection["TicketResolutionDetails"]))
            {
                TempData["errorMessage"] = "Wooooaaahh! We need at least a sentence for the ticket resolution. You see without it very little data mining can be accomplished.";
                return RedirectToAction("Details", new { id = id });
            }

            // grab a reference to the ticket
            Ticket ticket = ticketRepository.GetTicket(id);

            // parse the start time if values have been provided
            if (!string.IsNullOrEmpty(formCollection["TicketStartDate"]) && !string.IsNullOrEmpty(formCollection["TicketStartTime"]))
            {
                string taskStartDate = formCollection["TicketStartDate"] + " " + formCollection["TicketStartTime"];
                ticket.TicketStartTimeStamp = DateTime.Parse(taskStartDate).ToUniversalTime();
            }

            // parse the finish time if values have been provided
            if (!string.IsNullOrEmpty(formCollection["TicketFinishDate"]) && !string.IsNullOrEmpty(formCollection["TicketFinishTime"]))
            {
                string taskFinishTimeStamp = formCollection["TicketFinishDate"] + " " + formCollection["TicketFinishTime"];
                ticket.TicketFinishTimeStamp = DateTime.Parse(taskFinishTimeStamp).ToUniversalTime();
            }

            // proceed only if we have values
            if (ticket.TicketStartTimeStamp != null && ticket.TicketFinishTimeStamp != null)
            {
                // make sure the start time is less then the finish time stamp
                if (ticket.TicketStartTimeStamp < ticket.TicketFinishTimeStamp)
                {
                    // calculate the time to ticket resolution and store as Ticks
                    TimeSpan ticksToClose = ticket.TicketFinishTimeStamp.Value.Subtract(ticket.TicketStartTimeStamp.Value);
                    ticket.TicketResolutionTime = ticksToClose.Ticks;
                }
            }

            // update other field values
            ticket.TicketResolvedByUserId = UserHelpers.GetUserId(User.Identity.Name);
            ticket.TicketResolutionDetails = formCollection["TicketResolutionDetails"];
            ticket.TicketStatus = "CLOSED";
            ticketRepository.Save();

            new ActivityFeedHelpers().ShareTicketResolvedFeed(ticket);

            // send out the email notification
            new EmailNotificationHelpers().TicketResolvedEmail(ticket);

            return RedirectToAction("ByCreators", "Ticket", new { area = "Projects" });

        }

        [HttpPost, Authorize, ValidateInput(false)]
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

        [HttpPost, Authorize, ValidateInput(false)]
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

        [HttpGet, Authorize]
        public ActionResult SearchTickets (string q)
        {
            var results = ticketRepository.SearchTicketsTittles(q);
            return View(results);
        }



    }
}
