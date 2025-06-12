using Application.DTOs;
using Application.Services;
using Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class AppointmentServiceTests
    {
        [Test]
        public async Task ScheduleAsync_ShouldThrow_WhenNotAvailable()
        {
            // Arrange
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var patientRepo = new Mock<IPatientRepository>();
            var professionalRepo = new Mock<IProfessionalRepository>();

            var dto = new CreateAppointmentDTO
            {
                PatientId = Guid.NewGuid(),
                ProfessionalId = Guid.NewGuid(),
                ScheduledAt = DateTime.Today.AddHours(10)
            };

            professionalRepo.Setup(r => r.ExistsAsync(dto.ProfessionalId)).ReturnsAsync(true);
            patientRepo.Setup(r => r.ExistsAsync(dto.PatientId)).ReturnsAsync(true);
            appointmentRepo.Setup(r => r.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt)).ReturnsAsync(true);

            var service = new AppointmentService(appointmentRepo.Object, patientRepo.Object, professionalRepo.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => service.ScheduleAsync(dto));
        }
    }
}
