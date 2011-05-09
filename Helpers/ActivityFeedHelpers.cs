using System;
using System.Web;
using BetterTaskList.Models;

namespace BetterTaskList.Helpers
{
    public class ActivityFeedHelpers
    {
        public void ShareNewTicketFeed(Ticket ticket)
        {
            ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
            ActivityFeed activityFeed = new ActivityFeed();

            activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(HttpContext.Current.User.Identity.Name);
            activityFeed.FeedActionDescription = "Created ticket #" + ticket.TicketId;

            int stringLenth = ticket.TicketDescription.Length;
            if (stringLenth > 180) { activityFeed.FeedActionDetails = ticket.TicketDescription.Substring(0, 179); }
            else { activityFeed.FeedActionDetails = ticket.TicketDescription.Substring(0, stringLenth); }

            activityFeed.FeedActionTimeStamp = DateTime.UtcNow;
            activityFeed.FeedMoreUrl = HttpContext.Current.Request.ApplicationPath + "/Tickets/Ticket/Details/" + ticket.TicketId;

            activityFeedRepository.Add(activityFeed);
            activityFeedRepository.Save();
        }

        public void ShareTicketResolvedFeed(Ticket ticket)
        {
            // record the activity
            ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
            ActivityFeed activityFeed = new ActivityFeed();
            activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(HttpContext.Current.User.Identity.Name);
            activityFeed.FeedActionDescription = "Resolved & closed ticket #" + ticket.TicketId;

            int stringLenght = ticket.TicketResolutionDetails.Length;
            if (stringLenght > 180) { activityFeed.FeedActionDetails = ticket.TicketResolutionDetails.Substring(0, 180); }
            else { activityFeed.FeedActionDetails = ticket.TicketResolutionDetails.Substring(0, stringLenght); }

            activityFeed.FeedActionTimeStamp = DateTime.UtcNow;
            activityFeed.FeedMoreUrl = HttpContext.Current.Request.ApplicationPath + "/Tickets/Ticket/Details/" + ticket.TicketId;

            activityFeedRepository.Add(activityFeed);
            activityFeedRepository.Save();

        }

        public void ShareTicketCommentFeed(int ticketId, string ticketCommentDetails)
        {

            ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
            ActivityFeed activityFeed = new ActivityFeed();
            activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(HttpContext.Current.User.Identity.Name);
            activityFeed.FeedActionDescription = "Commented on ticket #" + ticketId;

            int stringLenght = ticketCommentDetails.Length;
            if (stringLenght > 180) { activityFeed.FeedActionDetails = ticketCommentDetails.Substring(0, 180); }
            else { activityFeed.FeedActionDetails = ticketCommentDetails.Substring(0, stringLenght); }

            activityFeed.FeedActionTimeStamp = DateTime.UtcNow;
            activityFeed.FeedMoreUrl = HttpContext.Current.Request.ApplicationPath + "/Tickets/Ticket/Details/" + ticketId;

            activityFeedRepository.Add(activityFeed);
            activityFeedRepository.Save();


        }

        public void ShareTicketCommentReplyFeed(int ticketId, int ticketParentId,  string commentReplyDetails)
        {
            ActivityFeedRepository activityFeedRepository = new ActivityFeedRepository();
            ActivityFeed activityFeed = new ActivityFeed();
            activityFeed.FeedActionCreatorUserId = UserHelpers.GetUserId(HttpContext.Current.User.Identity.Name);
            activityFeed.FeedActionDescription = "Replied to commented on ticket #" + ticketId;

            int stringLenght = commentReplyDetails.Length;
            if (stringLenght > 180) { activityFeed.FeedActionDetails = commentReplyDetails.Substring(0, 180); }
            else { activityFeed.FeedActionDetails = commentReplyDetails.Substring(0, stringLenght); }

            activityFeed.FeedActionTimeStamp = DateTime.UtcNow;
            activityFeed.FeedMoreUrl = HttpContext.Current.Request.ApplicationPath + "/Tickets/Ticket/Details/" + ticketId;

            activityFeedRepository.Add(activityFeed);
            activityFeedRepository.Save();
        }

    }
}