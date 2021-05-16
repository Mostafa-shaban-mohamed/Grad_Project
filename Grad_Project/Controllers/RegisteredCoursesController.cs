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
    public class RegisteredCoursesController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: RegisteredCourses
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string Search, int? Page_No)
        {
            var registeredCourses_tbl = db.RegisteredCourses_tbl.Include(r => r.Course_tbl).Include(r => r.Course_tbl1).Include(r => r.Course_tbl2).Include(r => r.Course_tbl3).Include(r => r.Course_tbl4).Include(r => r.Course_tbl5).Include(r => r.Course_tbl6);
            int Size_Of_Page = 2;
            int No_Of_Page = (Page_No ?? 1);
            if (!string.IsNullOrEmpty(Search))
            {
                return View(registeredCourses_tbl.Where(m => m.ID.Contains(Search)).ToList().ToPagedList(No_Of_Page, Size_Of_Page));
            }

            return View(registeredCourses_tbl.ToList().ToPagedList(No_Of_Page, Size_Of_Page));
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

        //AutoComplete mechanism for students code
        public JsonResult Search(string term)
        {
            List<string> Loc = db.Course_tbl.Where(x => x.Name.Contains(term)).Select(x => x.Name).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }


        // GET: RegisteredCourses/Create
        [Authorize(Roles = "Student")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisteredCourses/Create
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
                CreateAttendance(registeredCourses_tbl);
                return RedirectToAction("Index");
            }

            return View(registeredCourses_tbl);
        }

        //Automatic creation of attendance of student-course relation
        [HttpPost]
        public void CreateAttendance(RegisteredCourses_tbl reg)
        {
            if (reg.Course01 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl() {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course01,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course02 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course02,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course03 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course03,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course04 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course04,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course05 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course05,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course06 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course06,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (reg.Course07 != null)
            {
                db.Attendance_tbl.Add(new Attendance_tbl()
                {
                    ID = "Att" + db.Attendance_tbl.Count().ToString(),
                    StudentID = reg.ID,
                    CourseID = reg.Course07,
                    No_of_Attendances = 0
                });
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
        }

        // GET: RegisteredCourses/Edit/5
        [Authorize(Roles = "Student, Admin")]
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
                EditAttendance(registeredCourses_tbl);
                return RedirectToAction("Details", "Student", new { id = registeredCourses_tbl.ID });
            }

            return View(registeredCourses_tbl);
        }

        //----
        [HttpPost]
        public void EditAttendance(RegisteredCourses_tbl reg)
        {
            var atts = db.Attendance_tbl.Where(m => m.StudentID == reg.ID).ToList();
            if (reg.Course01 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course01);
                if(att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course01,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course02 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course02);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course02,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course03 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course03);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course03,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course04 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course04);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course04,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course05 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course05);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course05,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course06 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course06);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course06,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }
            if (reg.Course07 != null)
            {
                var att = atts.FirstOrDefault(m => m.CourseID == reg.Course07);
                if (att == null)
                {
                    db.Attendance_tbl.Add(new Attendance_tbl()
                    {
                        ID = "Att" + db.Attendance_tbl.Count().ToString(),
                        StudentID = reg.ID,
                        CourseID = reg.Course07,
                        No_of_Attendances = 0
                    });
                    db.SaveChanges();
                }
            }

            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
        }

        [Authorize(Roles = "Admin")]
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
            var std = db.Student_tbl.Find(id);
            std.Registered_Courses = null;
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
