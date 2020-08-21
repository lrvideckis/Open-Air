using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAir.Data.Models;

namespace OpenAir.Data
{
    /*
     * Class: PilotRepo
     * Description: Implements IUserRepo. Repository pattern applied to the Pilot entity.
     * 
     */
    public class PilotRepo : IUserRepo
    {
        // context reference to the database
        private readonly OpenAirDbContext db;

        /*
         * Class Constructor, set the context upon instantiation
         */
        public PilotRepo(OpenAirDbContext db)
        {
            this.db = db;
        }

        /*
         * Add method, Adds a user to the database context and prompts an update with the database
             */
        public async Task Add(IUser user)
        {
            await db.Pilots.AddAsync((Pilot)user);
            await db.SaveChangesAsync();
        }

        /*
         * Delete method, using the user's email, lookup their database entry and remove it.
         * The catch is in case it cannot find the user... 
         */
        public async Task Delete(string email)
        {
            // lets see if the pilot with this email even exists
            try
            { // TODO: see if remove and entity state deleted are redundent
                var pilot = GetUser(email).Result;
                db.Pilots.Remove((Pilot)pilot);
                db.Entry(pilot).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (System.ArgumentNullException) { } // doesn't exist-->need to handle this better
        }

        /*
         * GetAll method. Returns an IEnumerable of all the users in the Pilots table in the db.
         * The IEnumerable can be cast into the container of your choice later (ex. List). 
         */
        public async Task<IEnumerable<IUser>> GetAll()
        {
            var pilots = await db.Pilots.ToListAsync();
            return pilots.AsEnumerable();
        }

        /*
         *  GetUser Method. Uses the user email to lookup their entry in the db. 
         *  Returns a class containing all the column data as attributes.
         */
        public async Task<IUser> GetUser(string email)
        {
            return await db.Pilots.Where(p => p.EmailId == email).FirstOrDefaultAsync();
        }


        /*
         * Update method. Used to change existing attributes in a users entity instance.
         * TODO: Look into whether email should be passed in instead... i think maybe it should
         */
        public async Task Update(IUser user)
        {
            var entry = db.Entry(user);
            entry.State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
