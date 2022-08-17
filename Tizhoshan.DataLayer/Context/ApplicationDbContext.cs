using Microsoft.EntityFrameworkCore;
using Tizhoshan.DataLayer.Models.Account;

namespace Tizhoshan.DataLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option){}

        public DbSet<User> Users { get; set; }


    }
}
