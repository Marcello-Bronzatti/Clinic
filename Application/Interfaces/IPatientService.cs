using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(Guid id);
        Task AddAsync(Patient patient);
        Task DeleteAsync(Guid id);
    }
}