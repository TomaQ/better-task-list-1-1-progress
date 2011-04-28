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
        //[Authorize, HttpGet]
        public ActionResult Create()
        {
            // Ticket ticket = new Ticket();
            // ticket.TicketCreatorUserId = UserHelpers.GetUserId(User.Identity.Name);
            // ticket.TicketDueDate = DateTime.Now.AddDays(2);

            // TicketRepository ticketRepository = new TicketRepository();
            // ticketRepository.Add(ticket);
            // ticketRepository.Save();

            // return RedirectToAction("Edit", new {id = 87});
            return View();

        }

        public ActionResult Edit(int id)
        {
            Ticket ticket = ticketRepository.GetTicket(id);
            return View(ticket);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            Ticket ticket =ticketRepository.GetTicket(id);

            ticket.TicketPriority = formCollection["TicketPriority"];

            try
            {
                UpdateModel(ticket);
                ticketRepository.Save();

                return View(ticket);
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
