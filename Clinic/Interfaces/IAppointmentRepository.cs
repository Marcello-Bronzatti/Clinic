using Domain.Entities;

namespace Application.Services
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId, DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentsByProfessionalAsync(Guid professionalId);
        Task<bool> HasConflictAsync(Guid professionalId, DateTime scheduledAt);
        Task<bool> HasPatientConflictAsync(Guid patientId, Guid professionalId, DateTime scheduledAt);
        Task AddAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }

}
