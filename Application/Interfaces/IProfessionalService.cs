using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProfessionalService
    {
        Task<IEnumerable<Professional>> GetAllAsync();
        Task<Professional?> GetByIdAsync(Guid id);
        Task AddAsync(Professional professional);
        Task DeleteAsync(Guid id);
    }
}