using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.DataLayer.Models.Courses
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string CoursePreviewName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public string CoursePictureName { get; set; }
        public string CoursePictureAlt { get; set; }
        public string CoursePictureTitle { get; set; }

        public int Price { get; set; } = 0;

        public int CourseStatus { get; set; }

        public string Keywords { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string CourseTime { get; set; }

        #region rel
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public CourseTeacher CourseTeacher { get; set; }



        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CourseCategory CourseCategory { get; set; }

        public IEnumerable<CourseEpisode> CourseEpisodes { get; set; }


        public IEnumerable<UserCourse> UserCourses { get; set; }


        public IEnumerable<CourseComment> CourseComments { get; set; }



        #endregion
    }
}
