using Microsoft.EntityFrameworkCore;
using OpenAir.Data.Models;
using System.Diagnostics.Contracts;

namespace OpenAir.Data
{
    public class OpenAirDbContext : DbContext
    {
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=OpenAirLocalDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        public DbSet<Pilot> Pilots { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<RoutePoint> RoutePoints { get; set; }

        public OpenAirDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public OpenAirDbContext(DbContextOptions<OpenAirDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // use the connection string to find the database, then put the migration assembly folder into OpenAir.Data
                // These are from the appsettings.json
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("OpenAir.Data"));
            }
        }
        /*
         * This method defines the database model so the database can be created by EntityFrameworkCore.
         * Inside the map classes are details on specific entity attributes.
         * The modelBuilder.Entity<Class>().HasData calls seed the database with specific data upon the database's
         * creation.
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // setup the map references so ef knows how to specifically constrain attributes
            Contract.Requires(modelBuilder != null);
            base.OnModelCreating(modelBuilder);
            _ = new PilotMap(modelBuilder.Entity<Pilot>());
            _ = new RouteMap(modelBuilder.Entity<Route>());
            _ = new FlightMap(modelBuilder.Entity<Flight>());
            _ = new PointMap(modelBuilder.Entity<Point>());
            _ = new MultiplicityMap(modelBuilder);

            ////
            // Seed the Routes table
            ////
            modelBuilder.Entity<Route>().HasData(new Route { RouteId = "VR-140" });
        }

    }
}
