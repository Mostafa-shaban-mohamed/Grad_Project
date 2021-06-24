using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Description;
using Grad_Project.Models;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;
using System.Web.Mvc;
using System.Runtime.Remoting.Contexts;

namespace Grad_Project.Controllers
{
    public class AttendanceWebAPIController : ApiController
    {
        private LMSDBEntities db = new LMSDBEntities();

        // GET: api/AttendanceWebAPI
        public string GetAttendance_tbl()
        {
            return db.Attendance_tbl.FirstOrDefault().ID;
        }

        // GET: api/AttendanceWebAPI/5
        [ResponseType(typeof(Attendance_tbl))]
        public IHttpActionResult GetAttendance_tbl(string id)
        {
            Attendance_tbl attendance_tbl = db.Attendance_tbl.Find(id);
            if (attendance_tbl == null)
            {
                return NotFound();
            }

            return Ok(attendance_tbl);
        }

        // PUT: api/AttendanceWebAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAttendance_tbl()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("This is the invaild part");
            }
            // =================================== Send Json data in request body ======================
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            //bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            //=================================== Save JSON data in file in server =====================
            string fname = "h:\\root\\home\\phicommsees-001\\www\\db\\jsonfiles\\path.json";
            System.IO.File.WriteAllText(fname, bodyText);
            //Save file on server
            //fname = @"D:\path.json";
            //postedFile.SaveAs(fname);

            var json = File.ReadAllText(fname);
            var Att_VM = JsonConvert.DeserializeObject<AttVM>(json);

            for (int i = 0; i < Att_VM.StudentID.Length; i++)
            {
                var std_ID = Att_VM.StudentID[i];
                var attendance_tbl = db.Attendance_tbl.Where(s => s.CourseID == Att_VM.CourseID && s.StudentID == std_ID).FirstOrDefault();
                if (attendance_tbl != null)
                {
                    attendance_tbl.No_of_Attendances += 1;
                    db.Entry(attendance_tbl).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            //========================== JSON File ====================================================
            //if (HttpContext.Current.Request.Files.Count == 0)
            //    throw new HttpResponseException(new HttpResponseMessage()
            //    {
            //        ReasonPhrase = "Files are required",
            //        StatusCode = HttpStatusCode.BadRequest
            //    });

            //foreach (string file in HttpContext.Current.Request.Files)
            //{
            //    string fname;
            //    var postedFile = HttpContext.Current.Request.Files[file];
            //    fname = postedFile.FileName;
            //    if (!(postedFile.ContentType == "application/json" || postedFile.ContentType == "application/xml"))
            //    {
            //        throw new System.Web.Http.HttpResponseException(new HttpResponseMessage()
            //        {
            //            ReasonPhrase = "Wrong content type",
            //            StatusCode = HttpStatusCode.BadRequest
            //        });
            //    }
            //    // =================================== JSON File ===========================================
                
            //    //Save file on server
            //    fname = "h:\\root\\home\\phicommsees-001\\www\\db\\jsonfiles\\" + fname;
            //    postedFile.SaveAs(fname);

            //    var json = File.ReadAllText(fname);
            //    var Att_VM = JsonConvert.DeserializeObject<AttVM>(json);

            //    for(int i = 0; i < Att_VM.StudentID.Length; i++)
            //    {
            //        var std_ID = Att_VM.StudentID[i];
            //        var attendance_tbl = db.Attendance_tbl.Where(s => s.CourseID == Att_VM.CourseID && s.StudentID == std_ID).FirstOrDefault();
            //        if(attendance_tbl != null)
            //        {
            //            attendance_tbl.No_of_Attendances += 1;
            //            db.Entry(attendance_tbl).State = EntityState.Modified;
            //            db.SaveChanges();
            //        }
                    
            //    }
            //}

            return StatusCode(HttpStatusCode.Accepted);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Attendance_tblExists(string id)
        {
            return db.Attendance_tbl.Count(e => e.ID == id) > 0;
        }
    }
}