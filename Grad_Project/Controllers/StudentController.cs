using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grad_Project.Models;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Grad_Project.Controllers
{
    public class StudentController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();
        private static string str;

        // GET: Student
        public ActionResult Index()
        {
            var student_tbl = db.Student_tbl.Include(s => s.Attendance_tbl1).Include(s => s.RegisteredCourses_tbl).Include(s => s.Result_tbl1);
            return View(student_tbl.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_tbl student_tbl = db.Student_tbl.Find(id);
            if (student_tbl == null)
            {
                return HttpNotFound();
            }
            return View(student_tbl);
        }

        //Hashing methods ---------------------------------------------
        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];

                rng.GetBytes(randomNumber);

                return randomNumber;

            }
        }
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.Attend_Courses = new SelectList(db.Attendance_tbl, "ID", "StudentID");
            ViewBag.Registered_Courses = new SelectList(db.RegisteredCourses_tbl, "ID", "Course01");
            ViewBag.Results = new SelectList(db.Result_tbl, "ID", "CourseID");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student_tbl student_tbl)
        {
            var salt = GenerateSalt();
            if (ModelState.IsValid)
            {
                student_tbl.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(student_tbl.Password), salt));
                student_tbl.Salt = salt;
                str = student_tbl.ID;
                db.Student_tbl.Add(student_tbl);
                db.SaveChanges();
                return RedirectToAction("UploadImage");
            }

            ViewBag.Attend_Courses = new SelectList(db.Attendance_tbl, "ID", "StudentID", student_tbl.Attend_Courses);
            ViewBag.Registered_Courses = new SelectList(db.RegisteredCourses_tbl, "ID", "Course01", student_tbl.Registered_Courses);
            ViewBag.Results = new SelectList(db.Result_tbl, "ID", "CourseID", student_tbl.Results);
            return View(student_tbl);
        }

        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(int? a)
        {
            try
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/Images/"), fname);
                    file.SaveAs(fname);
                    var bytes = System.IO.File.ReadAllBytes(fname);
                    var std = db.Student_tbl.Find(str);
                    std.Image = bytes;
                    db.SaveChanges();
                }
                // message is successfully uploaded  
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_tbl student_tbl = db.Student_tbl.Find(id);
            if (student_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Attend_Courses = new SelectList(db.Attendance_tbl, "ID", "StudentID", student_tbl.Attend_Courses);
            ViewBag.Registered_Courses = new SelectList(db.RegisteredCourses_tbl, "ID", "Course01", student_tbl.Registered_Courses);
            ViewBag.Results = new SelectList(db.Result_tbl, "ID", "CourseID", student_tbl.Results);
            return View(student_tbl);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Password,Salt,Department,Ed_Level,Image,Attend_Courses,Registered_Courses,Results")] Student_tbl student_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Attend_Courses = new SelectList(db.Attendance_tbl, "ID", "StudentID", student_tbl.Attend_Courses);
            ViewBag.Registered_Courses = new SelectList(db.RegisteredCourses_tbl, "ID", "Course01", student_tbl.Registered_Courses);
            ViewBag.Results = new SelectList(db.Result_tbl, "ID", "CourseID", student_tbl.Results);
            return View(student_tbl);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_tbl student_tbl = db.Student_tbl.Find(id);
            if (student_tbl == null)
            {
                return HttpNotFound();
            }
            return View(student_tbl);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student_tbl student_tbl = db.Student_tbl.Find(id);
            db.Student_tbl.Remove(student_tbl);
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
