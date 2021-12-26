using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Db.models;
using Server.Db.Repository.Interface;

namespace Server.Db.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Guid> AddUser(string username)
        {
            try
            {
                var applicationContext = await _contextFactory.CreateDbContextAsync();
                if (await applicationContext.Users.FirstOrDefaultAsync(user => user.Username == username) != null)
                {
                    return Guid.Empty;
                }

                var id = Guid.NewGuid();
                await applicationContext.Users.AddAsync(new User
                {
                    Id = id,
                    Username = username
                });
                await applicationContext.SaveChangesAsync();
                return id;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public async Task<Guid> GetUserId(string username)
        {
            try
            {

                var applicationContext = await _contextFactory.CreateDbContextAsync();
                var user = await applicationContext.Users.FirstOrDefaultAsync(user => user.Username == username);

                return user?.Id ?? Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
    }
}