using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Patient?> GetByIdAsync(Guid id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Patient patient)
        {
            if (await _repository.ExistsAsync(patient.Id))
                throw new InvalidOperationException("Paciente já cadastrado.");

            await _repository.AddAsync(patient);
        }
    }
}
