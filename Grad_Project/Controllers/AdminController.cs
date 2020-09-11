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

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Admin_tbl.ToList());
        }

        // GET: Admin/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,Password,Salt")] Admin_tbl admin_tbl)
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
