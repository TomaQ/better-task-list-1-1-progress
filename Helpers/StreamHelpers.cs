using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterTaskList.Models;

namespace BetterTaskList.Helpers
{
    public class StreamHelpers
    {
        public void ShareNewTicketStream(Ticket ticket)
        {
            StreamRepository streamRepository = new StreamRepository();
            Stream stream = new Stream();

            stream.StreamType = "STATUS";
            stream.StreamDetails = "Submitted Ticket #" + ticket.TicketId + ": " + ticket.TicketSubject;
            stream.StreamCreatorUserId = UserHelpers.GetUserId(HttpContext.Current.User.Identity.Name);
            stream.StreamCreatorFullName = UserHelpers.GetUserFullName(HttpContext.Current.User.Identity.Name);
            stream.StreamCreatedTimeStamp = DateTime.UtcNow;
            stream.StreamLastUpdatedTimeStamp = DateTime.UtcNow;

            streamRepository.Add(stream);
            streamRepository.Save();
        }

    }
}