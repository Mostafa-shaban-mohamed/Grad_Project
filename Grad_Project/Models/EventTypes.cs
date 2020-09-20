using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class EventTypes
    {
        public List<string> Type { get; set; }

        public List<string> GetEventType()
        {
            List<string> typ = new List<string>()
            {
                "Assignment",
                "Announcement"
            };
            return typ;
        }
    }
}