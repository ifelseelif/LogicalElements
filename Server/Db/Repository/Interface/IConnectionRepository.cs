using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Db.models;

namespace Server.Db.Repository.Interface
{
    public interface IConnectionRepository
    {
        Task<List<Connection>> GetAllConnectionsByUserId(Guid userId);
        Task AddConnection(Connection connection);
    }
}