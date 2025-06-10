namespace Application.DTOs
{
    public class CreateAppointmentDTO
    {
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
