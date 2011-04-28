using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class Ticket
    {
        public void Onvalidate(System.Data.Linq.ChangeAction action)
        {
            
        }

        public bool IsValid
        {
            get { return (GetRuleViolations().Count() == 0); }
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            // FYI: Make sure that if you add any rule here that you also update the account register
            // post action as well otherwise you will end up with a broken registration process!

            //if (string.IsNullOrEmpty(FullName))
            //    yield return new RuleViolation("Full Name is required", "FullName");

            yield break;
        }


    }
}