using System;
using System.Threading.Tasks;
using Server.Db.models;
using Server.Models;

namespace Server.Db.Repository.Interface
{
    public interface IUserRepository
    {
        Task<OperationResult> AddUser(string username, string password);
        Task<OperationResult<User>> GetUserId(string username, string password);
    }
}