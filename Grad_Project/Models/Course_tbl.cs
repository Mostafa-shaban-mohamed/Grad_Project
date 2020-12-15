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
    
    public partial class Course_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course_tbl()
        {
            this.Attendance_tbl = new HashSet<Attendance_tbl>();
            this.Event_tbl = new HashSet<Event_tbl>();
            this.RegisteredCourses_tbl = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl1 = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl2 = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl3 = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl4 = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl5 = new HashSet<RegisteredCourses_tbl>();
            this.RegisteredCourses_tbl6 = new HashSet<RegisteredCourses_tbl>();
            this.Result_tbl = new HashSet<Result_tbl>();
            this.File_tbl = new HashSet<File_tbl>();
        }
    
        public string ID { get; set; }
        public string Name { get; set; }
        public string Prof { get; set; }
        public string Assistant { get; set; }
        public string PDFs { get; set; }
        public string Links { get; set; }
        public Nullable<int> Ed_Level { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance_tbl> Attendance_tbl { get; set; }
        public virtual Lecturer_tbl Lecturer_tbl { get; set; }
        public virtual Lecturer_tbl Lecturer_tbl1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event_tbl> Event_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RegisteredCourses_tbl> RegisteredCourses_tbl6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result_tbl> Result_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File_tbl> File_tbl { get; set; }
    }
}
