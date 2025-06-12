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
            const string sql = "INSERT INTO Patients (FullName, CPF, Email) VALUES (@FullName, @CPF, @Email)";
            await _connection.ExecuteAsync(sql, patient);
        }

        public async Task<bool> ExistsAsync(Guid patientId)
        {
            const string sql = "SELECT 1 FROM Patients WHERE Id = @Id";
            var result = await _connection.ExecuteScalarAsync<int?>(sql, new { Id = patientId });
            return result.HasValue;
        }
        public async Task DeleteAsync(Guid id)
        {
            const string sql = "DELETE FROM Patients WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<bool> HasPatientConflictAsync(Guid patientId, Guid professionalId, DateTime scheduledAt)
        {
            const string sql = @"
        SELECT 1 FROM Appointments 
        WHERE PatientId = @PatientId 
          AND ProfessionalId = @ProfessionalId 
          AND CAST(ScheduledAt AS DATE) = @Date";

            var result = await _connection.ExecuteScalarAsync<int?>(sql, new
            {
                PatientId = patientId,
                ProfessionalId = professionalId,
                Date = scheduledAt.Date
            });

            return result.HasValue;
        }
    }
}
