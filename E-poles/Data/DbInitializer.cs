using E_poles.Dal;
using E_poles.Services;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
           IFunctional functional)
        {
            context.Database.EnsureCreated();

            //check for users
            if (!context.Users.Any())
            {
                //init app with super admin user
                await functional.CreateDefaultSuperAdmin();
            }

            //init app data
            await functional.InitAppData();

            return;
        }
    }
}
