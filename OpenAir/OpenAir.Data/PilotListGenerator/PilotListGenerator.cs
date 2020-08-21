using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data
{
    public static class PilotListGenerator
    {
        public static async System.Threading.Tasks.Task<List<string>> GetPilotEmailsAsync(PilotRepo _db_pilots)
        {
            var pilotList = _db_pilots.GetAll();
            List<string> emailList = new List<string>();
            foreach (var pilot in await pilotList)
            {
                emailList.Add(pilot.EmailId);
            }
            return emailList;
        }
    }
}
