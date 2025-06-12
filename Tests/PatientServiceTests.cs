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
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _mockRepo;
        private PatientService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnPatients()
        {
            var patients = new List<Patient> { new Patient { Id = Guid.NewGuid(), FullName = "Teste", CPF = "12345678901", Email = "teste@email.com" } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task AddAsync_WhenPatientExists_ShouldThrowException()
        {
            var patient = new Patient { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.ExistsAsync(patient.Id)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(patient));
        }
    }
}
