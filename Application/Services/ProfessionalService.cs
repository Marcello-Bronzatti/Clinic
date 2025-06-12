using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProfessionalService
    {
        private readonly IProfessionalRepository _repository;

        public ProfessionalService(IProfessionalRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Professional>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Professional?> GetByIdAsync(Guid id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Professional professional)
        {
            if (await _repository.ExistsAsync(professional.Id))
                throw new InvalidOperationException("Profissional já cadastrado.");

            await _repository.AddAsync(professional);
        }
        public async Task DeleteAsync(Guid id)
        {
            var professional = await _repository.GetByIdAsync(id);
            if (professional == null)
                throw new InvalidOperationException("Profissional não encontrado.");

            await _repository.DeleteAsync(id);
        }

    }
}
