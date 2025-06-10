using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDbConnection _connection;

        public PatientRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM Patients WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Patient>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Patients";
            return await _connection.QueryAsync<Patient>(sql);
        }

        public async Task AddAsync(Patient patient)
        {
            const string sql = "INSERT INTO Patients (Id, Name) VALUES (@Id, @Name)";
            await _connection.ExecuteAsync(sql, patient);
        }

        public async Task<bool> ExistsAsync(Guid patientId)
        {
            const string sql = "SELECT 1 FROM Patients WHERE Id = @Id";
            var result = await _connection.ExecuteScalarAsync<int?>(sql, new { Id = patientId });
            return result.HasValue;
        }
    }


}
