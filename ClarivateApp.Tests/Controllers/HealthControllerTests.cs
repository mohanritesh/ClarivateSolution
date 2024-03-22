using ClarivateApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;
#nullable disable

namespace ClarivateApp.Tests.Controllers
{
    public class HealthControllerTests
    {
        private MockRepository mockRepository;
        public HealthControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private HealthController CreateHealthController()
        {
            return new HealthController();
        }

        [Fact]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var healthController = this.CreateHealthController();

            // Act
            var result = healthController.Index() as OkObjectResult;

            // Assert
            Assert.Equal(200, result.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}
