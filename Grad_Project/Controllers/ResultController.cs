using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grad_Project.Models;
using PagedList;

namespace Grad_Project.Controllers
{
    public class ResultController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Result
        [Authorize(Roles = "Lecturer, Student")]
        public ActionResult Index(string Search, int? Page_No)
        {
            var result_tbl = db.Result_tbl.Include(r => r.Course_tbl).Include(r => r.Student_tbl);
            if (User.IsInRole("Student"))
            {
                var std = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                result_tbl = db.Result_tbl.Where(m => m.StudentID == std.ID).Include(r => r.Course_tbl).Include(r => r.Student_tbl);
            }
            
            int Size_Of_Page = 6;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                return View(result_tbl.Where(m => m.Title.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }
            return View(result_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }
        
        //AutoComplete mechanism for student code
        public JsonResult SearchStd(string term)
        {
            List<string> Loc = db.Student_tbl.Where(x => x.ID.Contains(term)).Select(x => x.ID).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }

        //AutoComplete mechanism for course code
        public JsonResult SearchCourse(string term)
        {
            List<string> Loc = db.Course_tbl.Where(x => x.ID.Contains(term)).Select(x => x.ID).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
