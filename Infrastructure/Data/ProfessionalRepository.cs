using Domain.Interfaces;
using System.Data;
using Dapper;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly IDbConnection _connection;

        public ProfessionalRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Professional> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM Professionals WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Professional>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Professional>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Professionals";
            return await _connection.QueryAsync<Professional>(sql);
        }

        public async Task AddAsync(Professional professional)
        {
            const string sql = @"
            INSERT INTO Professionals (FullName, Specialty, CRM)
            VALUES (@FullName, @Specialty, @CRM)";

            await _connection.ExecuteAsync(sql, professional);
        }

        public async Task<bool> ExistsAsync(Guid professionalId)
        {
            const string sql = "SELECT 1 FROM Professionals WHERE Id = @Id";
            var result = await _connection.ExecuteScalarAsync<int?>(sql, new { Id = professionalId });
            return result.HasValue;
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM Professionals WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

    }

}
