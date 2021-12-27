using E_poles.Dal;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPagesAsync();

        Task AddToRoles(int applicationUserId, RoleEnum roleType);
    }
}
