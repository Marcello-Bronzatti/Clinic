namespace Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid ProfessionalId { get; set; }
        public DateTime ScheduledAt { get; set; }
        private int DurationInMinutes { get; } = 30;

      /*  public bool IsWithinBusinessHours()
        {
            var start = TimeSpan.FromHours(8);
            var end = TimeSpan.FromHours(18);
            var time = ScheduledAt.TimeOfDay;

            return time >= start && time.Add(TimeSpan.FromMinutes(DurationInMinutes)) <= end;
        }

        */
    }
}
