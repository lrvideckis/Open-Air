using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAir.Data.Models;

namespace OpenAir.Data
{
    public interface IRouteRepo
    {
        Task<IEnumerable<IRoute>> GetAll();

        Task<IRoute> GetRoute(string id);

        //void Add(IRoute user);

        //void Update(IRoute user);

        //bool Delete(int id);
    }
}
