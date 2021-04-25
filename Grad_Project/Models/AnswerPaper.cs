using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class AnswerPaper
    {
        public string Stu_Code { get; set; }
        public string Exam_ID { get; set; }
        public List<string> AnswersList { get; set; }
        public List<HttpPostedFileBase> AnsFiles { get; set; }
    }
}