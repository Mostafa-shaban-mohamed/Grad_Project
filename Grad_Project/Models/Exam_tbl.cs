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
    
    public partial class Exam_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam_tbl()
        {
            this.Result_tbl = new HashSet<Result_tbl>();
        }
    
        public string Exam_ID { get; set; }
        public string Course_ID { get; set; }
        public string Type { get; set; }
        public string Q01 { get; set; }
        public string Q02 { get; set; }
        public string Q03 { get; set; }
        public string Q04 { get; set; }
        public string Q05 { get; set; }
        public string Q06 { get; set; }
        public string Q07 { get; set; }
        public string Q08 { get; set; }
        public string Q09 { get; set; }
        public string Q10 { get; set; }
        public Nullable<int> Correct_Answers { get; set; }
        public Nullable<int> Total_Mark { get; set; }
        public Nullable<int> Achieved_Mark { get; set; }
        public Nullable<System.DateTime> ReleaseTime { get; set; }
        public Nullable<System.DateTime> AvailabilityTime { get; set; }
    
        public virtual Question_tbl Question_tbl { get; set; }
        public virtual Question_tbl Question_tbl1 { get; set; }
        public virtual Question_tbl Question_tbl2 { get; set; }
        public virtual Question_tbl Question_tbl3 { get; set; }
        public virtual Question_tbl Question_tbl4 { get; set; }
        public virtual Question_tbl Question_tbl5 { get; set; }
        public virtual Question_tbl Question_tbl6 { get; set; }
        public virtual Question_tbl Question_tbl7 { get; set; }
        public virtual Question_tbl Question_tbl8 { get; set; }
        public virtual Question_tbl Question_tbl9 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result_tbl> Result_tbl { get; set; }
        public virtual Course_tbl Course_tbl { get; set; }
    }
}
