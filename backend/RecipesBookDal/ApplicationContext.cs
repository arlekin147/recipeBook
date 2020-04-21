using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipesBookDomain.Configuration;
using RecipesBookDomain.Models;

namespace RecipesBookDal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingridient> Ingridients { get; set; }
        private SqlConfiguration _sqlConfiguration;

        public ApplicationContext(IOptions<SqlConfiguration> sqlConfiguration)
        {
            _sqlConfiguration = sqlConfiguration.Value;
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngridient>()
                .HasKey(t => new { t.RecipeId, t.IngridientId });

            modelBuilder.Entity<RecipeIngridient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(i => i.RecipeIngridients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngridient>()
                .HasOne(ri => ri.Ingridient)
                .WithMany(r => r.RecipeIngridients)
                .HasForeignKey(ri => ri.IngridientId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlConfiguration.ConnectionString); //TODO Add dbSettings from appconfig
        }
    }
}
