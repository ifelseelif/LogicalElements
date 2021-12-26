using System;
using System.Threading.Tasks;
using Server.Db.Repository.Interface;
using Server.Models.request;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Guid> Register(string username)
        {
            return _userRepository.AddUser(username);
        }

        public Task<Guid> ContainsUser(string username)
        {
            return _userRepository.GetUserId(username);
        }
    }
}