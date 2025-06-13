using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProfessionalRepository
    {
        Task<Professional> GetByIdAsync(Guid id);
        Task<IEnumerable<Professional>> GetAllAsync();
        Task AddAsync(Professional professional);
        Task<bool> ExistsAsync(Guid professionalId);
        Task DeleteAsync(Guid id);
    }
}
