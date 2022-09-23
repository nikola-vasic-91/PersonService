using Xunit;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.API.Controllers;
using PersonService.Domain.Commands;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Models;
using PersonService.Domain.Modules;
using PersonService.Domain.Queries;
using PersonService.Domain.Validators;

namespace PersonService.Tests.UnitTests.API
{
    public class PersonsControllerTests
    {
        #region Private fields

        private const string AddPersonSuccessId = "288cb352-31cc-427c-bfa5-f6930d227c3f";

        private readonly ILogger<PersonsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly PersonValidator _validator;
        private PersonsController _controller;

        #endregion

        #region Constructor

        public PersonsControllerTests()
        {
            var logMock = new Mock<ILogger<PersonsController>>();
            _logger = logMock.Object;

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<AddPerson>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.Parse(AddPersonSuccessId));
            mediatorMock.Setup(x => x.Send(It.IsAny<GetPersons>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Person> { GetPerson() });
            mediatorMock.Setup(x => x.Send(It.IsAny<GetPerson>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPerson());
            _mediator = mediatorMock.Object;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfiguration.CreateMapper();

            _validator = new PersonValidator();

            _controller = new PersonsController(_mediator, _mapper, _logger, _validator);
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData(null, "Smith", "Facebook", "www.facebook.com", "Team player", "First name cannot be null, First name must have a value")]
        [InlineData("John", null, "Facebook", "www.facebook.com", "Team player", "Last name cannot be null, Last name must have a value")]
        [InlineData("John", "Smith", null, "www.facebook.com", "Team player", "Social media account types must have a value")]
        [InlineData("John", "Smith", "Facebook", null, "Team player", "Social media account addresses must have a value")]
        [InlineData("John", "Smith", "Facebook", "www.facebook.com", null, "All social skills must have a value")]
        public async Task AddPersonAsync_InvalidPersonData_BadRequest(string firstName, string lastName, string smaType, string smaAddress, string ssName, string message)
        {
            // Arrange
            var person = GetPersonDto(firstName, lastName, smaType, smaAddress, ssName);

            // Act
            var response = await _controller.AddPersonAsync(person);

            // Assert
            var result = response.Result as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result?.StatusCode);
            Assert.Equal(message, result?.Value);
        }

        [Fact]
        public async Task AddPersonAsync_AddPersonCommandFail_InternalServerError()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<AddPerson>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            _controller = new PersonsController(mediatorMock.Object, _mapper, _logger, _validator);

            var person = GetPersonDto();

            // Act
            var response = await _controller.AddPersonAsync(person);

            // Assert
            var result = response.Result as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(500, result?.StatusCode);
        }

        [Fact]
        public async Task AddPersonAsync_AddPersonCommand_Success()
        {
            // Arrange
            var person = GetPersonDto();

            // Act
            var response = await _controller.AddPersonAsync(person);

            // Assert
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<Guid>(result?.Value);
            Assert.Equal(Guid.Parse(AddPersonSuccessId), result?.Value);
        }

        [Fact]
        public async Task GetPersonsAsync_GetPersonsQuery_Success()
        {
            // Arrange
            // Act
            var response = await _controller.GetPersonsAsync();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<PersonDto>>(result?.Value);
            Assert.NotEmpty(result?.Value as IEnumerable<PersonDto>);
        }

        [Fact]
        public async Task GetPersonsAsync_GetPersonsQueryFail_InternalServerError()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetPersons>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            _controller = new PersonsController(mediatorMock.Object, _mapper, _logger, _validator);

            // Act
            var response = await _controller.GetPersonsAsync();

            // Assert
            var result = response.Result as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(500, result?.StatusCode);
        }

        [Fact]
        public async Task GetPersonsAsync_GetPersonsQuery_NoPersonsFound_NotFound()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetPersons>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Person>());
            _controller = new PersonsController(mediatorMock.Object, _mapper, _logger, _validator);

            // Act
            var response = await _controller.GetPersonsAsync();

            // Assert
            var result = response.Result as NotFoundObjectResult;
            Assert.Equal(404, result?.StatusCode);
            Assert.Equal("No person was found.", result?.Value);
        }

        [Fact]
        public async Task GetPersonByIdAsync_GetPersonQuery_Success()
        {
            // Arrange
            // Act
            var response = await _controller.GetPersonByIdAsync(Guid.NewGuid());

            // Assert
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<PersonDto>(result?.Value);
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task GetModifiedPersonDataByIdAsync_GetPersonQuery_Success()
        {
            // Arrange
            // Act
            var response = await _controller.GetModifiedPersonDataByIdAsync(Guid.NewGuid());

            // Assert
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<ModifiedPersonDataDto>(result?.Value);
            Assert.NotNull(result?.Value);
        }

        #endregion

        #region Private methods

        private PersonDto GetPersonDto(string firstName = "John", string lastName = "Smith", string smaType = "Facebook", string smaAddress = "www.facebook.com", string ssName = "Team player")
        {
            return new PersonDto
            {
                FirstName = firstName,
                LastName = lastName,
                SocialMediaAccounts = new List<PersonSocialMediaAccountDto>
                {
                    new PersonSocialMediaAccountDto
                    {
                        Type = smaType,
                        Address = smaAddress
                    }
                },
                SocialSkills = new List<string>
                {
                    ssName
                }
            };
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
