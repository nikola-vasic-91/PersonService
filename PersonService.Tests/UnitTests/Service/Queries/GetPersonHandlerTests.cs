using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;
using PersonService.Service.Queries;

namespace PersonService.Tests.UnitTests.Service.Queries
{
    public class GetPersonHandlerTests
    {
        #region Private fields

        private readonly IDataRepository<Person> _repository;
        private readonly ILogger<GetPersonHandler> _logger;
        private GetPersonHandler _handler;

        #endregion

        #region Constructor

        public GetPersonHandlerTests()
        {
            var logMock = new Mock<ILogger<GetPersonHandler>>();
            _logger = logMock.Object;

            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Person());
            _repository = repositoryMock.Object;

            _handler = new GetPersonHandler(_repository, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Handle_GetPerson_Success()
        {
            //Arrange
            //Act
            var result = await _handler.Handle(new GetPerson(Guid.NewGuid(), Guid.NewGuid()), new CancellationToken());

            //Assert
            Assert.IsType<Person>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_RepositoryFail_Exception()
        {
            //Arrange
            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            _handler = new GetPersonHandler(repositoryMock.Object, _logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(new GetPerson(Guid.NewGuid(), Guid.NewGuid()), new CancellationToken()));
        }

        #endregion
    }
}
