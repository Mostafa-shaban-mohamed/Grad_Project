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
using System.Dynamic;
using System.IO;

namespace Grad_Project.Controllers
{
    public class CourseController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();
        private static string course_ID;

        // GET: Course
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult Index(string Search, int? Page_No)
        {
            var course_tbl = db.Course_tbl.Include(c => c.Lecturer_tbl).Include(c => c.Lecturer_tbl1);
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                return View(course_tbl.Where(m => m.Name.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }
            
            return View(course_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }


        // GET: Course/Details/5
        [Authorize(Roles = "Lecturer, Admin, Student")]
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
        [Authorize(Roles = "Admin")]
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
        public ActionResult Create(Course_tbl course_tbl)
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
        [Authorize(Roles = "Lecturer, Admin")]
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
            //Allow lecturers of this course and admins to access
            if(User.Identity.Name != db.Lecturer_tbl.Find(course_tbl.Prof).Email && User.Identity.Name != db.Lecturer_tbl.Find(course_tbl.Assistant).Email && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ViewBag.Prof = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Prof);
            ViewBag.Assistant = new SelectList(db.Lecturer_tbl, "ID", "Name", course_tbl.Assistant);
            course_ID = id;
            return View(course_tbl);
        }


        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course_tbl course_tbl)
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



        [HttpGet]
        [Authorize(Roles = "Lecturer, Admin")]
        public ActionResult UploadFiles()
        {
            var model = new FileDataVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult UploadFiles(FileDataVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            // now you could pass the byte array to your model and store wherever 
            // you intended to store it
            File_tbl fl = new File_tbl()
            {
                ID = db.File_tbl.Count() + 1,
                FileName = model.File.FileName,
                UploadOn = DateTime.Now,
                File = uploadedFile
            };
            db.File_tbl.Add(fl);
            db.SaveChanges();
            var course_tbl = db.Course_tbl.Find(course_ID) as Course_tbl;
            course_tbl.PDFs += fl.ID.ToString() + "/";
            db.Entry(course_tbl).State = EntityState.Modified;
            db.SaveChanges();
            
            
            return RedirectToAction("Edit", new { id = course_ID });
        }
        
        public FileResult ViewFile(string Name)
        {
            var fl = db.File_tbl.FirstOrDefault(m => m.FileName == Name);
            string contentType = MimeMapping.GetMimeMapping(fl.FileName);
            return File(fl.File, contentType);
        }

        // GET: Course/Delete/5
        [Authorize(Roles = "Admin")]
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
