using Application.Services;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Data
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnection _connection;

        public AppointmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM Appointments WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Appointment>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId, DateTime date)
        {
            const string sql = @"
            SELECT * FROM Appointments 
            WHERE ProfessionalId = @ProfessionalId 
              AND CAST(ScheduledAt AS DATE) = @Date";
            return await _connection.QueryAsync<Appointment>(sql, new { ProfessionalId = professionalId, Date = date.Date });
        }

        public async Task<bool> HasConflictAsync(Guid professionalId, DateTime scheduledAt)
        {
            const string sql = @"
            SELECT 1 FROM Appointments 
            WHERE ProfessionalId = @ProfessionalId AND ScheduledAt = @ScheduledAt";
            var result = await _connection.ExecuteScalarAsync<int?>(sql, new { ProfessionalId = professionalId, ScheduledAt = scheduledAt });
            return result.HasValue;
        }

        public async Task<bool> HasPatientConflictAsync(Guid patientId, Guid professionalId, DateTime scheduledAt)
        {
            const string sql = @"
            SELECT 1 FROM Appointments 
            WHERE PatientId = @PatientId AND ProfessionalId = @ProfessionalId AND CAST(ScheduledAt AS DATE) = @Date";
            var result = await _connection.ExecuteScalarAsync<int?>(sql, new
            {
                PatientId = patientId,
                ProfessionalId = professionalId,
                Date = scheduledAt.Date
            });
            return result.HasValue;
        }

        public async Task AddAsync(Appointment appointment)
        {
            const string sql = @"INSERT INTO Appointments (Id, PatientId, ProfessionalId, ScheduledAt) 
                             VALUES (@Id, @PatientId, @ProfessionalId, @ScheduledAt)";
            await _connection.ExecuteAsync(sql, appointment);
        }
    }

}


