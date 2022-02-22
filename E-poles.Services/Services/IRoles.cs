using E_poles.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public interface IRoles
    {
        Task<int> Get(int userId);

        Task GenerateRolesFromPagesAsync();

        Task AddToRoles(int applicationUserId, RoleEnum roleType);
        Task RemoveFromRoles(int id, IList<string> role);
    }
}
