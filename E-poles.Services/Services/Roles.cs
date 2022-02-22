using E_poles.Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public class Roles : IRoles
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public Roles(RoleManager<Role> roleManager,
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<int> Get(int userId)
        {
            var UserRoles = await _context.UserRoles.FirstOrDefaultAsync(f => f.UserId == userId);
            return UserRoles.RoleId;
        }

        public async Task GenerateRolesFromPagesAsync()
        {
            foreach (var roleName in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new Role { Name = roleName });
            }

        }
        public async Task RemoveFromRoles(int applicationUserId, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user != null)
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }
        }
        public async Task AddToRoles(int applicationUserId, RoleEnum roleType)
        {
            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user != null)
            {
                var roleValue = Convert.ToInt32(roleType);
                var roles = _roleManager.Roles.Where(w => w.Id == roleValue);
                List<string> listRoles = new List<string>();
                foreach (var item in roles)
                {
                    listRoles.Add(item.Name);
                }
                await _userManager.AddToRolesAsync(user, listRoles);
            }
        }
    }
}
