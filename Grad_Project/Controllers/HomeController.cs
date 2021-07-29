using Grad_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Grad_Project.Controllers
{
    public class HomeController : Controller
    {
        private LMSDBEntities db = new LMSDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string Email, string UserID)
        {
            if(Email != null && UserID != null)
            {
                //check if user is student
                if(db.Student_tbl.Any(m => m.Email == Email && m.ID == UserID))
                {
                    //give admin authorization to reset the password
                    var std = db.Student_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault();
                    std.ForgetPassword = true;
                    db.SaveChanges();
                    //add notification to admin to reset password
                    return RedirectToAction("Create", "Notification", new
                    {
                        mthd = "Index",
                        cntlr = "Home",
                        course_id = UserID,
                        subject = "Recover account of "+Email+"&",
                        role_not = "Admin"
                    });
                }
                //check if user is lecturer
                else if(db.Lecturer_tbl.Any(m => m.Email == Email && m.ID == UserID))
                {
                    //give admin authorization to reset the password
                    db.Lecturer_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault().ForgetPassword = true;
                    db.SaveChanges();
                    //add notification to admin to reset password
                    return RedirectToAction("Create", "Notification", new
                    {
                        mthd = "Index",
                        cntlr = "Home",
                        course_id = UserID,
                        subject = Email + " Forgot the password and ID is ",
                        role_not = "Admin"
                    });
                }
                //check if user is admin
                else if (db.Admin_tbl.Any(m => m.Email == Email && m.ID == UserID))
                {
                    //give admin authorization to reset the password
                    db.Admin_tbl.Where(m => m.Email == Email && m.ID == UserID).FirstOrDefault().ForgetPassword = true;
                    db.SaveChanges();
                    //add notification to admin to reset password
                    return RedirectToAction("Create", "Notification", new
                    {
                        mthd = "Index",
                        cntlr = "Home",
                        course_id = UserID,
                        subject = Email + " Forgot the password and ID is ",
                        role_not = "Admin"
                    });
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            
            return View();
        }

        //Hashing methods ---------------------------------------------
        public static byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }

        [HttpPost]
        public ActionResult LogIn(UserModel model)
        {
            using (LMSDBEntities db = new LMSDBEntities())
            {
                Student_tbl std = db.Student_tbl.FirstOrDefault(st => st.Email == model.Email);
                Lecturer_tbl lec = db.Lecturer_tbl.FirstOrDefault(st => st.Email == model.Email);
                Admin_tbl ad = db.Admin_tbl.FirstOrDefault(st => st.Email == model.Email);

                //Confirmation booleans
                bool IsValidStudent = false; bool IsValidLecturer = false; bool IsValidAdmin = false;

                if (std != null)
                {
                    var pass_std = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(model.Password), std.Salt));
                    IsValidStudent = db.Student_tbl.Any(user => user.Email.ToLower() == model.Email.ToLower() && user.Password == pass_std);
                }
                else if(lec != null)
                {
                    var pass_lec = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(model.Password), lec.Salt));
                    IsValidLecturer = db.Lecturer_tbl.Any(user => user.Email.ToLower() == model.Email.ToLower() && user.Password == pass_lec);
                }
                else if(ad != null)
                {
                    var pass_ad = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(model.Password), ad.Salt));
                    IsValidAdmin = db.Admin_tbl.Any(user => user.Email.ToLower() == model.Email.ToLower() && user.Password == pass_ad);
                }
                //-----------------------------------------------------
                if (IsValidStudent)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    //Session["UserID"] = std.ID;
                    //Go to profile page
                    return RedirectToAction("Index", "Home");
                }

                if (IsValidLecturer)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    //Session["UserID"] = lec.ID;
                    //Go to profile page
                    return RedirectToAction("Index", "Home");
                }

                if (IsValidAdmin)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    //Session["UserID"] = ad.ID;
                    //Go to profile page
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "invalid Username or Password");
                return View("LogIn");
            }
        }
    }
}