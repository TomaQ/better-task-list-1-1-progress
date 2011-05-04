using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class TicketComment
    {
        BetterTaskListDataContext db = new BetterTaskListDataContext();

        public string CommentSubmitterFullName
        {
            get { return (from r in db.Profiles where r.UserId.Equals(TicketCommentSubmitterUserId) select r.FirstName + " " + r.LastName).Single(); }
        }
    }

}

