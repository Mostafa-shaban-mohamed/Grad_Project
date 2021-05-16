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
        [Authorize(Roles = "Admin, Student, Lecturer")]
        public ActionResult Index()
        {
            var event_tbl = db.Event_tbl.Include(e => e.Course_tbl).OrderByDescending(m => m.ReleaseDate);
            if (User.IsInRole("Student"))
            {
                var std = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                var att = db.Attendance_tbl.Where(m => m.StudentID == std.ID).ToList();
                List<Event_tbl> ev = new List<Event_tbl>();
                //Add Courses
                foreach(var e in att)
                {
                    if(db.Event_tbl.FirstOrDefault(m => m.CourseID == e.CourseID) != null)
                        ev.Add(db.Event_tbl.FirstOrDefault(m => m.CourseID == e.CourseID));
                }
                var evl = event_tbl.Where(m => m.Ed_Level == std.Ed_Level).ToList();
                //Add Levels
                foreach (var u in evl)
                {
                    ev.Add(u);
                }
                return View(ev);
            }
            if (User.IsInRole("Lecturer"))
            {
                var lec = db.Lecturer_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                var courses = db.Course_tbl.Where(m => m.Prof == lec.ID || m.Assistant == lec.ID).ToList();
                List<Event_tbl> ev = new List<Event_tbl>();
                foreach (var c in courses)
                {
                    if(db.Event_tbl.FirstOrDefault(m => m.CourseID == c.ID) != null)
                        ev.Add(db.Event_tbl.FirstOrDefault(m => m.CourseID == c.ID));
                }
                return View(ev);
            }
            return View(event_tbl.ToList());
        }

        // GET: Event/Details/5
        [Authorize(Roles = "Admin, Student, Lecturer")]
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

        [HttpGet]
        [Authorize(Roles = "Student")]
        public ActionResult UploadAssig(string id)
        {
            var model = new FileDataVM();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult UploadAssig(string id, FileDataVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
            var assig = db.Event_tbl.Find(id);

            // now you could pass the byte array to your model and store wherever 
            // you intended to store it
            File_tbl fl = new File_tbl()
            {
                FileName = model.File.FileName,
                UploadOn = DateTime.Now,
                File = uploadedFile,
                CourseID = assig.CourseID,
                AssignmentID = assig.ID
            };
            db.File_tbl.Add(fl);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Event/Create
        [Authorize(Roles = "Admin, Lecturer")]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name");
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event_tbl event_tbl)
        {
            if (ModelState.IsValid)
            {
                event_tbl.ReleaseDate = DateTime.Now;
                db.Event_tbl.Add(event_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Course_tbl, "ID", "Name", event_tbl.CourseID);
            return View(event_tbl);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin, Lecturer")]
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
        [Authorize(Roles = "Admin, Lecturer")]
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
