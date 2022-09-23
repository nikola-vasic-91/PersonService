using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Models;
using PersonService.Infrastructure.Contexts;

namespace PersonService.Infrastructure.Repositories
{
    /// <summary>
    /// Class that contains the db operations logic for the person entity
    /// </summary>
    public class PersonRepository : DataRepository<Person>
    {
        #region Private methods

        private readonly PersonDbContext _dbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Person db context</param>
        public PersonRepository(PersonDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes operation for getting the concrete person entity
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Entity for the provided id</returns>
        public override async Task<Person> GetByIdAsync(Guid personId) => 
            await _dbContext
                .Persons
            .Include(p => p.PersonSocialMediaAccounts)
            .ThenInclude(psma => psma.SocialMediaAccount)
            .Include(p => p.SocialSkills)
                .FirstOrDefaultAsync(c =>
                    c.PersonId == personId
                );

        /// <summary>
        /// Executes get operation for the person entity
        /// </summary>
        /// <returns>All items for the person entity</returns>
        public override async Task<IList<Person>> GetAsync() =>
            await _dbContext
                .Persons
                .Include(p => p.PersonSocialMediaAccounts)
                .ThenInclude(psma => psma.SocialMediaAccount)
                .Include(p => p.SocialSkills)
                .ToListAsync();

        #endregion
    }
}
