using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class Departments
    {
        public List<string> Department { get; set; }

        public List<string> GetDepartment()
        {
            List<string> Depart = new List<string>()
            {
                "Civil Engineering",
                "Electrical Engineering",
                "Electronics & Communication Engineering",
                "Architectical Engineering",
                "Mechatronics Engineering"
            };

            return Depart;
        }
    }
}