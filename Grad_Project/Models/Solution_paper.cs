using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Grad_Project.Models;

namespace Grad_Project.Models
{
    public class Solution_paper
    {
        [Display(Name = "Student Code")]
        public string Student_ID { get; set; }
        [Display(Name = "Quiz Code")]
        public string Quiz_ID { get; set; }

        public List<ExamQues> exam { get; set; }
    }
}