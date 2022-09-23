using Newtonsoft.Json;
using PersonService.Domain.DtoModels;
using PersonService.Tests.IntegrationTests.Setup;
using System.Net;
using Xunit;
using Xbehave;
using static PersonService.Tests.IntegrationTests.Helpers.TestHelper;
using PersonService.Domain.Models;

namespace PersonService.Tests.IntegrationTests.API
{
    /// <summary>
    /// Persons integration tests
    /// </summary>
    [Collection("IntegrationTest Collection")]
    public class PersonFeature
    {
        #region Private fields

        private readonly TestFixture _testContext;
        private string personsEndpoint = "api/Persons";
        private string modifiedUrlPart = "modified";

        #endregion

        #region Constructor

        public PersonFeature(TestFixture testFixture) => _testContext = testFixture;

        #endregion

        #region Tests

        [Scenario]
        public void AddNewPerson_Success()
        {
            PersonDto newPerson = default;
            Guid personId = Guid.Empty;
            string socialMediaAccountType = $"TestType-{Guid.NewGuid()}";

            "Given I want to add a new person"
                .x(() =>
                {
                    newPerson = new PersonDto
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        SocialSkills = new List<string>
                        {
                            "Team player"
                        },
                                SocialMediaAccounts = new List<PersonSocialMediaAccountDto>
                        {
                            new PersonSocialMediaAccountDto
                            {
                                Address = "www.newsma.com",
                                Type = socialMediaAccountType
                            }
                        }
                    };
                });

            "When I add a new person"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.PostAsync(personsEndpoint, ConvertToByte(newPerson));
                });

            "Then person is successfully added"
                 .x(async () =>
                 {
                     _testContext.ResponseMessage.EnsureSuccessStatusCode();
                     personId = JsonConvert.DeserializeObject<Guid>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());

                     Assert.IsType<Guid>(personId);
                 })
                 .Teardown(async () =>
                 {
                     await _testContext.DeleteTestEntityAsync(personId, socialMediaAccountType);
                 });
        }

        [Scenario]
        public void AddNewPerson_InvalidData_BadRequest()
        {
            PersonDto newPerson = default;
            string socialMediaAccountType = $"TestType-{Guid.NewGuid()}";

            "Given I want to add a new person with invalid first name and last name data"
                .x(() =>
                {
                    newPerson = new PersonDto
                    {
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        SocialSkills = new List<string>
                        {
                            "Team player"
                        },
                        SocialMediaAccounts = new List<PersonSocialMediaAccountDto>
                        {
                            new PersonSocialMediaAccountDto
                            {
                                Address = "www.newsma.com",
                                Type = socialMediaAccountType
                            }
                        }
                    };
                });

            "When I add a new person"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.PostAsync(personsEndpoint, ConvertToByte(newPerson));
                });

            "Then bad request status code is returned"
                 .x(() =>
                 {
                     Assert.Equal(HttpStatusCode.BadRequest, _testContext.ResponseMessage.StatusCode);
                 });
        }


        [Scenario]
        public void GetAllPersons_Success()
        {
            PersonDto newPerson = default;
            List<PersonDto> persons;

            "Given I want to get all persons"
                .x(() =>
                {
                });

            "When I get all persons"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync(personsEndpoint);
                });

            "Then persons all successfully retrieved"
                 .x(async () =>
                 {
                     _testContext.ResponseMessage.EnsureSuccessStatusCode();
                     persons = JsonConvert.DeserializeObject<List<PersonDto>>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());

                     Assert.NotNull(persons);
                     Assert.NotEmpty(persons);
                 });
        }

        [Scenario]
        public void GetPersonById_Success()
        {
            PersonDto newPerson = default;
            Guid personId = Guid.Empty;
            string socialMediaAccountType = $"TestType-{Guid.NewGuid()}";

            "Given I want to get a person by id"
                .x(async () =>
                {
                    newPerson = new PersonDto
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        SocialSkills = new List<string>
                        {
                            "Team player"
                        },
                        SocialMediaAccounts = new List<PersonSocialMediaAccountDto>
                        {
                            new PersonSocialMediaAccountDto
                            {
                                Address = "www.newsma.com",
                                Type = socialMediaAccountType
                            }
                        }
                    };

                    _testContext.ResponseMessage = await _testContext.Client.PostAsync(personsEndpoint, ConvertToByte(newPerson));
                    _testContext.ResponseMessage.EnsureSuccessStatusCode();
                    personId = JsonConvert.DeserializeObject<Guid>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());
                });

            "When I get person by id"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync($"{personsEndpoint}/{personId}");
                });

            "Then person is successfully retrieved"
                 .x(async () =>
                 {
                     _testContext.ResponseMessage.EnsureSuccessStatusCode();
                     var person = JsonConvert.DeserializeObject<PersonDto>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());

                     Assert.NotNull(person);
                     Assert.Equal(newPerson.FirstName, person.FirstName);
                     Assert.Equal(newPerson.SocialMediaAccounts.First().Address, person.SocialMediaAccounts.First().Address);
                 })
                 .Teardown(async () =>
                 {
                     await _testContext.DeleteTestEntityAsync(personId, socialMediaAccountType);
                 });
        }

        [Scenario]
        public void GetPersonById_UnexistingId_NotFound()
        {
            Guid unexistingPersonId = Guid.NewGuid();

            "Given I want to get a person by unexisting id"
                .x( () =>
                {
                });

            "When I get person by unexisting id"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync($"{personsEndpoint}/{unexistingPersonId}");
                });

            "Then not found status code is returned"
                 .x(() =>
                 {
                     Assert.Equal(HttpStatusCode.NotFound, _testContext.ResponseMessage.StatusCode);
                 });
        }

        [Scenario]
        public void GetModifiedDataById_Success()
        {
            PersonDto newPerson = default;
            Guid personId = Guid.Empty;
            string socialMediaAccountType = $"TestType-{Guid.NewGuid()}";

            "Given I want to get person modified data by id"
                .x(async () =>
                {
                    newPerson = new PersonDto
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        SocialSkills = new List<string>
                        {
                            "Team player"
                        },
                        SocialMediaAccounts = new List<PersonSocialMediaAccountDto>
                        {
                            new PersonSocialMediaAccountDto
                            {
                                Address = "www.newsma.com",
                                Type = socialMediaAccountType
                            }
                        }
                    };

                    _testContext.ResponseMessage = await _testContext.Client.PostAsync(personsEndpoint, ConvertToByte(newPerson));
                    _testContext.ResponseMessage.EnsureSuccessStatusCode();
                    personId = JsonConvert.DeserializeObject<Guid>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());
                });

            "When I get person modified data by id"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync($"{personsEndpoint}/{personId}/{modifiedUrlPart}");
                });

            "Then person modified data is successfully retrieved"
                 .x(async () =>
                 {
                     _testContext.ResponseMessage.EnsureSuccessStatusCode();
                     var modifiedData = JsonConvert.DeserializeObject<ModifiedPersonDataDto>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());

                     Assert.NotNull(modifiedData);
                     Assert.Contains(newPerson.FirstName, modifiedData.FullName);
                     Assert.Equal("htimS nhoJ", modifiedData.ReversedName);
                 })
                 .Teardown(async () =>
                 {
                     await _testContext.DeleteTestEntityAsync(personId, socialMediaAccountType);
                 });
        }

        [Scenario]
        public void GetModifiedDataById_UnexistingId_NotFound()
        {
            Guid unexistingPersonId = Guid.NewGuid();

            "Given I want to get person modified data by unexisting id"
                .x(() =>
                {
                });

            "When I get person modified data by unexisting id"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync($"{personsEndpoint}/{unexistingPersonId}");
                });

            "Then not found status code is returned"
                 .x(() =>
                 {
                     Assert.Equal(HttpStatusCode.NotFound, _testContext.ResponseMessage.StatusCode);
                 });
        }

        #endregion
    }
}