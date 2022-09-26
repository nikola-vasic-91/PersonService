using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Exceptions;
using PersonService.Domain.Interfaces;
using PersonService.Infrastructure.Contexts;
using System.Reflection.Metadata;
using System.Threading;

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
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Added entity data</returns>
        /// <exception cref="ArgumentNullException">Thrown if entity is null</exception>
        public virtual async Task<T> AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                await _dbSet.AddAsync(entity, cancellationToken);

                return entity;
            }
            catch (Exception ex) when (ex is not ArgumentNullException)
            {
                throw new OperationFailedException($"[{nameof(DataRepository<T>)}][{nameof(AddAsync)}] An error occurred on adding entity of type {typeof(T)}.", ex);
            }
        }

        /// <summary>
        /// Executes operation for getting the concrete entity for the provided type
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entity for the provided id</returns>
        public virtual async Task<T> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet.FindAsync(new object[] { entityId }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new OperationFailedException($"[{nameof(DataRepository<T>)}][{nameof(GetByIdAsync)}] An error occurred on getting entity of type {typeof(T)} with id: {entityId}.", ex);
            }
        }

        /// <summary>
        /// Executes get operation for provided entity type
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>All items for the desired entity type</returns>
        public virtual async Task<IList<T>> GetAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new OperationFailedException($"[{nameof(DataRepository<T>)}][{nameof(GetAsync)}] An error occurred on getting entities of type {typeof(T)}.", ex);
            }
        }

        /// <summary>
        /// Executes save changes operation on the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new OperationFailedException($"[{nameof(DataRepository<T>)}][{nameof(SaveChangesAsync)}] An error occurred on saving database changes.", ex);
            }
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
