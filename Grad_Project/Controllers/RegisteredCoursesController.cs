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
    public class RegisteredCoursesController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: RegisteredCourses
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var registeredCourses_tbl = db.RegisteredCourses_tbl.Include(r => r.Course_tbl).Include(r => r.Course_tbl1).Include(r => r.Course_tbl2).Include(r => r.Course_tbl3).Include(r => r.Course_tbl4).Include(r => r.Course_tbl5).Include(r => r.Course_tbl6);
            return View(registeredCourses_tbl.ToList());
        }

        // GET: RegisteredCourses/Details/5
        [Authorize(Roles = "Admin, Student")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }
            return View(registeredCourses_tbl);
        }

        //AutoComplete mechanism
        public JsonResult Search(string term)
        {
            List<string> Loc = db.Course_tbl.Where(x => x.Name.Contains(term)).Select(x => x.Name).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }


        // GET: RegisteredCourses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisteredCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisteredCourses_tbl registeredCourses_tbl)
        {
            if (ModelState.IsValid)
            {
                if(registeredCourses_tbl.Course01 != null)
                {
                    registeredCourses_tbl.Course01 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course01).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course02 != null)
                {
                    registeredCourses_tbl.Course02 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course02).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course03 != null)
                {
                    registeredCourses_tbl.Course03 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course03).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course04 != null)
                {
                    registeredCourses_tbl.Course04 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course04).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course05 != null)
                {
                    registeredCourses_tbl.Course05 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course05).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course06 != null)
                {
                    registeredCourses_tbl.Course06 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course06).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course07 != null)
                {
                    registeredCourses_tbl.Course07 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course07).FirstOrDefault().ID;
                }
                
                db.RegisteredCourses_tbl.Add(registeredCourses_tbl);
                var std = db.Student_tbl.Find(registeredCourses_tbl.ID);
                std.Registered_Courses = registeredCourses_tbl.ID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registeredCourses_tbl);
        }

        // GET: RegisteredCourses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);

            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }

            //Redo the code of courses to their names
            if (registeredCourses_tbl.Course01 != null)
            {
                registeredCourses_tbl.Course01 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course01).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course02 != null)
            {
                registeredCourses_tbl.Course02 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course02).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course03 != null)
            {
                registeredCourses_tbl.Course03 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course03).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course04 != null)
            {
                registeredCourses_tbl.Course04 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course04).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course05 != null)
            {
                registeredCourses_tbl.Course05 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course05).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course06 != null)
            {
                registeredCourses_tbl.Course06 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course06).FirstOrDefault().Name;
            }
            if (registeredCourses_tbl.Course07 != null)
            {
                registeredCourses_tbl.Course07 = db.Course_tbl.Where(m => m.ID == registeredCourses_tbl.Course07).FirstOrDefault().Name;
            }

            return View(registeredCourses_tbl);
        }

        // POST: RegisteredCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisteredCourses_tbl registeredCourses_tbl)
        {
            if (ModelState.IsValid)
            {
                if (registeredCourses_tbl.Course01 != null)
                {
                    registeredCourses_tbl.Course01 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course01).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course02 != null)
                {
                    registeredCourses_tbl.Course02 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course02).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course03 != null)
                {
                    registeredCourses_tbl.Course03 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course03).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course04 != null)
                {
                    registeredCourses_tbl.Course04 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course04).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course05 != null)
                {
                    registeredCourses_tbl.Course05 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course05).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course06 != null)
                {
                    registeredCourses_tbl.Course06 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course06).FirstOrDefault().ID;
                }
                if (registeredCourses_tbl.Course07 != null)
                {
                    registeredCourses_tbl.Course07 = db.Course_tbl.Where(m => m.Name == registeredCourses_tbl.Course07).FirstOrDefault().ID;
                }
                //------
                db.Entry(registeredCourses_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registeredCourses_tbl);
        }

        // GET: RegisteredCourses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            if (registeredCourses_tbl == null)
            {
                return HttpNotFound();
            }
            return View(registeredCourses_tbl);
        }

        // POST: RegisteredCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RegisteredCourses_tbl registeredCourses_tbl = db.RegisteredCourses_tbl.Find(id);
            db.RegisteredCourses_tbl.Remove(registeredCourses_tbl);
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
