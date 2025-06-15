using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AppointmentService:IAppointmentService
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

            // Profissional não pode ter outro agendamento nesse horário
            var hasProfessionalConflict = await _appointmentRepository.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt);
            if (hasProfessionalConflict)
                return false;

            // Paciente não pode ter outro agendamento com o mesmo profissional no mesmo dia
            var hasPatientConflict = await _appointmentRepository.HasPatientConflictAsync(dto.PatientId, dto.ProfessionalId, dto.ScheduledAt);
            if (hasPatientConflict)
                return false;

            return true;
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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByProfessionalAsync(Guid professionalId)
        {
            return await _appointmentRepository.GetAppointmentsByProfessionalAsync(professionalId);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId, DateTime date)
        {
            return await _appointmentRepository.GetByProfessionalIdAsync(professionalId, date);
        }

        public async Task<IEnumerable<string>> GetAvailableTimesAsync(Guid professionalId, DateTime date)
        {
            var allSlots = GenerateTimeSlots(); // Gera horários padrão: 08:00, 08:30, ..., 17:30
            var booked = await _appointmentRepository.GetByProfessionalIdAsync(professionalId, date);

            var bookedTimes = booked.Select(a => a.ScheduledAt.ToString("HH:mm")).ToHashSet();
            return allSlots.Where(t => !bookedTimes.Contains(t));
        }

        private List<string> GenerateTimeSlots()
        {
            var slots = new List<string>();
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(17, 30, 0);
            var interval = new TimeSpan(0, 30, 0);

            for (var time = start; time <= end; time += interval)
            {
                slots.Add(time.ToString(@"hh\:mm"));
            }

            return slots;
        }

        public async Task CancelAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new ArgumentException("Consulta não encontrada.");

            await _appointmentRepository.DeleteAsync(id);
        }
    }
}
