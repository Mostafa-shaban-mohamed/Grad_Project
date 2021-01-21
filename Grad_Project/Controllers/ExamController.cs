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
        public ActionResult Index()
        {
            var exam_tbl = db.Exam_tbl.Include(e => e.Course_tbl).Include(e => e.Question_tbl).Include(e => e.Question_tbl1).Include(e => e.Question_tbl2).Include(e => e.Question_tbl3).Include(e => e.Question_tbl4).Include(e => e.Question_tbl5).Include(e => e.Question_tbl6).Include(e => e.Question_tbl7).Include(e => e.Question_tbl8).Include(e => e.Question_tbl9);
            return View(exam_tbl.ToList());
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
            var res = db.Result_tbl.FirstOrDefault(m => m.StudentID == st.ID && m.Exam_ID == id);
            if(res != null)
            {
                return RedirectToAction("Index", "Result");
            }
            Session["ExamID"] = id;
            return View(exam_tbl);
        }

        [HttpPost]
        public ActionResult Details(string Code, string StudentAnsQ_1, string StudentAnsQ_2, string StudentAnsQ_3, string StudentAnsQ_4, string StudentAnsQ_5,
            string StudentAnsQ_6, string StudentAnsQ_7, string StudentAnsQ_8, string StudentAnsQ_9, string StudentAnsQ_10)
        {
            var exam = db.Exam_tbl.Find(Session["ExamID"].ToString());
            if (exam == null)
            {
                return HttpNotFound();
            }
            var result = new Result_tbl();
            result.Exam_ID = Session["ExamID"].ToString();
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
            result.Title = exam.Type;

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
                if(StudentAnsQ_1 == exam.Question_tbl.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl.Total_Mark;
                }
                if (StudentAnsQ_2 == exam.Question_tbl1.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl1.Total_Mark;
                }
                if (StudentAnsQ_3 == exam.Question_tbl2.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl2.Total_Mark;
                }
                if (StudentAnsQ_4 == exam.Question_tbl3.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl3.Total_Mark;
                }
                if (StudentAnsQ_5 == exam.Question_tbl4.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl4.Total_Mark;
                }
                
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
                if (StudentAnsQ_1 == exam.Question_tbl.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl.Total_Mark;
                }
                if (StudentAnsQ_2 == exam.Question_tbl1.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl1.Total_Mark;
                }
                if (StudentAnsQ_3 == exam.Question_tbl2.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl2.Total_Mark;
                }
                if (StudentAnsQ_4 == exam.Question_tbl3.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl3.Total_Mark;
                }
                if (StudentAnsQ_5 == exam.Question_tbl4.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl4.Total_Mark;
                }
                if (StudentAnsQ_6 == exam.Question_tbl5.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl5.Total_Mark;
                }
                if (StudentAnsQ_7 == exam.Question_tbl6.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl6.Total_Mark;
                }
                if (StudentAnsQ_8 == exam.Question_tbl7.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl7.Total_Mark;
                }
                if (StudentAnsQ_9 == exam.Question_tbl8.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl8.Total_Mark;
                }
                if (StudentAnsQ_10 == exam.Question_tbl9.Correct_Ch)
                {
                    result.Achieved_Mark += exam.Question_tbl9.Total_Mark;
                }
            }
            db.Result_tbl.Add(result);
            db.SaveChanges();
            return RedirectToAction("Index", "Result");
        }

        // GET: Exam/Create
        public ActionResult Create()
        {
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
            ViewBag.Course_ID = new SelectList(db.Course_tbl, "ID", "Name");
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
                if(exam_tbl.Type == "Quiz")
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

                db.Exam_tbl.Add(exam_tbl);
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
            ViewBag.Course_ID = new SelectList(db.Course_tbl, "ID", "Name");
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
