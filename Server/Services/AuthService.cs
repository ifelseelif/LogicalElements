using System;
using System.Threading.Tasks;
using Server.Db.models;
using Server.Db.Repository.Interface;
using Server.Models;
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

        public Task<OperationResult> Register(string username, string password)
        {
            return _userRepository.AddUser(username, password);
        }

        public Task<OperationResult<User>> Login(string login, string password)
        {
            return _userRepository.GetUserId(login, password);
        }
    }
}