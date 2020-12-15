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
using PagedList;

namespace Grad_Project.Controllers
{
    public class StudentController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();
        private static string str;

        // GET: Student
        public ActionResult Index(string Search, int? Page_No)
        {
            var student_tbl = db.Student_tbl.Include(s => s.Attendance_tbl1).Include(s => s.RegisteredCourses_tbl).Include(s => s.Result_tbl1);
            var studentList = new List<Student_tbl>();
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                if (studentList.Count != 0)
                {
                    student_tbl = studentList.Where(m => m.Name.Contains(Search)).AsQueryable();
                }
                else
                {
                    student_tbl = student_tbl.Where(m => m.Name.Contains(Search)).AsQueryable();
                }
                return View(student_tbl.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }

            if (studentList.Count == 0)
            {
                return View(student_tbl.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }
            else
            {
                return View(studentList.OrderBy(m => m.ID).ToPagedList(No_Of_Page, Size_Of_Page));
            }
        }

        // GET: Student/Details/5
        [Authorize(Roles = "Student")]
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

        //Change Password ------------------------------
        [HttpGet]
        [Authorize(Roles = "Student")]
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_tbl std = db.Student_tbl.Find(id);
            if (std == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM CPVM)
        {
            if (string.IsNullOrWhiteSpace(CPVM.OldPassword) || string.IsNullOrWhiteSpace(CPVM.NewPassword))
            {
                return HttpNotFound();
            }
            var std = db.Student_tbl.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            //check if admin is null
            if (std == null)
            {
                return HttpNotFound();
            }
            //hashing old taken password
            var oldHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.OldPassword), std.Salt));
            //Check if oldpassowrd is the user password
            if (oldHashedPassword == std.Password && !string.IsNullOrWhiteSpace(CPVM.NewPassword))
            {
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.NewPassword), std.Salt));
                std.Password = NewHashedPassword;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = std.ID });
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public ActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(string id, FileDataVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
            // now you could pass the byte array to your model and store wherever 
            // you intended to store it
            var art = db.Student_tbl.Find(id);
            art.Image = uploadedFile;
            db.Entry(art).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Details", new { id = id });
        }

        // GET: Student/Edit/5
        [Authorize(Roles = "Student, Admin")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student_tbl student_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = student_tbl.ID });
            }
            return View(student_tbl);
        }

        // GET: Student/Delete/5
        [Authorize(Roles = "Admin")]
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
