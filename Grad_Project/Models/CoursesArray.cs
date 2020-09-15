using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class CoursesArray
    {
        public List<Course_tbl> C_List { get; set; }

        public List<Course_tbl> GetCourses()
        {
            using(LMSDBEntities db = new LMSDBEntities())
            {
                C_List = db.Course_tbl.ToList();
            }
            return C_List;
        }

    }
}