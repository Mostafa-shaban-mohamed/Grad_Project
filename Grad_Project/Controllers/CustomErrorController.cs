using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grad_Project.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}