using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tizhoshan.DataLayer.Models.Courses
{
    public class CourseCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string CategoryTitle { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Icon { get; set; }


        public int? ParentId { get; set; } 
        [ForeignKey("ParentId")]
        public List<CourseCategory> CourseCategories { get; set; }


        #region rel
        public IEnumerable<Course> Courses { get; set; }
        #endregion
    }
}
