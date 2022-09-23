using Xunit;
using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Models;
using PersonService.Infrastructure.Contexts;
using PersonService.Infrastructure.Repositories;

namespace PersonService.Tests.UnitTests.Infrastructure.Repositories
{
    public class PersonRepositoryTests
    {
        #region Private fields

        private readonly PersonDbContext _context;
        private readonly PersonRepository _repository;
        private readonly Person _testPerson;
        private readonly Person _testPerson2;

        #endregion

        #region Constructor

        public PersonRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<PersonDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            _context = new PersonDbContext(dbOptions.Options);
            _repository = new PersonRepository(_context);

            _testPerson = new Person
            {
                PersonId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                FirstName = "John",
                LastName = "Smith",
                SocialSkills = new List<SocialSkill>
                {
                    new SocialSkill
                    {
                        Name = "Team player"
                    }
                },
                PersonSocialMediaAccounts = new List<PersonSocialMediaAccount>
                {
                    new PersonSocialMediaAccount
                    {
                        Address = "www.facebook.com",
                        SocialMediaAccount = new SocialMediaAccount
                        {
                            Type = "Facebook"
                        }
                    }
                }
                
            };

            _testPerson2 = new Person
            {
                PersonId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                FirstName = "Mike",
                LastName = "Jones",
                SocialSkills = new List<SocialSkill>
                {
                    new SocialSkill
                    {
                        Name = "Team player"
                    }
                },
                PersonSocialMediaAccounts = new List<PersonSocialMediaAccount>
                {
                    new PersonSocialMediaAccount
                    {
                        Address = "www.twitter.com",
                        SocialMediaAccount = new SocialMediaAccount
                        {
                            Type = "Twitter"
                        }
                    }
                }

            };


            _context.Persons.AddRange(_testPerson, _testPerson2);
            _context.SaveChanges();
        }

        #endregion

        #region Tests

        [Fact]
        public async Task AddAsync_Success()
        {
            //Arrange
            var testPerson = new Person
            {
                PersonId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                FirstName = "William",
                LastName = "Tuple",
                SocialSkills = new List<SocialSkill>
                {
                    new SocialSkill
                    {
                        Name = "Team player"
                    }
                },
                PersonSocialMediaAccounts = new List<PersonSocialMediaAccount>
                {
                    new PersonSocialMediaAccount
                    {
                        Address = "www.linkedin.com",
                        SocialMediaAccount = new SocialMediaAccount
                        {
                            Type = "Linkedin"
                        }
                    }
                }

            };

            //Act
            await _repository.AddAsync(testPerson);
            await _repository.SaveChangesAsync();

            //Assert
            var result = await _repository.GetByIdAsync(testPerson.PersonId);

            Assert.NotNull(result);
            Assert.Equal(testPerson.FirstName, result.FirstName);
            Assert.Equal(testPerson.LastName, result.LastName);
            Assert.Equal(testPerson.SocialSkills.First(), result.SocialSkills.First());
            Assert.Equal(testPerson.PersonSocialMediaAccounts.First().Address, result.PersonSocialMediaAccounts.First().Address);
        }

        [Fact]
        public async Task AddAsync_EntityIsNull_ArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddAsync(default(Person)));
        }

        [Fact]
        public async Task GetByIdAsync_ExistingData_Success()
        {
            //Arrange
            //Act
            var result = await _repository.GetByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_testPerson.FirstName, result.FirstName);
            Assert.Equal(_testPerson.LastName, result.LastName);
            Assert.Equal(_testPerson.SocialSkills.First(), result.SocialSkills.First());
            Assert.Equal(_testPerson.PersonSocialMediaAccounts.First().Address, result.PersonSocialMediaAccounts.First().Address);
        }

        [Fact]
        public async Task GetAsync_ExistingData_Success()
        {
            //Arrange
            //Act
            var result = await _repository.GetAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        #endregion
    }
}
