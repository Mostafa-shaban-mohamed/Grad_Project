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
    public class AttendanceController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Attendance
        [Authorize(Roles = "Admin, Student, Lecturer")]
        public ActionResult Index(string Search, string Courses, int? Page_No)
        {
            var attendance_tbl = db.Attendance_tbl.AsQueryable();
            if (User.IsInRole("Lecturer"))
            {
                var lec = db.Lecturer_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                ViewBag.Courses = new SelectList(db.Course_tbl.Where(m => m.Prof == lec.ID || m.Assistant == lec.ID), "ID", "Name");
                attendance_tbl = db.Attendance_tbl.Include(a => a.Course_tbl).Include(a => a.Student_tbl).Where(m => m.Course_tbl.Prof == lec.ID ||
                m.Course_tbl.Assistant == lec.ID);
            }
            else if (User.IsInRole("Student"))
            {
                var std = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                var reg = db.RegisteredCourses_tbl.Find(std.ID);
                ViewBag.Courses = new SelectList(db.Course_tbl.Where(m => m.ID.Contains(reg.Course01) || m.ID.Contains(reg.Course02)
                || m.ID.Contains(reg.Course03) || m.ID.Contains(reg.Course04) || m.ID.Contains(reg.Course05)
                || m.ID.Contains(reg.Course05) || m.ID.Contains(reg.Course06)), "ID", "Name");
                attendance_tbl = db.Attendance_tbl.Include(a => a.Course_tbl).Include(a => a.Student_tbl).Where(m => m.StudentID == std.ID);
            }
            else //Admin
            {
                ViewBag.Courses = new SelectList(db.Course_tbl, "ID", "Name");
                attendance_tbl = db.Attendance_tbl.Include(a => a.Course_tbl).Include(a => a.Student_tbl);
            }
            
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Courses))
            {
                attendance_tbl = attendance_tbl.Where(m => m.CourseID.Contains(Courses));
            }
            if (!string.IsNullOrEmpty(Search))
            {
                return View(attendance_tbl.Where(m => m.StudentID.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }

            return View(attendance_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Attendance/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance_tbl attendance_tbl = db.Attendance_tbl.Find(id);
            if (attendance_tbl == null)
            {
                return HttpNotFound();
            }
            return View(attendance_tbl);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Attendance_tbl attendance_tbl = db.Attendance_tbl.Find(id);
            db.Attendance_tbl.Remove(attendance_tbl);
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
