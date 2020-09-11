using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class Role
    {
        public List<string> role { get; set; }

        public List<string> GetRole()
        {
            List<string> rol = new List<string>()
            {
                "Prof",
                "Assistant"
            };

            return rol;
        }
    }
}