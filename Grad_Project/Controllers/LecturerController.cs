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

namespace Grad_Project.Controllers
{
    public class LecturerController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();
        private static string str;


        // GET: Lecturer
        public ActionResult Index()
        {
            return View(db.Lecturer_tbl.ToList());
        }

        // GET: Lecturer/Details/5
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
                str = lecturer_tbl.ID;
                db.Lecturer_tbl.Add(lecturer_tbl);
                db.SaveChanges();
                return RedirectToAction("UploadImage");
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
                    var lec = db.Lecturer_tbl.Find(str);
                    lec.Image = bytes;
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


        // GET: Lecturer/Edit/5
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
