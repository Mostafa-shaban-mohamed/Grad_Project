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
    public class QuestionController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Question/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_tbl question_tbl = db.Question_tbl.Find(id);
            if (question_tbl == null)
            {
                return HttpNotFound();
            }
            return View(question_tbl);
        }
        [Authorize(Roles = "Lecturer")]
        // GET: Question/Create
        public ActionResult Create()
        {
            var lec = db.Lecturer_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
            if (lec == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseID = new SelectList(db.Course_tbl.Where(m => m.Prof == lec.ID || m.Assistant == lec.ID), "ID", "Name");

            return PartialView("Create");
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question_tbl question_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Question_tbl.Add(question_tbl);
                db.SaveChanges();
                return RedirectToAction("Create", "Exam");
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            return PartialView(question_tbl);
        }

        // GET: Question/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_tbl question_tbl = db.Question_tbl.Find(id);
            if (question_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            return View(question_tbl);
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question_tbl question_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question_tbl);
        }

        // GET: Question/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_tbl question_tbl = db.Question_tbl.Find(id);
            if (question_tbl == null)
            {
                return HttpNotFound();
            }
            return View(question_tbl);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Question_tbl question_tbl = db.Question_tbl.Find(id);
            db.Question_tbl.Remove(question_tbl);
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
