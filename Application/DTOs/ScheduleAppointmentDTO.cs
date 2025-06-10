namespace Application.DTOs
{
    public class ScheduleAppointmentDTO
    {
        public int PatientId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime Date { get; set; } 
    }
}
