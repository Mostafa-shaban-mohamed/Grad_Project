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
    public class CourseController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Course
        public ActionResult Index()
        {
            var course_tbl = db.Course_tbl.Include(c => c.Lecturer_tbl).Include(c => c.Lecturer_tbl1);
            return View(course_tbl.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_tbl course_tbl = db.Course_tbl.Find(id);
            if (course_tbl == null)
            {
                return HttpNotFound();
            }
            return View(course_tbl);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.Prof = new SelectList(db.Lecturer_tbl, "ID", "Name");
            ViewBag.Assistant = new SelectList(db.Lecturer_tbl, "ID", "Name");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Prof,Assistant,PDFs,Links")] Course_tbl course_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Course_tbl.Add(course_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Prof = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Prof);
            ViewBag.Assistant = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Assistant);
            return View(course_tbl);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_tbl course_tbl = db.Course_tbl.Find(id);
            if (course_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Prof = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Prof);
            ViewBag.Assistant = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Assistant);
            return View(course_tbl);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Prof,Assistant,PDFs,Links")] Course_tbl course_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Prof = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Prof);
            ViewBag.Assistant = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Assistant);
            return View(course_tbl);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_tbl course_tbl = db.Course_tbl.Find(id);
            if (course_tbl == null)
            {
                return HttpNotFound();
            }
            return View(course_tbl);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Course_tbl course_tbl = db.Course_tbl.Find(id);
            db.Course_tbl.Remove(course_tbl);
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
