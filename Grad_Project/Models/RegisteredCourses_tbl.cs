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
    using Grad_Project.Models;
    using System.Linq;

    public partial class RegisteredCourses_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegisteredCourses_tbl()
        {
            this.Student_tbl = new HashSet<Student_tbl>();
        }
    
        public string ID { get; set; }
        public string Course01 { get; set; }
        public string Course02 { get; set; }
        public string Course03 { get; set; }
        public string Course04 { get; set; }
        public string Course05 { get; set; }
        public string Course06 { get; set; }
        public string Course07 { get; set; }

        public virtual Course_tbl Course_tbl { get; set; }
        public virtual Course_tbl Course_tbl1 { get; set; }
        public virtual Course_tbl Course_tbl2 { get; set; }
        public virtual Course_tbl Course_tbl3 { get; set; }
        public virtual Course_tbl Course_tbl4 { get; set; }
        public virtual Course_tbl Course_tbl5 { get; set; }
        public virtual Course_tbl Course_tbl6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student_tbl> Student_tbl { get; set; }
        
    }
}
