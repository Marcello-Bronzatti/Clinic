using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _appointmentRepo;
        private Mock<IPatientRepository> _patientRepo;
        private Mock<IProfessionalRepository> _professionalRepo;
        private AppointmentService _service;

        [SetUp]
        public void Setup()
        {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _professionalRepo = new Mock<IProfessionalRepository>();

            _service = new AppointmentService(
                _appointmentRepo.Object,
                _patientRepo.Object,
                _professionalRepo.Object);
        }

        [Test]
        public async Task IsAvailableAsync_ShouldReturnFalse_IfProfessionalHasConflict()
        {
            var dto = new CreateAppointmentDTO
            {
                PatientId = Guid.NewGuid(),
                ProfessionalId = Guid.NewGuid(),
                ScheduledAt = DateTime.Today.AddHours(10)
            };

            _professionalRepo.Setup(r => r.ExistsAsync(dto.ProfessionalId)).ReturnsAsync(true);
            _patientRepo.Setup(r => r.ExistsAsync(dto.PatientId)).ReturnsAsync(true);
            _appointmentRepo.Setup(r => r.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt)).ReturnsAsync(true);

            var result = await _service.IsAvailableAsync(dto);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ScheduleAsync_ShouldThrow_WhenNotAvailable()
        {
            var dto = new CreateAppointmentDTO
            {
                PatientId = Guid.NewGuid(),
                ProfessionalId = Guid.NewGuid(),
                ScheduledAt = DateTime.Today.AddHours(10)
            };

            _professionalRepo.Setup(r => r.ExistsAsync(dto.ProfessionalId)).ReturnsAsync(true);
            _patientRepo.Setup(r => r.ExistsAsync(dto.PatientId)).ReturnsAsync(true);
            _appointmentRepo.Setup(r => r.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.ScheduleAsync(dto));
        }

        [Test]
        public async Task ScheduleAsync_ShouldSucceed_WhenAvailable()
        {
            var dto = new CreateAppointmentDTO
            {
                PatientId = Guid.NewGuid(),
                ProfessionalId = Guid.NewGuid(),
                ScheduledAt = DateTime.Today.AddHours(10)
            };

            _professionalRepo.Setup(r => r.ExistsAsync(dto.ProfessionalId)).ReturnsAsync(true);
            _patientRepo.Setup(r => r.ExistsAsync(dto.PatientId)).ReturnsAsync(true);
            _appointmentRepo.Setup(r => r.HasConflictAsync(dto.ProfessionalId, dto.ScheduledAt)).ReturnsAsync(false);
            _appointmentRepo.Setup(r => r.HasPatientConflictAsync(dto.PatientId, dto.ProfessionalId, dto.ScheduledAt)).ReturnsAsync(false);

            await _service.ScheduleAsync(dto);

            _appointmentRepo.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAppointments()
        {
            var list = new List<Appointment>
            {
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = Guid.NewGuid(),
                    ProfessionalId = Guid.NewGuid(),
                    ScheduledAt = DateTime.Now,
                    PatientName = "Teste Paciente",
                    ProfessionalName = "Dr. Teste"
                }
            };

            _appointmentRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }
    }
}
