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
    public class FileController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: File
        [Authorize(Roles = "Lecturer, Admin, Student")]
        public ActionResult Index(string Search, string Courses, string assigID, int? Page_No)
        {
            if (User.IsInRole("Student"))
            {
                var stu = db.Student_tbl.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
                var reg = db.RegisteredCourses_tbl.Find(stu.ID);
                if(reg == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //add registered courses in list
                var stuCourses = new List<Course_tbl>();
                if(reg.Course01 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course01));
                }
                if (reg.Course02 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course02));
                }
                if (reg.Course03 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course03));
                }
                if (reg.Course04 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course04));
                }
                if (reg.Course05 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course05));
                }
                if (reg.Course06 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course06));
                }
                if (reg.Course07 != null)
                {
                    stuCourses.Add(db.Course_tbl.Find(reg.Course07));
                }
                ViewBag.Courses = new SelectList(stuCourses, "ID", "Name");
            }
            else
            {
                ViewBag.Courses = new SelectList(db.Course_tbl, "ID", "Name");
            }
            
            int Size_Of_Page = 6;
            int No_Of_Page = (Page_No ?? 1);

            //Drop down List
            if (!string.IsNullOrEmpty(Courses))
            {
                var course = db.Course_tbl.Find(Courses);
                var fls = new List<File_tbl>();
                fls = db.File_tbl.Where(m => m.CourseID == Courses).ToList();

                //Assig Documents
                if(assigID != null)
                {
                    fls = fls.Where(m => m.AssignmentID == assigID).ToList();
                }
                //Search box
                if (!string.IsNullOrEmpty(Search))
                {
                    return View(fls.Where(m => m.FileName.Contains(Search)).ToPagedList(No_Of_Page, Size_Of_Page));
                    //return View(db.File_tbl.Where(m => m.FileName.Contains(Search)).ToPagedList(No_Of_Page, Size_Of_Page));
                }
                else
                {
                    return View(fls.ToPagedList(No_Of_Page, Size_Of_Page));
                }
            }
            
            return View(db.File_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }
        
        // GET: File/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File_tbl file_tbl = db.File_tbl.Find(id);
            if (file_tbl == null)
            {
                return HttpNotFound();
            }
            return View(file_tbl);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File_tbl file_tbl = db.File_tbl.Find(id);
            db.File_tbl.Remove(file_tbl);
            db.SaveChanges();
            return RedirectToAction("Index");
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
