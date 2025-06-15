using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<bool> IsAvailableAsync(CreateAppointmentDTO dto);
        Task ScheduleAsync(CreateAppointmentDTO dto);
        Task<IEnumerable<Appointment>> GetAppointmentsByProfessionalAsync(Guid professionalId);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId, DateTime date);
        Task<IEnumerable<string>> GetAvailableTimesAsync(Guid professionalId, DateTime date);
        Task CancelAsync(Guid id);
    }
}
