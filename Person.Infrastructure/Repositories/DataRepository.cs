using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Interfaces;
using PersonService.Infrastructure.Contexts;

namespace PersonService.Infrastructure.Repositories
{
    /// <summary>
    /// Class that contains the db operations logic for the provided entity
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    public class DataRepository<T> : IDataRepository<T>, IDisposable where T : class
    {
        #region Private fields

        private readonly PersonDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        private bool _disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">Person db context</param>
        /// <exception cref="ArgumentNullException">Thrown if person db context is null</exception>
        public DataRepository(PersonDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = dbContext.Set<T>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes an operation for adding a new entity for the provided type
        /// </summary>
        /// <param name="entity">Entity that should be added to database</param>
        /// <returns>Added entity data</returns>
        /// <exception cref="ArgumentNullException">Thrown if entity is null</exception>
        public virtual async Task<T> AddAsync(
            T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbSet.AddAsync(entity);

            return entity;
        }

        /// <summary>
        /// Executes operation for getting the concrete entity for the provided type
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Entity for the provided id</returns>
        public virtual async Task<T> GetByIdAsync(Guid entityId)
        {
            return await _dbSet.FindAsync(entityId);
        }

        /// <summary>
        /// Executes get operation for provided entity type
        /// </summary>
        /// <returns>All items for the desired entity type</returns>
        public virtual async Task<IList<T>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Executes save changes operation on the database
        /// </summary>
        public async Task SaveChangesAsync()
        {
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
                _dbContext.Dispose();
            }
            _disposed = true;
        }

        #endregion
    }
}
