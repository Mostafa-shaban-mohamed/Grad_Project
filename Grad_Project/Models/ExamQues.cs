using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class ExamQues
    {
        public string quesCode { get; set; }
        public string quesTitle { get; set; }
        public string FirstCh { get; set; }
        public string SecondCh { get; set; }
        public string ThirdCh { get; set; }
        public string FourthCh { get; set; }
        public string studentAnswer { get; set; }
        public bool isCorrectAnswer { get; set; }
    }
}