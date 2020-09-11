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
    public class EventController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Event
        public ActionResult Index()
        {
            var event_tbl = db.Event_tbl.Include(e => e.Course_tbl);
            return View(event_tbl.ToList());
        }

        // GET: Event/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_tbl event_tbl = db.Event_tbl.Find(id);
            if (event_tbl == null)
            {
                return HttpNotFound();
            }
            return View(event_tbl);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,ReleaseDate,Type,Ed_Level,CourseID")] Event_tbl event_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Event_tbl.Add(event_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", event_tbl.CourseID);
            return View(event_tbl);
        }

        // GET: Event/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_tbl event_tbl = db.Event_tbl.Find(id);
            if (event_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", event_tbl.CourseID);
            return View(event_tbl);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,ReleaseDate,Type,Ed_Level,CourseID")] Event_tbl event_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(event_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", event_tbl.CourseID);
            return View(event_tbl);
        }

        // GET: Event/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event_tbl event_tbl = db.Event_tbl.Find(id);
            if (event_tbl == null)
            {
                return HttpNotFound();
            }
            return View(event_tbl);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Event_tbl event_tbl = db.Event_tbl.Find(id);
            db.Event_tbl.Remove(event_tbl);
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
