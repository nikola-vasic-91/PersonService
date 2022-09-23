using Xunit;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.API.Controllers;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Models;
using PersonService.Domain.Modules;
using PersonService.Domain.Queries;

namespace PersonService.Tests.UnitTests.API
{
    public class SocialMediaAccountsControllerTests
    {
        #region Private fields

        private readonly ILogger<SocialMediaAccountsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private SocialMediaAccountsController _controller;

        #endregion

        #region Constructor

        public SocialMediaAccountsControllerTests()
        {
            var logMock = new Mock<ILogger<SocialMediaAccountsController>>();
            _logger = logMock.Object;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetSocialMediaAccounts>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SocialMediaAccount> { GetSocialMediaAccount() });
            _mediator = mediatorMock.Object;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfiguration.CreateMapper();

            _controller = new SocialMediaAccountsController(_mediator, _mapper, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task GetSocialMediaAccountsAsync_GetSocialMediaAccountsQuery_Success()
        {
            // Arrange
            // Act
            var response = await _controller.GetSocialMediaAccountsAsync();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<SocialMediaAccountDto>>(result?.Value);
            Assert.NotEmpty(result?.Value as IEnumerable<SocialMediaAccountDto>);
        }

        [Fact]
        public async Task GetSocialMediaAccountsAsync_GetSocialMediaAccountsQueryFail_InternalServerError()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetSocialMediaAccounts>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            _controller = new SocialMediaAccountsController(mediatorMock.Object, _mapper, _logger);

            // Act
            var response = await _controller.GetSocialMediaAccountsAsync();

            // Assert
            var result = response.Result as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(500, result?.StatusCode);
        }

        [Fact]
        public async Task GetSocialMediaAccountsAsync_GetSocialMediaAccountsQuery_NoSocialMediaAccountsFound_NotFound()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetSocialMediaAccounts>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SocialMediaAccount>());
            _controller = new SocialMediaAccountsController(mediatorMock.Object, _mapper, _logger);

            // Act
            var response = await _controller.GetSocialMediaAccountsAsync();

            // Assert
            var result = response.Result as NotFoundObjectResult;
            Assert.Equal(404, result?.StatusCode);
            Assert.Equal("No social media account was found.", result?.Value);
        }

        #endregion

        #region Private methods

        private SocialMediaAccount GetSocialMediaAccount()
        {
            return new SocialMediaAccount
            {
                Type = "Twitter"
            };
        }

        #endregion
    }
}
