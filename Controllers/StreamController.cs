﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetterTaskList.Models;
using BetterTaskList.Helpers;
using System.Collections.Generic;

namespace BetterTaskList.Controllers
{
    public class StreamController : Controller
    {


        [HttpPost, Authorize]
        public ActionResult PostStream(FormCollection formCollection)
        {

            if (string.IsNullOrEmpty(formCollection["StreamDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Wall", "Home");
            }

            StreamRepository streamRepository = new StreamRepository();
            Stream stream = new Stream();

            stream.StreamType = "STATUS";
            stream.StreamDetails = formCollection["StreamDetails"];
            stream.StreamCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            stream.StreamCreatorFullName = UserHelpers.GetUserFullName(User.Identity.Name);
            stream.StreamLastUpdatedTimeStamp = DateTime.UtcNow;
            stream.StreamCreatedTimeStamp = DateTime.UtcNow;

            streamRepository.Add(stream);
            streamRepository.Save();


            TempData["message"] = "Your input was posted to " + stream.StreamCreatorFullName + " wall. We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Wall", "Home");

            //if (string.IsNullOrEmpty(formCollection["TicketCommentDetails"]))
            //{
            //    TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
            //    return RedirectToAction("Details", new { id = id });
            //}

            //TicketCommentRepository ticketCommentRepository = new TicketCommentRepository();

            //TicketComment ticketComment = new TicketComment();
            //ticketComment.TicketId = id;
            //ticketComment.TicketCommentTimeStamp = DateTime.UtcNow;
            //ticketComment.TicketCommentDetails = formCollection["TicketCommentDetails"];
            //ticketComment.TicketCommentSubmitterUserId = UserHelpers.GetUserId(User.Identity.Name);

            //ticketCommentRepository.Add(ticketComment);
            //ticketCommentRepository.Save();

            //// send out email notifications
            //Ticket ticket = ticketRepository.GetTicket(id);
            //new EmailNotificationHelpers().TicketCommentEmail(ticket, ticketComment);

            //// post to feed notifications
            //new ActivityFeedHelpers().ShareTicketCommentFeed(id, ticketComment.TicketCommentDetails);


            //TempData["message"] = "Your valuable input was successfully posted to ticket #" + id + ". We even went as far as notifiying the appropiate parties!.";
            //return RedirectToAction("Details", new { id = id });
            return View();
        }

        [HttpPost, Authorize]
        public ActionResult PostStreamComment(int replyToStreamId, FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["StreamCommentDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Wall", "Home");
            }

            StreamCommentRepository streamCommentRepository = new StreamCommentRepository();
            StreamComment streamComment = new StreamComment();

            streamComment.StreamId = replyToStreamId;
            streamComment.StreamCommentDetails = formCollection["StreamCommentDetails"];
            streamComment.StreamCommentSubmitterUserId = UserHelpers.GetUserId(User.Identity.Name);
            streamComment.StreamCommentSubmitterFullName = UserHelpers.GetUserFullName(User.Identity.Name);
            streamComment.StreamCommentTimeStamp = DateTime.UtcNow;
            streamCommentRepository.Add(streamComment);
            streamCommentRepository.Save();

            TempData["message"] = "Your input was posted to " + streamComment.StreamCommentSubmitterFullName + " wall. We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Wall", "Home");
        }

    }
}