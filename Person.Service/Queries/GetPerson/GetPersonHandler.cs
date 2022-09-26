using MediatR;
using Microsoft.Extensions.Logging;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;

namespace PersonService.Service.Queries
{
    /// <summary>
    /// Class that contains the person retrieval logic
    /// </summary>
    public class GetPersonHandler : IRequestHandler<GetPerson, Person>
    {
        #region Private fields

        private readonly IDataRepository<Person> _repository;
        private readonly ILogger<GetPersonHandler> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPersonHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">Logger instance</param>
        public GetPersonHandler(
            IDataRepository<Person> repository,
            ILogger<GetPersonHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the person retrieval
        /// </summary>
        /// <param name="request">Person retrieval request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Person for the provided id</returns>
        public async Task<Person> Handle(GetPerson request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"[{nameof(GetPersonHandler)}][{nameof(Handle)} | {request.CorrelationId}] Initiating database operation for {nameof(GetPerson)} command...");

                return await _repository.GetByIdAsync(request.PersonId, cancellationToken);
            }
            catch (Exception)
            {
                _logger.LogError($"[{nameof(GetPersonHandler)}][{nameof(Handle)}" +
                    $"{(request != null ? $"| {request.CorrelationId}" : string.Empty)}" +
                    $"] An error occurred while handling {nameof(GetPerson)} request.");
                throw;
            }
        }

        #endregion
    }
}
