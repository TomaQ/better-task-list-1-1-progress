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
            //NOTE: Commented code below for future reference only
            //var emailList = new string[] {"geovanimartinez@yovasolutions.com", "juanmartinez@yovasolutions.com", "oscarmartinez@yovasolutions.com" };
            //return emailList;
            
            // We only return active users so if someones account has been disabled (IsApproved = false) we exclude them. 
            return (from u in db.Users where u.LoweredUserName.Contains(searchString) && u.Memberships.IsApproved.Equals(true) select u.LoweredUserName).ToArray();
        }

        public IEnumerable<Ticket> SearchTicketsTittles(string q)
        {
            var results = (from r in db.Tickets where r.TicketSubject.Contains(q) select r);
            return results;
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

        public int GetTicketCommentCount(long ticketId)
        {
            return (from r in db.TicketComments where r.TicketId.Equals(ticketId) select r.TicketId).Count();
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