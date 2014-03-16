using System.Data.Entity;

namespace DemoProject.Model
{
    public class DemoProjectContext : DbContext
    {
        static DemoProjectContext()
        {
            // Database.SetInitializer<DemoProjectContext>(null);

            // We want to recreate our database every time when application starts
            Database.SetInitializer<DemoProjectContext>(new DropCreateDatabaseAlways<DemoProjectContext>());
        }

        public DemoProjectContext()
            : base("DemoProjectContext")
        {
        }

        public DbSet<Metadata> Metadata { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
