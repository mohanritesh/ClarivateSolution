using ClarivateApp.Controllers;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ClarivateApp.Tests.Controllers
{
    public class UserControllerTests
    {

        [Fact]
        public async Task GetRandomUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mockRepository = new MockRepository(MockBehavior.Loose);
            var httpClientFactoryMock = mockRepository.Create<IHttpClientFactory>();
            var httpClientMock = mockRepository.Create<HttpClient>();

            var validResponseContent = @"{
                ""results"": [
                    {
                        ""gender"": ""female"",
                        ""name"": { ""title"": ""Ms"", ""first"": ""Vanessa"", ""last"": ""Carter"" },
                        ""email"": ""vanessa.carter@example.com""
                    }
                ]
            }";

            var validResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(validResponseContent)
            };

            httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(validResponse);

            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClientMock.Object);

            var controller = new UserController(httpClientFactoryMock.Object);

            // Act
            var response = await controller.GetRandomUser();

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(response);

            var okResult = (Microsoft.AspNetCore.Mvc.OkObjectResult)response;
            dynamic jsonResponse = okResult.Value;
            Assert.NotNull(jsonResponse.results);
            Assert.Single(jsonResponse.results);
            Assert.Equal("female", jsonResponse.results[0].gender.ToString());
            Assert.Equal("Ms", jsonResponse.results[0].name.title.ToString());
            Assert.Equal("Vanessa", jsonResponse.results[0].name.first.ToString());
            Assert.Equal("Carter", jsonResponse.results[0].name.last.ToString());
            Assert.Equal("vanessa.carter@example.com", jsonResponse.results[0].email.ToString());
        }
    }

}
