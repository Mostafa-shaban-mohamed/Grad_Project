using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class ChangePasswordVM
    {
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}