using System;
using System.Threading.Tasks;
using Server.Models.request;

namespace Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Guid> Register(string userCredential);
        Task<Guid> ContainsUser(string model);
    }
}