using Microsoft.EntityFrameworkCore;

namespace CsharpBlackBelt.Models
{
    public class ActivityContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ActivityContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activitys { get; set; }
        public DbSet<Attandence> Attandences { get; set; }


    }
}