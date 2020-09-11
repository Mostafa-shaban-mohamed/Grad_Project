using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grad_Project.Models;

namespace Grad_Project.Controllers
{
    public class AttendanceController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Attendance
        public ActionResult Index()
        {
            var attendance_tbl = db.Attendance_tbl.Include(a => a.Course_tbl).Include(a => a.Student_tbl);
            return View(attendance_tbl.ToList());
        }

        // GET: Attendance/Details/5
        public ActionResult Details(string id)
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

        // GET: Attendance/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name");
            return View();
        }

        // POST: Attendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,CourseID,No_of_Attendances")] Attendance_tbl attendance_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Attendance_tbl.Add(attendance_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", attendance_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", attendance_tbl.StudentID);
            return View(attendance_tbl);
        }

        // GET: Attendance/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", attendance_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", attendance_tbl.StudentID);
            return View(attendance_tbl);
        }

        // POST: Attendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,CourseID,No_of_Attendances")] Attendance_tbl attendance_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", attendance_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", attendance_tbl.StudentID);
            return View(attendance_tbl);
        }

        // GET: Attendance/Delete/5
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
