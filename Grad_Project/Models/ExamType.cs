using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class ExamType
    {
        public List<string> Type { get; set; }

        public List<string> GetExamType()
        {
            List<string> typ = new List<string>()
            {
                "Quiz",
                "Exam"
            };
            return typ;
        }
    }
}