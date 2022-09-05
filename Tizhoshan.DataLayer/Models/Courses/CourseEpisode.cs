using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tizhoshan.DataLayer.Models.Courses
{
    public class CourseEpisode
    {

        [Key]
        public int EpisodeId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;


        public string EpisodeTitle { get; set; }
        public string Description { get; set; }
        public TimeSpan EpisodeTime { get; set; }

        public string EpisodeFileName { get; set; }

        public bool IsFree { get; set; }


        #region rel

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        #endregion


    }
}
