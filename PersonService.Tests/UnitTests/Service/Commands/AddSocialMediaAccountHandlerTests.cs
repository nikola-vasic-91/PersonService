using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Domain.Commands;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Service.Commands;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.UnitTests.Service.Commands
{
    public class AddSocialMediaAccountHandlerTests
    {
        #region Private fields

        private const string AddSocialMediaAccountSuccessId = "288cb352-31cc-427c-bfa5-f6930d227c3f";

        private readonly IDataRepository<SocialMediaAccount> _repository;
        private readonly ILogger<AddSocialMediaAccountHandler> _logger;
        private AddSocialMediaAccountHandler _handler;

        #endregion

        #region Constructor

        public AddSocialMediaAccountHandlerTests()
        {
            var logMock = new Mock<ILogger<AddSocialMediaAccountHandler>>();
            _logger = logMock.Object;

            var repositoryMock = new Mock<IDataRepository<SocialMediaAccount>>();
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<SocialMediaAccount>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SocialMediaAccount { SocialMediaAccountId = Guid.Parse(AddSocialMediaAccountSuccessId) });
            _repository = repositoryMock.Object;

            _handler = new AddSocialMediaAccountHandler(_repository, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Handle_AddSocialMediaAccountRequestNull_Fail()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.Handle(default(AddSocialMediaAccount), new CancellationToken()));
        }

        [Fact]
        public async Task Handle_AddSocialMediaAccount_Success()
        {
            //Arrange
            //Act
            var result = await _handler.Handle(GetAddSocialMediaAccount(), new CancellationToken());

            //Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(Guid.Parse(AddSocialMediaAccountSuccessId), result);
        }

        [Fact]
        public async Task Handle_RepositoryFail_Exception()
        {
            var repositoryMock = new Mock<IDataRepository<SocialMediaAccount>>();
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<SocialMediaAccount>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationFailedException());

            _handler = new AddSocialMediaAccountHandler(repositoryMock.Object, _logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<OperationFailedException>(async () => await _handler.Handle(GetAddSocialMediaAccount(), new CancellationToken()));
        }

        #endregion

        #region Private methods

        private AddSocialMediaAccount GetAddSocialMediaAccount()
        {
            return new AddSocialMediaAccount(Guid.NewGuid(), GetSocialMediaAccount());
        }

        private SocialMediaAccount GetSocialMediaAccount()
        {
            return new SocialMediaAccount
            {
                Type = "Facebook"
            };
        }

        #endregion
    }
}
