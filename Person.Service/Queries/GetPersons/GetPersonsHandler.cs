using MediatR;
using Microsoft.Extensions.Logging;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;

namespace PersonService.Service.Queries
{
    /// <summary>
    /// Class that contains the persons retrieval logic
    /// </summary>
    public class GetPersonsHandler : IRequestHandler<GetPersons, IList<Person>>
    {
        #region Private fields

        private readonly IDataRepository<Person> _repository;
        private readonly ILogger<GetPersonsHandler> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPersonsHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">Logger instance</param>
        public GetPersonsHandler(
            IDataRepository<Person> repository,
            ILogger<GetPersonsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the persons retrieval
        /// </summary>
        /// <param name="request">Persons retrieval request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of all persons</returns>
        public async Task<IList<Person>> Handle(GetPersons request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"[{nameof(GetPersonsHandler)}][{nameof(Handle)} | {request.CorrelationId}] Initiating database operation for {nameof(GetPersons)} command...");

                return await _repository.GetAsync(cancellationToken);
            }
            catch (Exception)
            {
                _logger.LogError($"[{nameof(GetPersonsHandler)}][{nameof(Handle)}" +
                    $"{(request != null ? $"| {request.CorrelationId}" : string.Empty)}" +
                    $"] An error occurred while handling {nameof(GetPersons)} request.");
                throw;
            }
        }

        #endregion
    }
}
