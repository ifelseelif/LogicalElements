using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Db.models;
using Server.Db.Repository.Interface;

namespace Server.Db.Repository
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public ConnectionRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Connection>> GetAllConnectionsByUserId(Guid userId)
        {
            var applicationContext = await _contextFactory.CreateDbContextAsync();
            return await applicationContext.Connections
                .Where(elem => elem.UserId == userId)
                .ToListAsync();
        }

        public async Task AddConnection(Connection connection)
        {
            var applicationContext = await _contextFactory.CreateDbContextAsync();
            await applicationContext.Connections.AddAsync(connection);
            await applicationContext.SaveChangesAsync();
        }
    }
}