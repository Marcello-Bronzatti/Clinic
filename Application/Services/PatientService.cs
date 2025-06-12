using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository repository)
        {
            _patientRepository = repository;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() =>
            await _patientRepository.GetAllAsync();

        public async Task<Patient?> GetByIdAsync(Guid id) =>
            await _patientRepository.GetByIdAsync(id);

        public async Task AddAsync(Patient patient)
        {
            if (await _patientRepository.ExistsAsync(patient.Id))
                throw new InvalidOperationException("Paciente já cadastrado.");

            await _patientRepository.AddAsync(patient);
        }
        public async Task DeleteAsync(Guid id)
        {
            var existing = await _patientRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Paciente não encontrado.");

            await _patientRepository.DeleteAsync(id);
        }
    }
}
