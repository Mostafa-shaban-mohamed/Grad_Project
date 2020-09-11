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
    public class RegisteredCoursesController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: RegisteredCourses
        public ActionResult Index()
        {
            var registeredCourses_tbl = db.RegisteredCourses_tbl.Include(r => r.Course_tbl).Include(r => r.Course_tbl1).Include(r => r.Course_tbl2).Include(r => r.Course_tbl3).Include(r => r.Course_tbl4).Include(r => r.Course_tbl5).Include(r => r.Course_tbl6);
            return View(registeredCourses_tbl.ToList());
        }

        // GET: RegisteredCourses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }
            return View(registeredCourses_tbl);
        }

        // GET: RegisteredCourses/Create
        public ActionResult Create()
        {
            ViewBag.Course01 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course02 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course03 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course04 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course05 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course06 = new SelectList(db.Course_tbl, "ID", "Name");
            ViewBag.Course07 = new SelectList(db.Course_tbl, "ID", "Name");
            return View();
        }

        // POST: RegisteredCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Course01,Course02,Course03,Course04,Course05,Course06,Course07")] RegisteredCourses_tbl registeredCourses_tbl)
        {
            if (ModelState.IsValid)
            {
                db.RegisteredCourses_tbl.Add(registeredCourses_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course01 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course01);
            ViewBag.Course02 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course02);
            ViewBag.Course03 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course03);
            ViewBag.Course04 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course04);
            ViewBag.Course05 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course05);
            ViewBag.Course06 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course06);
            ViewBag.Course07 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course07);
            return View(registeredCourses_tbl);
        }

        // GET: RegisteredCourses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course01 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course01);
            ViewBag.Course02 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course02);
            ViewBag.Course03 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course03);
            ViewBag.Course04 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course04);
            ViewBag.Course05 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course05);
            ViewBag.Course06 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course06);
            ViewBag.Course07 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course07);
            return View(registeredCourses_tbl);
        }

        // POST: RegisteredCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Course01,Course02,Course03,Course04,Course05,Course06,Course07")] RegisteredCourses_tbl registeredCourses_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registeredCourses_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course01 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course01);
            ViewBag.Course02 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course02);
            ViewBag.Course03 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course03);
            ViewBag.Course04 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course04);
            ViewBag.Course05 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course05);
            ViewBag.Course06 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course06);
            ViewBag.Course07 = new SelectList(db.Course_tbl, "ID", "Name", registeredCourses_tbl.Course07);
            return View(registeredCourses_tbl);
        }

        // GET: RegisteredCourses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }
            return View(registeredCourses_tbl);
        }

        // POST: RegisteredCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            db.RegisteredCourses_tbl.Remove(registeredCourses_tbl);
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
