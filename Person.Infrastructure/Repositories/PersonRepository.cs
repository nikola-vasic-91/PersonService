using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Exceptions;
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
        /// <param name="personId">Id of the entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entity for the provided id</returns>
        public override async Task<Person> GetByIdAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext
                .Persons
                .Include(p => p.PersonSocialMediaAccounts)
                .ThenInclude(psma => psma.SocialMediaAccount)
                .Include(p => p.SocialSkills)
                .FirstOrDefaultAsync(c =>
                    c.PersonId == personId, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new OperationFailedException($"[{nameof(PersonRepository)}][{nameof(GetByIdAsync)}] An error occurred on getting entity of type {nameof(Person)} with id: {personId}.", ex);
            }
        }
            

        /// <summary>
        /// Executes get operation for the person entity
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>All items for the person entity</returns>
        public override async Task<IList<Person>> GetAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext
                        .Persons
                        .Include(p => p.PersonSocialMediaAccounts)
                        .ThenInclude(psma => psma.SocialMediaAccount)
                        .Include(p => p.SocialSkills)
                        .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new OperationFailedException($"[{nameof(PersonRepository)}][{nameof(GetAsync)}] An error occurred on getting entities of type {nameof(Person)}.", ex);
            }
        }
            
        #endregion
    }
}
