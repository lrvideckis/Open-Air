using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAir.SystemTest
{
    public class PilotRepositorySystemTest
    {
        // The database context for database manipulation
        private OpenAirDbContext _context;

        /*
         * Setup the needed resources for the test. This automatically is called before tests are run.
         */
        [SetUp]
        public void Setup()
        {          
            /// ARRANGE

            // ask system test setup class to get the context instance
            _context = SingletonTestSetup.Instance().Get();       
        }


        /*
         * The test method where assertions are enforced.
         */
        [Test]
        public async Task TestWhetherPilotGotAdded()
        {
            /// ARRANGE
            
            // setup PilotRepo to be tested
            var repo = new PilotRepo(_context);
            // setup pilot with name, email, aircraft and a password
            var pilot = CreatePilot("PilotAdded@USAF.mil");

            /// ACT
           
            // call the method to be tested
            await repo.Add(pilot);
            var result = await (_context.Pilots.Where(p => p.EmailId == pilot.EmailId).FirstOrDefaultAsync());

            /// ASSERT

            // if pilot was successfully added, testResult shouldn't be null
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task TestWhetherPilotWasDeleted()
        {
            /// ARANGE

            // setup PilotRepo to be tested
            var repo = new PilotRepo(_context);
            //Aircraft aircraft = _context.Aircrafts.Find("F-16");
            // setup pilot with name, email, aircraft and a password
            var pilot = CreatePilot("PilotDelete@USAF.mil");

            /// ACT

            await AddForTest(pilot);
            await repo.Delete(pilot.EmailId);
            var result = await (_context.Pilots.Where(p => p.EmailId == pilot.EmailId).FirstOrDefaultAsync());

            /// ASSERT

            Assert.IsNull(result);
        }

        [Test]
        public async Task TestPilotGetUser()
        {
            /// ARRANGE

            // setup PilotRepo to be tested
            var repo = new PilotRepo(_context);           
            // setup pilot with name, email, aircraft and a password
            var pilot = CreatePilot("DoctorGetUser@USAF.mil");

            /// ACT

            await AddForTest(pilot);
            var foundPilot = repo.GetUser(pilot.EmailId).Result;

            /// ASSERT

            Assert.IsNotNull(foundPilot);
        }

        [Test]
        public async Task TestPilotGetAll()
        {
            /// ARRANGE

            var repo = new PilotRepo(_context);
            var pilot = CreatePilot("Pilot1GetAll@USAF.mil");
            var pilot2 = CreatePilot("Pilot2GetAll@USAF.mil");

            /// ACT

            await AddForTest(pilot);
            await AddForTest(pilot2);
            var pilots = await repo.GetAll();

            /// ASSERT 

            Assert.IsTrue(pilots.ToList().Count == 2);
        }

       [Test]
        public async Task TestWhetherPilotUpdated()
        {
            /// ARRAGNE

            var repo = new PilotRepo(_context);
            var pilot = CreatePilot("PilotUpdated@USAF.mil");
            var oldPilot = pilot;

            /// ACT

            await AddForTest(pilot);         
            pilot.Name = "UPDATED NAME";
            await repo.Update(pilot);
            var pilots = await _context.Pilots.ToListAsync();
            var foundPilot = pilots.Where(p => p.Name == pilot.Name).FirstOrDefault();

            /// ASSERT

            Assert.IsTrue(foundPilot.Name == pilot.Name && foundPilot.EmailId == oldPilot.EmailId);
        }


        /*
         * Helper function to act as a pilot factory to avoid code copying.
         *    */
        public Pilot CreatePilot(string emailId)
        {
            return new Pilot { Aircraft = "F-16", EmailId = emailId, Name = "Test Pilot", Pwd = "a" };
        }

        /*
         * Helper function for adding a pilot durring ACT phase of tests.
         * Aids in avoiding code copying.
         *    */
        public async Task AddForTest(Pilot pilot)
        {
            await _context.Pilots.AddAsync(pilot);
            _context.Entry(pilot).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }     

        /*
         * Code to clean up all of the database changes made during
         * the test so there are no conflicts in the database entries
         * next time the code is run.
         */
         [TearDown]
        public async Task CleanUpPilotsAdded()
        {
            // get all pilots in the db
            var pilots = await _context.Pilots.ToListAsync();
            // walk through and remove them
            foreach(Pilot pilot in pilots)
            {
                _context.Remove(pilot);
                _context.Entry(pilot).State = EntityState.Deleted;
            }           
            // tell the db all the pilots were removed
            await _context.SaveChangesAsync();           
        }

    }
}