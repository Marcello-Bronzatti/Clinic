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
            if (!IsBusinessHour(dto.ScheduledAt))
                return false;

            var hasProfessionalConflict = await _appointmentRepository.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt);
            if (hasProfessionalConflict) return false;

            var hasPatientConflict = await _appointmentRepository.HasPatientConflictAsync(dto.PatientId, dto.ProfessionalId, dto.ScheduledAt);
            return !hasPatientConflict;
        }

        public async Task ScheduleAsync(CreateAppointmentDTO dto)
        {
            if (!await _patientRepository.ExistsAsync(dto.PatientId))
                throw new InvalidOperationException("Paciente não encontrado.");

            if (!await _professionalRepository.ExistsAsync(dto.ProfessionalId))
                throw new InvalidOperationException("Profissional não encontrado.");

            if (!await IsAvailableAsync(dto))
                throw new InvalidOperationException("O horário já está ocupado ou fora do expediente.");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                ScheduledAt = dto.ScheduledAt
            };

            await _appointmentRepository.AddAsync(appointment);
        }

        private bool IsBusinessHour(DateTime dateTime)
        {
            var day = dateTime.DayOfWeek;
            var hour = dateTime.TimeOfDay;

            return day != DayOfWeek.Saturday &&
                   day != DayOfWeek.Sunday &&
                   hour >= TimeSpan.FromHours(8) &&
                   hour < TimeSpan.FromHours(18);
        }
    }


}
