//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Grad_Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Result_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Result_tbl()
        {
            this.Student_tbl1 = new HashSet<Student_tbl>();
        }
    
        public string ID { get; set; }
        public string CourseID { get; set; }
        public string StudentID { get; set; }
        public Nullable<int> Total_Mark { get; set; }
        public Nullable<int> Achieved_Mark { get; set; }
        public string Title { get; set; }
        public string Exam_ID { get; set; }
    
        public virtual Course_tbl Course_tbl { get; set; }
        public virtual Student_tbl Student_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_tbl> Student_tbl1 { get; set; }
        public virtual Exam_tbl Exam_tbl { get; set; }
    }
}
