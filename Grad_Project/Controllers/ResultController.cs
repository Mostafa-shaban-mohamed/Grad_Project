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

namespace Grad_Project.Controllers
{
    public class ResultController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Result
        [Authorize(Roles = "Lecturer, Student")]
        public ActionResult Index(string Search, int? Page_No)
        {
            var result_tbl = db.Result_tbl.Include(r => r.Course_tbl).Include(r => r.Student_tbl);
            if (User.IsInRole("Student"))
            {
                var std = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
                result_tbl = db.Result_tbl.Where(m => m.StudentID == std.ID).Include(r => r.Course_tbl).Include(r => r.Student_tbl);
            }
            
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                return View(result_tbl.Where(m => m.Title.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }
            return View(result_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Result/Details/5
        //[Authorize(Roles = "Lecturer, Student")]
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Result_tbl result_tbl = db.Result_tbl.Find(id);
        //    if (result_tbl == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result_tbl);
        //}

        //// GET: Result/Create
        //[Authorize(Roles = "Lecturer")]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Result/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Result_tbl result_tbl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Result_tbl.Add(result_tbl);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(result_tbl);
        //}

        //// GET: Result/Edit/5
        //[Authorize(Roles = "Lecturer")]
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Result_tbl result_tbl = db.Result_tbl.Find(id);
        //    if (result_tbl == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result_tbl);
        //}

        //AutoComplete mechanism for student code
        public JsonResult SearchStd(string term)
        {
            List<string> Loc = db.Student_tbl.Where(x => x.ID.Contains(term)).Select(x => x.ID).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }

        //AutoComplete mechanism for course code
        public JsonResult SearchCourse(string term)
        {
            List<string> Loc = db.Course_tbl.Where(x => x.ID.Contains(term)).Select(x => x.ID).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }

        // POST: Result/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Result_tbl result_tbl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(result_tbl).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(result_tbl);
        //}

        //// GET: Result/Delete/5
        //[Authorize(Roles = "Lecturer")]
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Result_tbl result_tbl = db.Result_tbl.Find(id);
        //    if (result_tbl == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result_tbl);
        //}

        //// POST: Result/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Result_tbl result_tbl = db.Result_tbl.Find(id);
        //    db.Result_tbl.Remove(result_tbl);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
