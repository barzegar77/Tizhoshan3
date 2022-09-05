using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizhoshan.DataLayer.Models.Account;

namespace Tizhoshan.DataLayer.Models.Courses
{
    public class CourseComment
    {
        [Key]
        public int CommentId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;


        public string Text { get; set; }

        public bool IsAdminRead { get; set; } = false;

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public CourseComment Parent { get; set; }


        #region rel

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }



        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        #endregion
    }
}
