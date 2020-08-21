using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAir.Data.Models;
namespace OpenAir.Data
{
    /*
     * Interface: Interface to describe what a user entity should have
     */
    public interface IUserRepo
    {
        Task<IEnumerable<IUser>> GetAll();

        Task<IUser> GetUser(string email);

        Task Add(IUser user); // should this actuall return a bool? If so, then error messages can be output based on the bool

        Task Update(IUser user);

        Task Delete(string email);
    }
}
