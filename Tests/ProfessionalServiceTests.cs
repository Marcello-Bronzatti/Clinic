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
    public class ProfessionalServiceTests
    {
        private Mock<IProfessionalRepository> _mockRepo;
        private ProfessionalService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IProfessionalRepository>();
            _service = new ProfessionalService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnProfessionals()
        {
            var professionals = new List<Professional> { new Professional { Id = Guid.NewGuid(), FullName = "Prof", Specialty = "Cardio", CRM = "12345" } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(professionals);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task AddAsync_WhenProfessionalExists_ShouldThrowException()
        {
            var prof = new Professional { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.ExistsAsync(prof.Id)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(prof));
        }
    }
}
