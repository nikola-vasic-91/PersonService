using Xunit;
using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Models;
using PersonService.Infrastructure.Contexts;
using PersonService.Infrastructure.Repositories;
using PersonService.Domain.Exceptions;

namespace PersonService.Tests.UnitTests.Infrastructure.Repositories
{
    public class DataRepositoryTests
    {
        #region Private fields

        private readonly PersonDbContext _context;
        private readonly DataRepository<SocialMediaAccount> _repository;
        private readonly SocialMediaAccount _testSocialMediaAccount;
        private readonly SocialMediaAccount _testSocialMediaAccount2;

        #endregion

        #region Constructor

        public DataRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<PersonDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            _context = new PersonDbContext(dbOptions.Options);
            _repository = new DataRepository<SocialMediaAccount>(_context);

            _testSocialMediaAccount = new SocialMediaAccount
            {
                SocialMediaAccountId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Type = "Facebook"
            };
            _testSocialMediaAccount2 = new SocialMediaAccount
            {
                SocialMediaAccountId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Type = "Twitter"
            };


            _context.SocialMediaAccounts.AddRange(_testSocialMediaAccount, _testSocialMediaAccount2);
            _context.SaveChanges();
        }

        #endregion

        #region Tests

        [Fact]
        public async Task AddAsync_Success()
        {
            //Arrange
            var testSocialMediaAccount = new SocialMediaAccount
            {
                SocialMediaAccountId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Type = "Linkedin"
            };

            //Act
            await _repository.AddAsync(testSocialMediaAccount);
            await _repository.SaveChangesAsync();

            //Assert
            var result = await _repository.GetByIdAsync(testSocialMediaAccount.SocialMediaAccountId);

            Assert.NotNull(result);
            Assert.Equal(testSocialMediaAccount.Type, result.Type);
        }

        [Fact]
        public async Task AddAsync_EntityIsNull_ArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddAsync(default(SocialMediaAccount)));
        }

        [Fact]
        public async Task AddAsync_DatabaseFail_OperationFailedException()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            //Assert
            await _repository.AddAsync(new SocialMediaAccount { SocialMediaAccountId = id });
            await Assert.ThrowsAsync<OperationFailedException>(async () => await _repository.AddAsync(new SocialMediaAccount { SocialMediaAccountId = id }));
        }

        [Fact]
        public async Task GetByIdAsync_ExistingData_Success()
        {
            //Arrange
            //Act
            var result = await _repository.GetByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_testSocialMediaAccount.Type, result.Type);
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
