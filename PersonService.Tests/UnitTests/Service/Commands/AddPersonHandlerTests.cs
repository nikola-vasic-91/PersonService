using Xunit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Domain.Commands;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Service.Commands;

namespace PersonService.Tests.UnitTests.Service.Commands
{
    public class AddPersonHandlerTests
    {
        #region Private fields

        private const string AddPersonSuccessId = "288cb352-31cc-427c-bfa5-f6930d227c3f";

        private readonly IDataRepository<Person> _repository;
        private readonly ILogger<AddPersonHandler> _logger;
        private readonly IMediator _mediator;
        private AddPersonHandler _handler;

        #endregion

        #region Constructor

        public AddPersonHandlerTests()
        {
            var logMock = new Mock<ILogger<AddPersonHandler>>();
            _logger = logMock.Object;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<AddSocialMediaAccount>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            _mediator = mediatorMock.Object;

            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Person>()))
                .ReturnsAsync(new Person { PersonId = Guid.Parse(AddPersonSuccessId) });
            _repository = repositoryMock.Object;

            _handler = new AddPersonHandler(_repository, _mediator, _logger);
        }

        #endregion

        #region Tests

        [Fact]
        public async Task Handle_AddPersonRequestNull_Fail()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _handler.Handle(default(AddPerson), new CancellationToken()));
        }

        [Fact]
        public async Task Handle_AddPerson_Success()
        {
            //Arrange
            //Act
            var result = await _handler.Handle(GetAddPerson(), new CancellationToken());

            //Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(Guid.Parse(AddPersonSuccessId), result);
        }

        [Fact]
        public async Task Handle_RepositoryFail_Exception()
        {
            var repositoryMock = new Mock<IDataRepository<Person>>();
            repositoryMock.Setup(x => x.SaveChangesAsync())
                .ThrowsAsync(new Exception());

            _handler = new AddPersonHandler(repositoryMock.Object, _mediator, _logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(GetAddPerson(), new CancellationToken()));
        }

        #endregion

        #region Private methods

        private AddPerson GetAddPerson()
        {
            return new AddPerson(Guid.NewGuid(), GetPerson());
        }

        private Person GetPerson(string firstName = "John", string lastName = "Smith", string smaType = "Facebook", string smaAddress = "www.facebook.com", string ssName = "Team player")
        {
            return new Person
            {
                FirstName = firstName,
                LastName = lastName,
                PersonSocialMediaAccounts = new List<PersonSocialMediaAccount>
                {
                    new PersonSocialMediaAccount
                    {
                        Address = smaAddress,
                        SocialMediaAccount = new SocialMediaAccount
                        {
                            Type = smaType
                        }
                    }
                },
                SocialSkills = new List<SocialSkill>
                {
                    new SocialSkill
                    {
                        Name = ssName
                    }
                }
            };
        }

        #endregion
    }
}
