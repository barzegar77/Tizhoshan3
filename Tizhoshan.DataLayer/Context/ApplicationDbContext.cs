using Microsoft.EntityFrameworkCore;
using Tizhoshan.DataLayer.Models.Account;
using Tizhoshan.DataLayer.Models.Courses;

namespace Tizhoshan.DataLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option){}


        #region user

        public DbSet<User> Users { get; set; }

        #endregion




        #region course

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        #endregion


    }
}
