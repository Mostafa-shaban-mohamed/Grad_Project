using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class FileDataVM
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}