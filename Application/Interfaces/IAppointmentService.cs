using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task ScheduleAsync(CreateAppointmentDTO dto);
        Task<bool> IsAvailableAsync(CreateAppointmentDTO dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAppointmentsByProfessionalAsync(Guid professionalId);
    }
}
