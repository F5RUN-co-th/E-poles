using Dapper;
using Dapper.Contrib.Extensions;
using E_poles.Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;

        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Groups> Get(int Id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(f => f.Id == Id);
            return group;
        }
        public async Task<bool> UpdateAsync(Groups model)
        {
            _context.Groups.Update(model);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Groups> CreateAsync(Groups model)
        {
            var data = await _context.Groups.AddAsync(model);
            await _context.SaveChangesAsync();

            return data.Entity;
        }

        public async Task<bool> DeleteAsync(Groups model)
        {
            _context.Groups.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Groups>> GetAll(int groupsId)
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
            var query = String.Format("SELECT [Id],[Name] FROM [E-poles].[dbo].[Groups] WHERE {0}", condition);
            var groups = await SqlMapper.QueryAsync<Groups>(connection, query, commandType: CommandType.Text);

            return groups;
        }

        public async Task<UserGroups> GetGroupByUserId(int userId)
        {
            var userGroups = await _context.UserGroups.FirstOrDefaultAsync(f => f.UserId == userId);
            return userGroups;
        }
    }
}
