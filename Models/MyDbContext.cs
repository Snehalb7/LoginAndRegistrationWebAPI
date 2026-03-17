using Microsoft.EntityFrameworkCore;

namespace LoginAndRegistrationWebAPI.Models
{
    public class MyDbContext:DbContext
    {
        //Constructor to initialize the DbContext with options
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    
    }
}
