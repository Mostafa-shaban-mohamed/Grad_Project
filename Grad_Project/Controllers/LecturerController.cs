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
using PagedList;

namespace Grad_Project.Controllers
{
    public class LecturerController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Lecturer
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string Search, int? Page_No)
        {
            int Size_Of_Page = 6;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                return View(db.Lecturer_tbl.Where(m => m.Name.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }

            return View(db.Lecturer_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Lecturer/Details/5
        [Authorize(Roles = "Lecturer")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer_tbl lecturer_tbl = db.Lecturer_tbl.Find(id);
            if (lecturer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(lecturer_tbl);
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

        [Authorize(Roles = "Admin")]
        // GET: Lecturer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lecturer_tbl lecturer_tbl)
        {
            var salt = GenerateSalt();
            if (ModelState.IsValid)
            {
                lecturer_tbl.Password = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(lecturer_tbl.Password), salt));
                lecturer_tbl.Salt = salt;
                db.Lecturer_tbl.Add(lecturer_tbl);
                db.SaveChanges();
                return RedirectToAction("UploadImage", "Lecturer", new { id = lecturer_tbl.ID});
            }

            return View(lecturer_tbl);
        }

        //Change Password ------------------------------
        [HttpGet]
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer_tbl lec = db.Lecturer_tbl.Find(id);
            if (lec == null)
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
            var lec = db.Lecturer_tbl.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            //check if admin is null
            if (lec == null)
            {
                return HttpNotFound();
            }
            //hashing old taken password
            var oldHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.OldPassword), lec.Salt));
            //Check if oldpassowrd is the user password
            if (oldHashedPassword == lec.Password && !string.IsNullOrWhiteSpace(CPVM.NewPassword))
            {
                var NewHashedPassword = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(CPVM.NewPassword), lec.Salt));
                lec.Password = NewHashedPassword;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = lec.ID });
            }

            return View();
        }

        [HttpGet]
        public ActionResult UploadImage(string id)
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
            var art = db.Lecturer_tbl.Find(id);
            art.Image = uploadedFile;
            db.Entry(art).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Details", new { id = id });
        }


        // GET: Lecturer/Edit/5
        [Authorize(Roles = "Lecturer")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer_tbl lecturer_tbl = db.Lecturer_tbl.Find(id);
            if (lecturer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(lecturer_tbl);
        }

        // POST: Lecturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Password,Salt,Role,Image")] Lecturer_tbl lecturer_tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecturer_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lecturer_tbl);
        }

        // GET: Lecturer/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer_tbl lecturer_tbl = db.Lecturer_tbl.Find(id);
            if (lecturer_tbl == null)
            {
                return HttpNotFound();
            }
            return View(lecturer_tbl);
        }

        // POST: Lecturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Lecturer_tbl lecturer_tbl = db.Lecturer_tbl.Find(id);
            db.Lecturer_tbl.Remove(lecturer_tbl);
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
