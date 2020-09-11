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
    public class ResultController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Result
        public ActionResult Index()
        {
            var result_tbl = db.Result_tbl.Include(r => r.Course_tbl).Include(r => r.Student_tbl);
            return View(result_tbl.ToList());
        }

        // GET: Result/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result_tbl result_tbl = db.Result_tbl.Find(id);
            if (result_tbl == null)
            {
                return HttpNotFound();
            }
            return View(result_tbl);
        }

        // GET: Result/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name");
            return View();
        }

        // POST: Result/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CourseID,StudentID,Total_Mark,Achieved_Mark")] Result_tbl result_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Result_tbl.Add(result_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", result_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", result_tbl.StudentID);
            return View(result_tbl);
        }

        // GET: Result/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result_tbl result_tbl = db.Result_tbl.Find(id);
            if (result_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", result_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", result_tbl.StudentID);
            return View(result_tbl);
        }

        // POST: Result/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CourseID,StudentID,Total_Mark,Achieved_Mark")] Result_tbl result_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", result_tbl.CourseID);
            ViewBag.StudentID = new SelectList(db.Student_tbl, "ID", "Name", result_tbl.StudentID);
            return View(result_tbl);
        }

        // GET: Result/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result_tbl result_tbl = db.Result_tbl.Find(id);
            if (result_tbl == null)
            {
                return HttpNotFound();
            }
            return View(result_tbl);
        }

        // POST: Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Result_tbl result_tbl = db.Result_tbl.Find(id);
            db.Result_tbl.Remove(result_tbl);
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
