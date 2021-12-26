using System;
using System.Threading.Tasks;

namespace Server.Db.Repository.Interface
{
    public interface IUserRepository
    {
        Task<Guid> AddUser(string username);
        Task<Guid> GetUserId(string username);
    }
}