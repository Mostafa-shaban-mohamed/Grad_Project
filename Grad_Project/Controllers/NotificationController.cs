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
    public class NotificationController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Notification
        [Authorize(Roles = "Student, Admin, Lecturer")]
        public ActionResult Index()
        {
            if (User.IsInRole("Student"))
            {
                return View(db.Notification_tbl.Where(m => m.Role == "Student").ToList());
            }
            else if (User.IsInRole("Admin"))
            {
                return View(db.Notification_tbl.Where(m => m.Role == "Admin").ToList());
            }
            else //Lecturers
            {
                return View(db.Notification_tbl.Where(m => m.Role == "Lecturer").ToList());
            }
        }
        
        // GET: Notification/Create
        public ActionResult Create(string mthd, string cntlr, string course_id, string subject, string role_not, Notification_tbl notification_tbl)
        {
            if (ModelState.IsValid)
            {
                string Not_id;
                do
                {
                    Random rnd = new Random();
                    Not_id = "Not" + (rnd.Next(100001)).ToString();
                } while (db.Notification_tbl.Find(Not_id) != null);

                notification_tbl.NotificationID = Not_id;
                notification_tbl.Description = subject + course_id;
                notification_tbl.Role = role_not;
                db.Notification_tbl.Add(notification_tbl);
                db.SaveChanges();
                return RedirectToAction(mthd, cntlr);
            }

            return null;
        }
        
        // GET: Notification/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification_tbl notification_tbl = db.Notification_tbl.Find(id);
            if (notification_tbl == null)
            {
                return HttpNotFound();
            }
            db.Notification_tbl.Remove(notification_tbl);
            db.SaveChanges();
            return RedirectToAction("Index");
            //return View(notification_tbl);
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
