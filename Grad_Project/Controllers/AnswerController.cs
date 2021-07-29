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
    [Authorize(Roles = "Lecturer")]
    public class AnswerController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: Answer
        
        public ActionResult Index(string search)
        {
            return View(db.Answer_tbl.Where(m => m.Exam_ID == search).ToList());
        }

        // GET: Answer/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer_tbl answer_tbl = db.Answer_tbl.Find(id);
            if (answer_tbl == null)
            {
                return HttpNotFound();
            }
            var ex = db.Exam_tbl.Find(answer_tbl.Exam_ID);
            var maxMark = new List<int>();   // store mark of each question in exam before assign it to viewbag

            maxMark.Add(ex.Question_tbl.Total_Mark.GetValueOrDefault());
            maxMark.Add(ex.Question_tbl1.Total_Mark.GetValueOrDefault());
            maxMark.Add(ex.Question_tbl2.Total_Mark.GetValueOrDefault());
            maxMark.Add(ex.Question_tbl3.Total_Mark.GetValueOrDefault());
            maxMark.Add(ex.Question_tbl4.Total_Mark.GetValueOrDefault());

            if (ex.Type != "Quiz")
            {
                maxMark.Add(ex.Question_tbl5.Total_Mark.GetValueOrDefault());
                maxMark.Add(ex.Question_tbl6.Total_Mark.GetValueOrDefault());
                maxMark.Add(ex.Question_tbl7.Total_Mark.GetValueOrDefault());
                maxMark.Add(ex.Question_tbl8.Total_Mark.GetValueOrDefault());
                maxMark.Add(ex.Question_tbl9.Total_Mark.GetValueOrDefault());
            }
            ViewBag.mm = maxMark;
            return View(answer_tbl);
        }
        [HttpPost]
        public ActionResult Details(string id, string mark01, string mark02, string mark03, string mark04, string mark05, string mark06,
             string mark07, string mark08, string mark09, string mark10)
        {
            var ans = db.Answer_tbl.Find(id);
            var st = db.Student_tbl.Find(ans.Stu_Code);
            
            if(st != null)
            {
                var res = db.Result_tbl.Where(m => m.StudentID == st.ID && m.Exam_ID == ans.Exam_ID).FirstOrDefault();
                if(ans.Ans_6 == null && ans.Ans_7 == null)
                {
                    res.Achieved_Mark = int.Parse(mark01) + int.Parse(mark02) + int.Parse(mark03) + int.Parse(mark04) + int.Parse(mark05);
                }else
                {
                    res.Achieved_Mark = int.Parse(mark01) + int.Parse(mark02) + int.Parse(mark03) + int.Parse(mark04)
                    + int.Parse(mark05) + int.Parse(mark06) + int.Parse(mark07) + int.Parse(mark08) + int.Parse(mark09) + int.Parse(mark10);
                }
                db.Entry(res).State = EntityState.Modified;
                db.SaveChanges();
            }
            //Add notification
            return RedirectToAction("Create", "Notification", new
            {
                mthd = "Index",
                cntlr = "Result",
                course_id = ans.Exam_ID,
                subject = "New Result is added for Course ",
                role_not = "Student"
            });
            //return RedirectToAction("Index", "Result");
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
