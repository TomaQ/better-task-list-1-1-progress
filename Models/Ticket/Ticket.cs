using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterTaskList.Models
{
    public partial class Ticket
    {
        public string TicketResolutionTimeToLongString
        {
            get
                {
                    if(TicketResolutionTime.HasValue)
                    {
                        var resolutionTime = TimeSpan.FromTicks(TicketResolutionTime.Value);
                        int days, hours, min;

                        days = resolutionTime.Days;
                        hours = resolutionTime.Hours;
                        min = resolutionTime.Minutes;

                        if(days == 1)
                        { return @days + " Day"; }

                        if(days > 1)
                        { return @days + " Days"; }

                        if(days == 0 && hours == 1)
                        { return @hours + " Hour"; }

                        if(days == 0 && hours > 1)
                        { return @hours + " Hours " + @min + " Minutes"; }
                        
                        if(days == 0 && hours == 0 && min > 0)
                        { return @min + " Minutes"; }


                    }
                    // If we got here then no TicketResolutionTime has been defined
                    return "Unknown";
                }
        }

    }
}