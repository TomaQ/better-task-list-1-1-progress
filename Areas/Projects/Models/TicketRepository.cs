using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models.Tickets
{
    public class TicketRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();

        public Ticket GetTicket(int ticketId)
        {
            return db.Tickets.SingleOrDefault(t => t.TicketId == ticketId);
        }

        public long[] GetUserDraftTickets(string userName)
        {
            Guid userId =(from u in db.Users where u.LoweredUserName.Equals(userName) select u.UserId).SingleOrDefault();
            long[] results = (from r in db.Tickets where r.TicketStatus.Equals("DRAFT") && r.TicketCreatorUserId.Equals(userId) select r.TicketId).ToArray();
            return results;
        }

        public Guid[] GetTicketCreators()
        {
            var results = (from r in db.Tickets where r.TicketStatus.Equals("NEW") select r.TicketCreatorUserId).Distinct().ToArray();
            return results;
        }

        public IQueryable<Ticket> GetNewTickets(Guid ticketCreatorUserId)
        {
            return (from r in db.Tickets where r.TicketStatus.Equals("NEW") && r.TicketCreatorUserId.Equals(ticketCreatorUserId) select r).AsQueryable();
        }

        public string[] GetProjectMembersEmailList(string searchString)
        {
            //var emailList = new string[] {"geovanimartinez@yovasolutions.com", "juanmartinez@yovasolutions.com", "oscarmartinez@yovasolutions.com" };
            //return emailList;
            return (from u in db.Users where u.LoweredUserName.Contains(searchString) select u.LoweredUserName).ToArray();
        }

        public void Add(Ticket ticket)
        {
            db.Tickets.InsertOnSubmit(ticket);
        }

        public void Delete(Ticket ticket)
        {
            db.Tickets.DeleteOnSubmit(ticket);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }

    public class TicketCommentRepository
    {
        private BetterTaskListDataContext db = new BetterTaskListDataContext();

        public IEnumerable<TicketComment> GetTicketComments(long id)
        {
            // We only want to return comments that do not have a parent tied to them. Comments with a parent value greater then zero are comment replys
            return (from r in db.TicketComments where r.TicketId.Equals(id) && r.TicketCommentParentId.Equals(0) select r).AsEnumerable();
        }

        public IEnumerable<TicketComment> GetTicketCommentReplys(long commentId)
        {
            return (from r in db.TicketComments where r.TicketCommentParentId.Equals(commentId) orderby r.TicketCommentTimeStamp ascending select r).AsEnumerable();
        }


        public void Add(TicketComment ticketComment)
        {
            db.TicketComments.InsertOnSubmit(ticketComment);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }

}