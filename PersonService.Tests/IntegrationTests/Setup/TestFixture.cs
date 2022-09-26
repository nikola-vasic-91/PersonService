using Microsoft.Extensions.Configuration;
using FluentValidation;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Validators;
using PersonService.Infrastructure.Contexts;
using PersonService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using PersonService.Domain.Modules;
using PersonService.Service.Modules;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using PersonService.Domain.Options;
using Microsoft.Extensions.Options;
using PersonService.API.Helpers;

namespace PersonService.Tests.IntegrationTests.Setup
{
    /// <summary>
    /// Integration Test Collection
    /// </summary>
    [CollectionDefinition("IntegrationTest Collection")]
    public class IntegrationTestCollection : ICollectionFixture<TestFixture>
    {
    }

    /// <summary>
    /// Test fixture class
    /// </summary>
    public class TestFixture : IDisposable
    {
        #region Private fields

        private bool _disposed;
        private PersonDbContext _dbContext;

        #endregion

        #region Constructor

        public TestFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbConnectionString = string.Empty;

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.Configure<DatabaseOptions>(config.GetSection(nameof(DatabaseOptions)));
                        services.Configure<ServiceUrls>(config.GetSection(nameof(ServiceUrls)));
                        services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
                        services.AddScoped(typeof(IDataRepository<Person>), typeof(PersonRepository));
                        services.AddScoped<IValidator<PersonDto>, PersonValidator>();
                        services.AddDbContext<PersonDbContext>((serviceProvider, options) =>
                                       options.UseSqlServer(
                                            new SqlConnection(serviceProvider.GetService<IOptions<DatabaseOptions>>()?.Value?.ConnectionString)));

                        services.RegisterMediatR();

                        var mapperConfig = new MapperConfiguration(mc =>
                        {
                            mc.AddProfile(new MappingProfile());
                        });

                        IMapper mapper = mapperConfig.CreateMapper();
                        services.AddSingleton(mapper);

                        services.AddMvc(options =>
                        {
                            options.Filters.Add<OperationCancelledExceptionFilter>();
                        });

                        dbConnectionString = services.BuildServiceProvider().GetService<IOptions<DatabaseOptions>>()?.Value?.ConnectionString;
                    });
                });

            Client = application.CreateClient();

            _dbContext = new PersonDbContext(new DbContextOptionsBuilder().UseSqlServer(dbConnectionString).Options);
        }

        #endregion

        #region Public properties

        public HttpClient Client { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; } = new HttpResponseMessage();

        #endregion

        #region Public methods

        /// <summary>
        /// Delete test entities created in integration tests
        /// </summary>
        /// <param name="personId">Person id</param>
        /// <param name="socialMediaAccountType">Social media account type</param>
        public async Task DeleteTestEntityAsync(Guid personId, string socialMediaAccountType)
        {
            var person = await _dbContext.Persons.FindAsync(personId);
            _dbContext.Persons.Remove(person);

            var socialMediaAccount = _dbContext.SocialMediaAccounts.Where(sma => sma.Type == socialMediaAccountType).ToList().First();
            _dbContext.SocialMediaAccounts.Remove(socialMediaAccount);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Executes dispose operation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private methods

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Client?.Dispose();
                ResponseMessage?.Dispose();
            }
            _disposed = true;
        }

        #endregion
    }
}