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

        public async Task<IEnumerable<Poles>> GetAll(int groupsId)
        {
            var connection = _context.Database.GetDbConnection();
            var query = String.Format("SELECT [Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status],[GroupsId] FROM [dbo].[Poles] WHERE GroupsId = {0}", groupsId);
            var poles = await SqlMapper.QueryAsync<Poles>(connection, query, commandType: CommandType.Text);

            return poles;
            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status] FROM [dbo].[Poles];", commandType: CommandType.Text);

            //    return poles;
            //}

        }

        public async Task<IEnumerable<Poles>> GetAllArea()
        {
            var connection = _context.Database.GetDbConnection();
            var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Area] FROM [dbo].[Poles] GROUP BY Area;", commandType: CommandType.Text);

            return poles;
        }
        public async Task<IEnumerable<Poles>> GetAllStreet()
        {
            var connection = _context.Database.GetDbConnection();
            var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Street] FROM [dbo].[Poles] GROUP BY Street;", commandType: CommandType.Text);

            return poles;
        }
    }
}
