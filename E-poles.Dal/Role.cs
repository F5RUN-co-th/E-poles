using Microsoft.AspNetCore.Identity;

namespace E_poles.Dal
{
    public partial class Role : IdentityRole<int>
    {
    }
    public enum RoleEnum
    {
        SuperAdministrator = 1,
        Administrator = 2,
        User = 3
    }
}
