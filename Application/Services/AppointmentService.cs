using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IProfessionalRepository _professionalRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IProfessionalRepository professionalRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _professionalRepository = professionalRepository;
        }

        public async Task<bool> IsAvailableAsync(CreateAppointmentDTO dto)
        {
            if (!await _professionalRepository.ExistsAsync(dto.ProfessionalId))
                throw new ArgumentException("Profissional não encontrado.");

            if (!await _patientRepository.ExistsAsync(dto.PatientId))
                throw new ArgumentException("Paciente não encontrado.");

            var hasProfessionalConflict = await _appointmentRepository.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt);
            if (hasProfessionalConflict) return false;

            var hasPatientConflict = await _appointmentRepository.HasPatientConflictAsync(dto.PatientId, dto.ProfessionalId, dto.ScheduledAt);
            return !hasPatientConflict;
        }

        public async Task ScheduleAsync(CreateAppointmentDTO dto)
        {
            if (!await IsAvailableAsync(dto))
                throw new InvalidOperationException("O horário já está ocupado para este profissional ou paciente.");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                ScheduledAt = dto.ScheduledAt
            };

            await _appointmentRepository.AddAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByProfessionalAsync(Guid professionalId, DateTime date)
        {
            return await _appointmentRepository.GetByProfessionalIdAsync(professionalId, date);
        }
    }
}
