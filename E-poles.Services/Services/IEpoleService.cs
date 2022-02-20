using E_poles.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public interface IEpoleService
    {
        Task<IEnumerable<Poles>> GetAll(int groupsId);
        Task<IEnumerable<Poles>> GetAllArea();
        Task<IEnumerable<Poles>> GetAllStreet();
        Task<bool> DeleteAsync(Poles model);
        Task<Poles> CreateAsync(Poles model);
        Task<bool> UpdateAsync(Poles model);
    }
}
