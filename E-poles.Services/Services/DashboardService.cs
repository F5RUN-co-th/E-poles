using E_poles.Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetAllInfomation()
        {
            return new
            {
                TotalAdmin = await _context.UserRoles.CountAsync(w => w.RoleId == Convert.ToInt32(RoleEnum.Administrator)),
                TotalUser = await _context.UserRoles.CountAsync(w => w.RoleId == Convert.ToInt32(RoleEnum.User)),
                TotalPole = await _context.Poles.CountAsync()
            };
        }
    }
}
