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
    public class ProfessionalServiceTests
    {
        private Mock<IProfessionalRepository> _repo;
        private ProfessionalService _service;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<IProfessionalRepository>();
            _service = new ProfessionalService(_repo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnList()
        {
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Professional>
            {
                new Professional { Id = Guid.NewGuid(), FullName = "Dr. House" }
            });

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task AddAsync_ShouldSucceed_WhenNotExists()
        {
            var prof = new Professional { Id = Guid.NewGuid(), FullName = "Dra. Jane" };

            _repo.Setup(r => r.ExistsAsync(prof.Id)).ReturnsAsync(false);

            await _service.AddAsync(prof);

            _repo.Verify(r => r.AddAsync(prof), Times.Once);
        }

        [Test]
        public void AddAsync_ShouldThrow_WhenAlreadyExists()
        {
            var prof = new Professional { Id = Guid.NewGuid() };

            _repo.Setup(r => r.ExistsAsync(prof.Id)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(prof));
        }
       
    }
}
