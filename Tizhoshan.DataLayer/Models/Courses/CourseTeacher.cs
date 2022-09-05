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
    public class CourseTeacher
    {
        [Key]
        public int TeacherId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public string Bio { get; set; }

        public string AdminNote { get; set; }


        #region rel

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        #endregion


    }
}
