using Microsoft.AspNetCore.Identity;

namespace E_poles.Dal
{
    public partial class Role : IdentityRole<int>
    {
    }
    public enum RoleEnum
    {
        SuperAdministrator = 0,
        Administrator = 1,
        User = 2
    }
}
