using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;
using PersonService.Service.Queries;

namespace PersonService.Tests.UnitTests.Service.Queries
{
    public class GetPersonsHandlerTests
    {
        #region Private fields

        private readonly IDataRepository<Person> _repository;
        private readonly ILogger<GetPersonsHandler> _logger;
        private GetPersonsHandler _handler;

        #endregion

        #region Constructor

        public GetPersonsHandlerTests()
        {
            var logMock = new Mock<ILogger<GetPersonsHandler>>();
            _logger = logMock.Object;

            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.GetAsync())
                .ReturnsAsync(new List<Person>
                {
                    new Person()
                });
            _repository = repositoryMock.Object;

            _handler = new GetPersonsHandler(_repository, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Handle_GetPersons_Success()
        {
            //Arrange
            //Act
            var result = await _handler.Handle(new GetPersons(Guid.NewGuid()), new CancellationToken());

            //Assert
            Assert.IsType<List<Person>>(result);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Handle_RepositoryFail_Exception()
        {
            //Arrange
            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.GetAsync())
                .ThrowsAsync(new Exception());

            _handler = new GetPersonsHandler(repositoryMock.Object, _logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(new GetPersons(Guid.NewGuid()), new CancellationToken()));
        }

        #endregion
    }
}
