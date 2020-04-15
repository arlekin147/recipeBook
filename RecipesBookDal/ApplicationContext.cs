using Microsoft.EntityFrameworkCore;
using RecipesBookDomain.Models;

namespace RecipesBookDal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingridient> Ingridients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngridient>()
                .HasKey(t => new { t.RecipeId, t.IngridientId });

            modelBuilder.Entity<RecipeIngridient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(i => i.RecipeIngridients)
                .HasForeignKey(ri => ri.IngridientId);

            modelBuilder.Entity<RecipeIngridient>()
                .HasOne(ri => ri.Ingridient)
                .WithMany(r => r.RecipeIngridients)
                .HasForeignKey(ri => ri.RecipeId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;MultipleActiveResultSets=true;"); //TODO Add dbSettings from appconfig
        }
    }
}
