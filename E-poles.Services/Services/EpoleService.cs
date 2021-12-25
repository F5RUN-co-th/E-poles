using Dapper;
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

        public async Task<IEnumerable<Poles>> GetAll()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var poles = await SqlMapper.QueryAsync<Poles>(connection, "SELECT [Id],[Name],[Latitude],[Longitude],[Zone],[Status] FROM [dbo].[Poles];", commandType: CommandType.Text);

                return poles;
            }

        }
    }
}
