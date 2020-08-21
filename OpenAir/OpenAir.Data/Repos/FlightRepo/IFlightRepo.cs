using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAir.Data.Models;

namespace OpenAir.Data
{
    public interface IFlightRepo
    {
        Task<IEnumerable<IFlight>> GetAll();
        Task<IEnumerable<IFlight>> GetAll(string email);
        Task<IEnumerable<IFlight>> GetAll(DateTime date);
        Task<IEnumerable<IFlight>> GetAll(DateTime date, string route, string email);
        Task<IFlight> GetFlight(DateTime takeOff, string routeId);
        Task Add(IFlight flight);
        Task Update(IFlight flight);
        Task Delete(DateTime takeOff, string routeId);
    }
}
