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
    [TestFixture]
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _repo;
        private PatientService _service;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<IPatientRepository>();
            _service = new PatientService(_repo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnPatients()
        {
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Patient>
            {
                new Patient { Id = Guid.NewGuid(), FullName = "Alice" }
            });

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task AddAsync_ShouldCallRepository_WhenPatientIsNew()
        {
            var patient = new Patient { Id = Guid.NewGuid(), FullName = "João" };

            _repo.Setup(r => r.ExistsAsync(patient.Id)).ReturnsAsync(false);

            await _service.AddAsync(patient);

            _repo.Verify(r => r.AddAsync(patient), Times.Once);
        }

        [Test]
        public void AddAsync_ShouldThrow_WhenPatientExists()
        {
            var patient = new Patient { Id = Guid.NewGuid(), FullName = "Repetido" };

            _repo.Setup(r => r.ExistsAsync(patient.Id)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(patient));
        }
    }
}
