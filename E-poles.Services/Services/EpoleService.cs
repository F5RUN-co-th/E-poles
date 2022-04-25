using Dapper;
using Dapper.Contrib.Extensions;
using E_poles.Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public class EpoleService : IEpoleService
    {
        private readonly ApplicationDbContext _context;

        public EpoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateAsync(Poles model)
        {
            _context.Poles.Update(model);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Poles> CreateAsync(Poles model)
        {
            var data = await _context.Poles.AddAsync(model);
            await _context.SaveChangesAsync();

            return data.Entity;
        }

        public async Task<bool> DeleteAsync(Poles model)
        {
            _context.Poles.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Poles>> GetLast(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            string condition;
            if (groupsId == (int)RoleEnum.SuperAdministrator)
            {
                condition = "1=1";
            }
            else
            {
                condition = $"GroupsId = {groupsId}";
            }
            var query = String.Format(@"SELECT Top 1 p.[Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status],[GroupsId],[UpdatedAt],
users.Id as usersID,
users.UserName
  FROM [dbo].[Poles] p Left Join Users users ON p.UpdatedBy = users.Id
  WHERE {0} ORDER BY id DESC", condition);

            var poles = await SqlMapper.QueryAsync<Poles, User, Poles>(connection, query,
                    (poles, users) =>
                    {
                        poles.Users = users;
                        return poles;
                    }, splitOn: "usersID");

            //var poles = await SqlMapper.QueryAsync<Poles, User>(connection, query, commandType: CommandType.Text);

            return poles;
            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status] FROM [dbo].[Poles];", commandType: CommandType.Text);

            //    return poles;
            //}

        }

        public async Task<IEnumerable<Poles>> GetAll(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            string condition;
            if (groupsId == (int)RoleEnum.SuperAdministrator)
            {
                condition = "1=1";
            }
            else
            {
                condition = $"GroupsId = {groupsId}";
            }
            var query = String.Format(@"SELECT p.[Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status],[GroupsId],[UpdatedAt],
users.Id as usersID,
users.UserName
  FROM [dbo].[Poles] p Left Join Users users ON p.UpdatedBy = users.Id
  WHERE {0}", condition);

            var poles = await SqlMapper.QueryAsync<Poles, User, Poles>(connection, query,
                    (poles, users) =>
                    {
                        poles.Users = users;
                        return poles;
                    }, splitOn: "usersID");

            //var poles = await SqlMapper.QueryAsync<Poles, User>(connection, query, commandType: CommandType.Text);

            return poles;
            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status] FROM [dbo].[Poles];", commandType: CommandType.Text);

            //    return poles;
            //}

        }

        public async Task<IEnumerable<Poles>> GetAllArea(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            string condition;
            if (groupsId == (int)RoleEnum.SuperAdministrator)
            {
                condition = "1=1";
            }
            else
            {
                condition = $"GroupsId = {groupsId}";
            }
            var query = String.Format("SELECT [Area] FROM [dbo].[Poles] WHERE {0} GROUP BY Area;", condition);
            var poles = await SqlMapper.QueryAsync<Poles>(connection, query, commandType: CommandType.Text);

            return poles;
        }

        public async Task<IEnumerable<Poles>> GetAllStreet(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            string condition;
            if (groupsId == (int)RoleEnum.SuperAdministrator)
            {
                condition = "1=1";
            }
            else
            {
                condition = $"GroupsId = {groupsId}";
            }
            var query = String.Format("SELECT [Street] FROM [dbo].[Poles] WHERE {0} GROUP BY Street;", condition);
            var poles = await SqlMapper.QueryAsync<Poles>(connection, query, commandType: CommandType.Text);

            return poles;
        }
    }
}
