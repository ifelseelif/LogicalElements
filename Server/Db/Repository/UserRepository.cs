using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Db.models;
using Server.Db.Repository.Interface;
using Server.Models;

namespace Server.Db.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<OperationResult> AddUser(string username, string password)
        {
            try
            {
                var applicationContext = await _contextFactory.CreateDbContextAsync();
                if (await applicationContext.Users.FirstOrDefaultAsync(user => user.Username == username) != null)
                {
                    return new OperationResult(HttpStatusCode.BadRequest, "User already created");
                }

                var id = Guid.NewGuid();
                await applicationContext.Users.AddAsync(new User
                {
                    Id = id,
                    Username = username,
                    Password = password
                });
                await applicationContext.SaveChangesAsync();
                return new OperationResult();
            }
            catch (Exception)
            {
                return new OperationResult(HttpStatusCode.InternalServerError, "try again later");
            }
        }

        public async Task<OperationResult<User>> GetUserId(string username, string password)
        {
            try
            {
                var applicationContext = await _contextFactory.CreateDbContextAsync();
                var user = await applicationContext.Users.FirstOrDefaultAsync(user => user.Username == username);
                if (user == null) return new OperationResult<User>(HttpStatusCode.NotFound, "User not found");
                return user.Password != password
                    ? new OperationResult<User>(HttpStatusCode.BadRequest, "User password invalid")
                    : new OperationResult<User>(user);
            }
            catch (Exception)
            {
                return new OperationResult<User>(HttpStatusCode.InternalServerError, "try again later");
            }
        }
    }
}