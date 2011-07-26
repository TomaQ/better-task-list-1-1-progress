using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;

namespace BetterTaskList.Models
{
    public partial class Profile
    {

        public bool IsLockedOut
        {
            get
            {
                MembershipUser mu = Membership.GetUser(UserId, false);
                return mu.IsLockedOut;
            }
        }

        public bool IsApproved
        {
            get
            {
                MembershipUser mu = Membership.GetUser(UserId, false);
                return mu.IsApproved;
            }
        }


        // TimeZones
        public SelectList TimeZones
        {
            get { return new SelectList(GetTimeZones(), "Key", "Value", TimeZone); }
        }

        private IDictionary<string, string> GetTimeZones()
        {
            return (from t in TimeZoneInfo.GetSystemTimeZones() select new { t.Id, t.DisplayName }).AsEnumerable().ToDictionary(k => k.Id, v => v.DisplayName);
        }

        // CellPhoneCarrierDomainNames = new SelectList(ProfileHelpers.CellPhoneCarriers, "Key", "Value", profile.CellPhoneCarrierDomainName)
        // Cell Phone Carriers 
        public SelectList CellPhoneCarriers
        {
            get { return new SelectList(GetCellPhoneCarriers(), "Key", "Value", CellPhoneCarrier); }
        }

        private IDictionary<string, string> GetCellPhoneCarriers()
        {
            Dictionary<string, string> carrierDomainNames = new Dictionary<string, string>();
            carrierDomainNames.Add("@nomobileservice.com", "No mobile service");
            carrierDomainNames.Add("@vtext.com", "Verizon Wireless");
            carrierDomainNames.Add("@tmomail.net", "T-Mobile");
            carrierDomainNames.Add("@vmobl.com", "Virgin Mobile");
            carrierDomainNames.Add("@cingularme.com", "Cingular");
            carrierDomainNames.Add("@messaging.nextel.com", "Nextel");
            carrierDomainNames.Add("@message.alltel.com", "Alltel");
            carrierDomainNames.Add("@messaging.sprintpcs.com", "Sprint PCS");
            carrierDomainNames.Add("@txt.att.net", "AT&T Mobile");
            return carrierDomainNames;
        }


    }
}