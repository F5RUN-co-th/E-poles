using Dapper;
using Dapper.Contrib.Extensions;
using E_poles.Dal;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Poles> CreateAsync(Poles model)
        {
            //long objId = 0;
            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    objId = await connection.InsertAsync(model);
            //}

            //return await _context.Database.GetDbConnection().GetAsync<Poles>(objId);
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

        public async Task<IEnumerable<Poles>> GetAll()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Id],[Name],[Latitude],[Longitude],[Area],[Street],[Note],[Description],[Status] FROM [dbo].[Poles];", commandType: CommandType.Text);

                return poles;
            }

        }
    }
}
