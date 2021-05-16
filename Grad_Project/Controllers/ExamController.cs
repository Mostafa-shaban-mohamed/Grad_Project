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
    public class ExamController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Exam
        [Authorize(Roles = "Student, Lecturer")]
        public ActionResult Index()
        {
            var st = db.Student_tbl.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            var reg = db.RegisteredCourses_tbl.Find(st.ID);

            var exam_tbl = db.Exam_tbl.Include(e => e.Course_tbl).Include(e => e.Question_tbl).Include(e => e.Question_tbl1).Include(e => e.Question_tbl2).Include(e => e.Question_tbl3).Include(e => e.Question_tbl4).Include(e => e.Question_tbl5).Include(e => e.Question_tbl6).Include(e => e.Question_tbl7).Include(e => e.Question_tbl8).Include(e => e.Question_tbl9).ToList();
            
            return View(exam_tbl);
        }

        // GET: Exam/Details/5
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam_tbl exam_tbl = db.Exam_tbl.Find(id);
            if (exam_tbl == null)
            {
                return HttpNotFound();
            }
            var st = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
            //check if student registered in this course of exam
            var reg = db.RegisteredCourses_tbl.Find(st.ID);
            bool isregistered = false;
            if(reg.Course01 == id || reg.Course02 == id || reg.Course03 == id || reg.Course04 == id || reg.Course05 == id || reg.Course06 == id
                || reg.Course07 == id)
            {
                isregistered = true;
            }
            var pretended_ans_ID = st.ID + id;
            var ans = db.Answer_tbl.FirstOrDefault(m => m.Ans_ID == pretended_ans_ID);
            var res = db.Result_tbl.FirstOrDefault(m => m.StudentID == st.ID && m.Exam_ID == id);
            //calculate the difference in time
            //TimeSpan difftm = exam_tbl.AvailabilityTime.Value - DateTime.Now;
            int diff = DateTime.Compare(exam_tbl.AvailabilityTime.Value, DateTime.Now);
            if(diff < 0 && ans == null && isregistered)
            {
                //create res and ans paper and leave it blank
                var result = new Result_tbl();
                result.Exam_ID = id;
                result.ID = "R0" + db.Result_tbl.Count().ToString();
                result.StudentID = st.ID;
                result.CourseID = exam_tbl.Course_ID;
                result.Title = exam_tbl.Type;
                //create answer paper
                var Ans = new Answer_tbl()
                {
                    Ans_ID = st.ID + id,
                    Exam_ID = id,
                    Stu_Code = st.ID
                };

                Ans.Ans_1 = string.Empty;
                Ans.Ans_2 = string.Empty;
                Ans.Ans_3 = string.Empty;
                Ans.Ans_4 = string.Empty;
                Ans.Ans_5 = string.Empty;

                if (exam_tbl.Type != "Quiz")
                {
                    Ans.Ans_6 = string.Empty;
                    Ans.Ans_7 = string.Empty;
                    Ans.Ans_8 = string.Empty;
                    Ans.Ans_9 = string.Empty;
                    Ans.Ans_10 = string.Empty;
                }
                db.Result_tbl.Add(result);
                db.Answer_tbl.Add(Ans);
                db.SaveChanges();
                return RedirectToAction("Index", "Result");
            }
            if(ans != null || res != null || isregistered == false)
            {
                return RedirectToAction("Index", "Result");
            }
            return View(exam_tbl);
        }

        [HttpPost]
        public ActionResult Details(string id, string Code, string StudentAnsQ_1, string StudentAnsQ_2, string StudentAnsQ_3, string StudentAnsQ_4, string StudentAnsQ_5,
            string StudentAnsQ_6, string StudentAnsQ_7, string StudentAnsQ_8, string StudentAnsQ_9, string StudentAnsQ_10)
        {
            var exam = db.Exam_tbl.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            var result = new Result_tbl();
            result.Exam_ID = id;
            result.ID = "R0" + db.Result_tbl.Count().ToString();
            var st = db.Student_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);

            if(st != null)
            {
                result.StudentID = st.ID;
            }
            else
            {
                result.StudentID = Code;
            }
            
            result.CourseID = exam.Course_ID;
            //change result title in future
            result.Title = exam.Type;
            //create answer paper
            var ans = new Answer_tbl()
            {
                Ans_ID = st.ID + exam.Exam_ID,
                Exam_ID = exam.Exam_ID,
                Stu_Code = st.ID
            };

            if(exam.Type == "Quiz")
            {
                //Total mark of exam
                result.Total_Mark = exam.Question_tbl.Total_Mark;
                result.Total_Mark += exam.Question_tbl1.Total_Mark;
                result.Total_Mark += exam.Question_tbl2.Total_Mark;
                result.Total_Mark += exam.Question_tbl3.Total_Mark;
                result.Total_Mark += exam.Question_tbl4.Total_Mark;
                //Achieved mark of exam
                result.Achieved_Mark = 0;
                
                ans.Ans_1 = StudentAnsQ_1;
                ans.Ans_2 = StudentAnsQ_2;
                ans.Ans_3 = StudentAnsQ_3;
                ans.Ans_4 = StudentAnsQ_4;
                ans.Ans_5 = StudentAnsQ_5;
            }
            else
            {
                //Total mark of exam
                result.Total_Mark = exam.Question_tbl.Total_Mark;
                result.Total_Mark += exam.Question_tbl1.Total_Mark;
                result.Total_Mark += exam.Question_tbl2.Total_Mark;
                result.Total_Mark += exam.Question_tbl3.Total_Mark;
                result.Total_Mark += exam.Question_tbl4.Total_Mark;
                result.Total_Mark += exam.Question_tbl5.Total_Mark;
                result.Total_Mark += exam.Question_tbl6.Total_Mark;
                result.Total_Mark += exam.Question_tbl7.Total_Mark;
                result.Total_Mark += exam.Question_tbl8.Total_Mark;
                result.Total_Mark += exam.Question_tbl9.Total_Mark;
                //Achieved mark of exam
                result.Achieved_Mark = 0;
                
                ans.Ans_1 = StudentAnsQ_1;
                ans.Ans_2 = StudentAnsQ_2;
                ans.Ans_3 = StudentAnsQ_3;
                ans.Ans_4 = StudentAnsQ_4;
                ans.Ans_5 = StudentAnsQ_5;
                ans.Ans_6 = StudentAnsQ_6;
                ans.Ans_7 = StudentAnsQ_7;
                ans.Ans_8 = StudentAnsQ_8;
                ans.Ans_9 = StudentAnsQ_9;
                ans.Ans_10 = StudentAnsQ_10;
            }
            db.Result_tbl.Add(result);
            db.Answer_tbl.Add(ans);
            db.SaveChanges();
            return RedirectToAction("Index", "Result");
        }

        //AutoComplete mechanism for Questions
        public JsonResult Search(string term)
        {
            List<string> Loc = db.Question_tbl.Where(x => x.Ques_Title.Contains(term)).Select(x => x.Ques_Title).ToList();
            return Json(Loc, JsonRequestBehavior.AllowGet);
        }

        // GET: Exam/Create
        [Authorize(Roles = "Lecturer")]
        public ActionResult Create()
        {
            var lec = db.Lecturer_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
            if(lec == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Q01 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q02 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q03 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q04 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q05 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q06 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q07 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q08 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q09 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Q10 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title");
            ViewBag.Course_ID = new SelectList(db.Course_tbl.Where(m => m.Prof == lec.ID || m.Assistant == lec.ID), "ID", "Name");
            return View();
        }

        // POST: Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam_tbl exam_tbl)
        {
            if (ModelState.IsValid)
            {
                var question = new List<Question_tbl>();

                question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q01).FirstOrDefault());
                exam_tbl.Q01 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q01).FirstOrDefault().Q_ID;
                question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q02).FirstOrDefault());
                exam_tbl.Q02 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q02).FirstOrDefault().Q_ID;
                question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q03).FirstOrDefault());
                exam_tbl.Q03 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q03).FirstOrDefault().Q_ID;
                question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q04).FirstOrDefault());
                exam_tbl.Q04 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q04).FirstOrDefault().Q_ID;
                question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q05).FirstOrDefault());
                exam_tbl.Q05 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q05).FirstOrDefault().Q_ID;

                if (exam_tbl.Type == "Quiz")
                {
                    exam_tbl.Q06 = null; exam_tbl.Q07 = null; exam_tbl.Q08 = null; exam_tbl.Q09 = null;
                    exam_tbl.Q10 = null;
                }
                else
                {
                    question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q06).FirstOrDefault());
                    exam_tbl.Q06 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q06).FirstOrDefault().Q_ID;
                    question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q07).FirstOrDefault());
                    exam_tbl.Q07 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q07).FirstOrDefault().Q_ID;
                    question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q08).FirstOrDefault());
                    exam_tbl.Q08 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q08).FirstOrDefault().Q_ID;
                    question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q09).FirstOrDefault());
                    exam_tbl.Q09 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q09).FirstOrDefault().Q_ID;
                    question.Add(db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q10).FirstOrDefault());
                    exam_tbl.Q10 = db.Question_tbl.Where(q => q.Ques_Title == exam_tbl.Q10).FirstOrDefault().Q_ID;
                }
                // Adding total mark of each question to the exam
                exam_tbl.Total_Mark = 0;
                for (int i = 0; i < question.Count; i++)
                {
                    exam_tbl.Total_Mark += question[i].Total_Mark;
                }
                exam_tbl.ReleaseTime = DateTime.Now;
                db.Exam_tbl.Add(exam_tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var lec = db.Lecturer_tbl.FirstOrDefault(m => m.Email == User.Identity.Name);
            if (lec == null)
            {
                return HttpNotFound();
            }

            ViewBag.Q01 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q01);
            ViewBag.Q02 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q02);
            ViewBag.Q03 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q03);
            ViewBag.Q04 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q04);
            ViewBag.Q05 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q05);
            ViewBag.Q06 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q06);
            ViewBag.Q07 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q07);
            ViewBag.Q08 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q08);
            ViewBag.Q09 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q09);
            ViewBag.Q10 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q10);
            ViewBag.Course_ID = new SelectList(db.Course_tbl.Where(m => m.Prof == lec.ID || m.Assistant == lec.ID), "ID", "Name");
            return View(exam_tbl);
        }

        // GET: Exam/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam_tbl exam_tbl = db.Exam_tbl.Find(id);
            if (exam_tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.Q01 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q01);
            ViewBag.Q02 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q02);
            ViewBag.Q03 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q03);
            ViewBag.Q04 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q04);
            ViewBag.Q05 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q05);
            ViewBag.Q06 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q06);
            ViewBag.Q07 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q07);
            ViewBag.Q08 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q08);
            ViewBag.Q09 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q09);
            ViewBag.Q10 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q10);
            ViewBag.Course_ID = new SelectList(db.Course_tbl, "ID", "Name", exam_tbl.Course_ID);
            return View(exam_tbl);
        }

        // POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Exam_tbl exam_tbl)
        {
            if (ModelState.IsValid)
            {
                var question = new List<Question_tbl>();
                if (exam_tbl.Type == "Quiz")
                {
                    exam_tbl.Q06 = null; exam_tbl.Q07 = null; exam_tbl.Q08 = null; exam_tbl.Q09 = null;
                    exam_tbl.Q10 = null;

                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q01).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q02).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q03).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q04).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q05).FirstOrDefault());

                }
                else
                {
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q01).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q02).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q03).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q04).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q05).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q06).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q07).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q08).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q09).FirstOrDefault());
                    question.Add(db.Question_tbl.Where(q => q.Q_ID == exam_tbl.Q10).FirstOrDefault());


                }
                // Adding total mark of each question to the exam
                exam_tbl.Total_Mark = 0;
                for (int i = 0; i < question.Count; i++)
                {
                    exam_tbl.Total_Mark += question[i].Total_Mark;
                }

                db.Entry(exam_tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Q01 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q01);
            ViewBag.Q02 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q02);
            ViewBag.Q03 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q03);
            ViewBag.Q04 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q04);
            ViewBag.Q05 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q05);
            ViewBag.Q06 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q06);
            ViewBag.Q07 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q07);
            ViewBag.Q08 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q08);
            ViewBag.Q09 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q09);
            ViewBag.Q10 = new SelectList(db.Question_tbl, "Q_ID", "Ques_Title", exam_tbl.Q10);
            ViewBag.Course_ID = new SelectList(db.Course_tbl, "ID", "Name", exam_tbl.Course_ID);
            return View(exam_tbl);
        }

        // GET: Exam/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam_tbl exam_tbl = db.Exam_tbl.Find(id);
            if (exam_tbl == null)
            {
                return HttpNotFound();
            }
            return View(exam_tbl);
        }

        // POST: Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Exam_tbl exam_tbl = db.Exam_tbl.Find(id);
            db.Exam_tbl.Remove(exam_tbl);
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
