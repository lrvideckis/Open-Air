using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAir.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.SystemTest
{
    /*
     * Singleton pattern for the testing db context. Don't need a bunch just 1. 
     */
    public sealed class SingletonTestSetup
    {
        private static SingletonTestSetup instance = null;
        private readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=OpenAirLocalTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        private OpenAirDbContext _context;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SingletonTestSetup() {}

        /*
         * Private constructor. Only Instance function can call it.
         */
        private SingletonTestSetup()
        {
            // setup the local db for the system testing
            var serviceProvider = new ServiceCollection()
                                .AddEntityFrameworkSqlServer()
                                .BuildServiceProvider();

            // setup builder and point to the local db
            var builder = new DbContextOptionsBuilder<OpenAirDbContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("OpenAir.Data"));
            _context = new OpenAirDbContext(builder.Options);
            _context.Database.Migrate();
        }

        /*
         * Getter of the context instance. 
         */
        public static SingletonTestSetup Instance()
        {       
            if (instance == null)
            {
                instance = new SingletonTestSetup();
            }
            return instance;
        }

        /*
         * Getter for the instance of OpenAirDbContext
         */
        public OpenAirDbContext Get()
        {
            return _context;
        }
    }
}
