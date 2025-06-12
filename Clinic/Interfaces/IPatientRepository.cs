using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> GetByIdAsync(Guid id);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task<bool> ExistsAsync(Guid patientId);
        Task DeleteAsync(Guid id);
    }
}
