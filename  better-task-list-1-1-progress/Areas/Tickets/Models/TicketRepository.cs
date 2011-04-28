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
}