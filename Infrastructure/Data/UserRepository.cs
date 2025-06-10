using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            const string sql = @"SELECT * FROM Users WHERE Username = @Username";
            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task AddAsync(User user)
        {
            const string sql = @"INSERT INTO Users (Id, Username, Password) VALUES (@Id, @Username, @Password)";
            await _connection.ExecuteAsync(sql, user);
        }
    }

}
