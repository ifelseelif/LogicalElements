using System;
using System.Threading.Tasks;
using Server.Db.models;
using Server.Models;
using Server.Models.request;

namespace Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> Register(string userCredential, string password);
        Task<OperationResult<User>> Login(string login, string password);
    }
}