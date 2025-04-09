using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Movies.Model;

namespace Movie_Collection.Settings
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }


        public AppDbContext(IConfiguration configuration, DbContextOptions<AppDbContext> options) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MovieCollectionDb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Movies)
            .WithMany(m => m.Users);


        }

    }
}
