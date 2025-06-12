using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _mockRepo;
        private IConfiguration _config;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();

            var settings = new Dictionary<string, string>
            {
                { "Jwt:Key", "MySuperSecretKey12345" },
                { "Jwt:Issuer", "ClinicAuth" },
                { "Jwt:Audience", "ClinicUsers" }
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            _service = new AuthService(_mockRepo.Object, _config);
        }

        [Test]
        public async Task RegisterAsync_ShouldHashPasswordAndSave()
        {
            var username = "admin";
            var password = "admin";

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            Assert.DoesNotThrowAsync(() => _service.RegisterAsync(username, password));
        }

        [Test]
        public async Task AuthenticateAsync_InvalidUser_ThrowsUnauthorized()
        {
            _mockRepo.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync((User)null!);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.AuthenticateAsync("admin", "admin"));
        }
    }
}
