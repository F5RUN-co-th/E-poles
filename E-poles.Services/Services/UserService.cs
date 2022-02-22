using Dapper;
using Dapper.Contrib.Extensions;
using E_poles.Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<User> Get(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == Id);
            return user;
        }
        public async Task<bool> UpdateAsync(User model)
        {
            _context.Users.Update(model);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IdentityResult> CreateAsync(User model, string password)
        {
            var result = await _userManager.CreateAsync(model, password);

            return result;
        }

        public async Task<bool> DeleteAsync(User model)
        {
            _context.Users.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAll(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            string condition;
            if (groupsId == (int)RoleEnum.SuperAdministrator)
            {
                condition = "1=1";
            }
            else
            {
                condition = $"Id != {(int)RoleEnum.SuperAdministrator}";
            }
            var query = String.Format(@"SELECT [Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount] FROM [dbo].[Users]
 WHERE {0}", condition);
            var users = await SqlMapper.QueryAsync<User>(connection, query, commandType: CommandType.Text);

            return users;
        }
    }
}
