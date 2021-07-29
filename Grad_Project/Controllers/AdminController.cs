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

namespace Grad_Project.Controllers
{
    public class AdminController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Admin/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_tbl admin_tbl = db.Admin_tbl.Find(id);
            if (admin_tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_tbl);
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
        
        // GET: Admin/Create
        [Route("Admin/Register")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [Route("Admin/Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin_tbl admin_tbl)
        {
            var salt = GenerateSalt();
            if (ModelState.IsValid)
            {
                admin_tbl.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(admin_tbl.Password), salt));
                admin_tbl.Salt = salt;
                db.Admin_tbl.Add(admin_tbl);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(admin_tbl);
        }

        //Change Password ------------------------------
        [HttpGet]
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_tbl admin_tbl = db.Admin_tbl.Find(id);
            if (admin_tbl == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM CPVM)
        {
            if(string.IsNullOrWhiteSpace(CPVM.OldPassword) || string.IsNullOrWhiteSpace(CPVM.NewPassword))
            {
                return HttpNotFound();
            }
            var admin = db.Admin_tbl.Where( m => m.Email == User.Identity.Name).FirstOrDefault();
            //check if admin is null
            if(admin == null)
            {
                return HttpNotFound();
            }
            //hashing old taken password
            var oldHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.OldPassword), admin.Salt));
            //Check if oldpassowrd is the user password
            if (oldHashedPassword == admin.Password && !string.IsNullOrWhiteSpace(CPVM.NewPassword))
            {
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.NewPassword), admin.Salt));
                admin.Password = NewHashedPassword;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = admin.ID });
            }

            return View();
        }


        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_tbl admin_tbl = db.Admin_tbl.Find(id);
            if (admin_tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_tbl);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin_tbl admin_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin_tbl);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin_tbl admin_tbl = db.Admin_tbl.Find(id);
            if (admin_tbl == null)
            {
                return HttpNotFound();
            }
            return View(admin_tbl);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin_tbl admin_tbl = db.Admin_tbl.Find(id);
            db.Admin_tbl.Remove(admin_tbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // to reset passwords of students & lecturers & other admins 
        [HttpGet]
        public ActionResult ResetPasswords()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPasswords(string Email, string UserID, string newPassword)
        {
            if(db.Student_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault().ForgetPassword == true)
            {
                var std = db.Student_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault();
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(newPassword), std.Salt));
                std.ForgetPassword = false;
                std.Password = NewHashedPassword;
                db.SaveChanges();
                return RedirectToAction("Index", "Student");
            }
            else if(db.Lecturer_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault().ForgetPassword == true)
            {
                var lec = db.Lecturer_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault();
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(newPassword), lec.Salt));
                lec.Password = NewHashedPassword;
                lec.ForgetPassword = false;
                db.SaveChanges();
                return RedirectToAction("Index", "Lecturer");
            }
            else if(db.Admin_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault().ForgetPassword == true)
            {
                var ad = db.Admin_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault();
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(newPassword), ad.Salt));
                ad.Password = NewHashedPassword;
                ad.ForgetPassword = false;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View();
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
