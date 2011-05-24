using System;
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
        public ActionResult PostStatusStream(FormCollection formCollection)
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

            // inform the poster friends of what has been posted
            new EmailNotificationHelpers().StatusPost(User.Identity.Name, stream.StreamId, stream.StreamDetails);

            return RedirectToAction("Wall", "Home");

        }

        [HttpPost, Authorize]
        public ActionResult PostStatusStreamComment(int replyToStreamId, FormCollection formCollection)
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

          //  TempData["message"] = "Your input was posted to " + streamComment.StreamCommentSubmitterFullName + " wall. We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Wall", "Home");
        }

        [HttpPost, Authorize]
        public ActionResult PostToCoWorkerStream(Guid id, FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["StreamDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Profile", "Home", new {id = id});
            }

            StreamRepository streamRepository = new StreamRepository();
            Stream stream = new Stream();

            stream.StreamType = "WALLPOST";
            stream.StreamDetails = formCollection["StreamDetails"];
            stream.StreamCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            stream.StreamCreatorFullName = UserHelpers.GetUserFullName(User.Identity.Name);
            stream.StreamLastUpdatedTimeStamp = DateTime.UtcNow;
            stream.StreamCreatedTimeStamp = DateTime.UtcNow;

            streamRepository.Add(stream);
            streamRepository.Save();

            new EmailNotificationHelpers().CoWorkerWallPostEmail(User.Identity.Name, id, stream.StreamId, stream.StreamDetails);

          //  TempData["message"] = "Your input was posted to " + stream.StreamCreatorFullName + " wall. We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Profile", "Home", new { id = id});

        }

        [HttpPost, Authorize]
        public ActionResult PostToCoWorkerStreamComment(Guid id, int replyToStreamId, FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["StreamCommentDetails"]))
            {
                TempData["errorMessage"] = "Yikes! Seems like you forgot to provide us with your valuable thoughts in the comments field. How about you try again?";
                return RedirectToAction("Wall", "Home", new {id = id});
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

          //  TempData["message"] = "Your input was posted to " + streamComment.StreamCommentSubmitterFullName + " wall. We even went as far as notifiying the appropiate parties!.";
            return RedirectToAction("Profile", "Home", new {id = id});
        }



    }
}
