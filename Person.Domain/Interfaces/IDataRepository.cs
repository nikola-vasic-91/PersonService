using System.Threading;

namespace PersonService.Domain.Interfaces
{
    /// <summary>
    /// Class that contains the db operations logic for the provided entity
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    public interface IDataRepository<T> where T : class
    {
        /// <summary>
        /// Executes an operation for adding a new entity for the provided type
        /// </summary>
        /// <param name="entity">Entity that should be added to database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Added entity data</returns>
        /// <exception cref="ArgumentNullException">Thrown if entity is null</exception>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes get operation for provided entity type
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>All items for the desired entity type</returns>
        Task<IList<T>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes operation for getting the concrete entity for the provided type
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entity for the provided id</returns>
        Task<T> GetByIdAsync(Guid entityId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes save changes operation on the database
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
