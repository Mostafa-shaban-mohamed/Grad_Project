using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grad_Project.Models
{
    public class QestionType
    {
        public List<string> QuesType { get; set; }

        public List<string> GetQT()
        {
            List<string> QT = new List<string>()
            {
                "Multi-chose",
                "Written",
                "FileAnswer"
            };

            return QT;
        }
    }
}