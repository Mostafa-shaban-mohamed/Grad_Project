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
    public class FileController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: File
        public ActionResult Index()
        {
            return View(db.File_tbl.ToList());
        }

        // GET: File/Details/5
        public ActionResult Details(int? id)
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

        // GET: File/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: File/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileName,UploadOn,File")] File_tbl file_tbl)
        {
            if (ModelState.IsValid)
            {
                db.File_tbl.Add(file_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(file_tbl);
        }

        // GET: File/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: File/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,UploadOn,File")] File_tbl file_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(file_tbl);
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
