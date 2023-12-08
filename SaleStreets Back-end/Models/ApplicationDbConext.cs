using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SaleStreets_Back_end.Models
{
    public class ApplicationDbConext : IdentityDbContext
    {

        public ApplicationDbConext
            (DbContextOptions<ApplicationDbConext> options):base(options)
        {
            
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Image> Images { get; set; }
    }
}
