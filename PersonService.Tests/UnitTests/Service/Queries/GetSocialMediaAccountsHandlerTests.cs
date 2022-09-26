using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;
using PersonService.Service.Queries;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.UnitTests.Service.Queries
{
    public class GetSocialMediaAccountsHandlerTests
    {
        #region Private fields

        private readonly IDataRepository<SocialMediaAccount> _repository;
        private readonly ILogger<GetSocialMediaAccountsHandler> _logger;
        private GetSocialMediaAccountsHandler _handler;

        #endregion

        #region Constructor

        public GetSocialMediaAccountsHandlerTests()
        {
            var logMock = new Mock<ILogger<GetSocialMediaAccountsHandler>>();
            _logger = logMock.Object;

            var repositoryMock = new Mock<IDataRepository<SocialMediaAccount>>();
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SocialMediaAccount>
                {
                    new SocialMediaAccount()
                });
            _repository = repositoryMock.Object;

            _handler = new GetSocialMediaAccountsHandler(_repository, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Handle_GetPersons_Success()
        {
            //Arrange
            //Act
            var result = await _handler.Handle(new GetSocialMediaAccounts(Guid.NewGuid()), new CancellationToken());

            //Assert
            Assert.IsType<List<SocialMediaAccount>>(result);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Handle_RepositoryFail_Exception()
        {
            //Arrange
            var repositoryMock = new Mock<IDataRepository<SocialMediaAccount>>();
            repositoryMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationFailedException());

            _handler = new GetSocialMediaAccountsHandler(repositoryMock.Object, _logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<OperationFailedException>(async () => await _handler.Handle(new GetSocialMediaAccounts(Guid.NewGuid()), new CancellationToken()));
        }

        #endregion
    }
}
