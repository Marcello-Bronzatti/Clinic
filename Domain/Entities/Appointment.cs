namespace Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid ProfessionalId { get; set; }
        public string ProfessionalName { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
