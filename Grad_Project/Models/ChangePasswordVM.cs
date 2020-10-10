using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class ChangePasswordVM
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}