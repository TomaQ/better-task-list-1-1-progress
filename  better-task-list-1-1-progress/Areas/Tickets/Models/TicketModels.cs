using System;
using System.Web;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BetterTaskList.Models
{
    public class CreateTicketModel
    {
        [Required]
        [Display(Name = "Title")]
        [DataType(DataType.Text)]
        public string TicketName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string TicketDescription { get; set; }

        [Display(Name = "Due")]
        [DataType(DataType.Date)]
        public DateTime TicketDueDate { get; set; }

        [Display(Name = "Notification list")]
        [DataType(DataType.Text)]
        public string TicketNotificationList { get; set; }

        [Display(Name = "Tags")]
        [DataType(DataType.Text)]
        public string TicketTags { get; set; }

        [Display(Name = "Priority")]
        public string TicketPriority { get; set; }

        [Display(Name = "Status")]
        public string TicketStatus { get; set; }

        [Required]
        [Display(Name = "Assigned To")]
        public Guid TicketAssignedToUserId { get; set; }

    }
}