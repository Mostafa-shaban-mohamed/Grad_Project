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
        [HttpGet]
        [Authorize(Roles = "Lecturer, Admin, Student")]
        public ActionResult Index(string Search, int? Page_No)
        {
            var course_tbl = db.Course_tbl.Include(c => c.Lecturer_tbl).Include(c => c.Lecturer_tbl1);
            var coursesList = new List<Course_tbl>();
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);

            if (User.IsInRole("Student"))
            {
                var std = db.Student_tbl.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
                if (std != null)
                {
                    var IDCode = std.ID;
                    var reg = db.RegisteredCourses_tbl.Find(IDCode);
                    if (reg.Course01 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course01));
                    }
                    if (reg.Course02 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course02));
                    }
                    if (reg.Course03 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course03));
                    }
                    if (reg.Course04 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course04));
                    }
                    if (reg.Course05 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course05));
                    }
                    if (reg.Course06 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course06));
                    }
                    if (reg.Course07 != null)
                    {
                        coursesList.Add(db.Course_tbl.Find(reg.Course07));
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(Search))
            {
                if (coursesList.Count != 0)
                {
                    course_tbl = coursesList.Where(m => m.Name.Contains(Search)).AsQueryable();
                }
                else
                {
                    course_tbl = course_tbl.Where(m => m.Name.Contains(Search)).AsQueryable();
                }
                return View(course_tbl.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }

            if(coursesList.Count == 0)
            {
                return View(course_tbl.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }
            else
            {
                return View(coursesList.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }
            
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
            course_tbl.PDFs = db.File_tbl.Where(m => m.CourseID == course_tbl.ID).Count().ToString();
            if (course_tbl == null)
            {
                return HttpNotFound();
            }
            return PartialView(course_tbl);
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
                FileName = model.File.FileName,
                UploadOn = DateTime.Now,
                File = uploadedFile,
                CourseID = course_ID
            };
            db.File_tbl.Add(fl);
            db.SaveChanges();
            db.Course_tbl.Find(course_ID).PDFs = db.File_tbl.Where(m => m.CourseID == course_ID).Count().ToString();
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
