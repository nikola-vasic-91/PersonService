using MediatR;
using Microsoft.Extensions.Logging;
using PersonService.Domain.Commands;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;

namespace PersonService.Service.Commands
{
    /// <summary>
    /// Class that contains the person adding logic
    /// </summary>
    public class AddPersonHandler : IRequestHandler<AddPerson, Guid>
    {
        #region Private fields

        private readonly IDataRepository<Person> _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<AddPersonHandler> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPersonHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="mediator">Mediator instance</param>
        /// <param name="logger">Logger instance</param>
        public AddPersonHandler(
            IDataRepository<Person> repository,
            IMediator mediator,
            ILogger<AddPersonHandler> logger)
        {
            _repository = repository;
            _mediator = mediator;
            _logger = logger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles a request for adding a new person
        /// </summary>
        /// <param name="request">The request for adding a new person</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Id of the added person</returns>
        public async Task<Guid> Handle(AddPerson request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogError($"[{nameof(AddPersonHandler)}][{nameof(Handle)}] Request with type {nameof(AddPerson)} is null.");
                    throw new ArgumentNullException(nameof(request));
                }

                await AddNewSocialMediaAccountAsync(request);

                _logger.LogError($"[{nameof(AddPersonHandler)}][{nameof(Handle)} | {request.CorrelationId}] Adding a new person to the database...");

                var retVal = await _repository.AddAsync(request.Person, cancellationToken);

                await _repository.SaveChangesAsync(cancellationToken);

                _logger.LogError($"[{nameof(AddPersonHandler)}][{nameof(Handle)} | {request.CorrelationId}] Database operations were successfully completed.");

                return retVal.PersonId;
            }
            catch (Exception)
            {
                _logger.LogError($"[{nameof(AddPersonHandler)}][{nameof(Handle)}" +
                    $"{(request != null ? $"| {request.CorrelationId}" : string.Empty)}" +
                    $"] An error occurred while handling {nameof(AddPerson)} request.");
                throw;
            }
        }

        #endregion

        #region Private methods

        private async Task AddNewSocialMediaAccountAsync(AddPerson request)
        {
            if (request.Person.PersonSocialMediaAccounts.Any(psma => psma.SocialMediaAccountId == Guid.Empty))
            {
                request.Person.PersonSocialMediaAccounts.Where(psma => psma.SocialMediaAccountId == Guid.Empty).ToList().ForEach(async psma =>
                    psma.SocialMediaAccountId = await _mediator.Send(new AddSocialMediaAccount(request.CorrelationId, psma.SocialMediaAccount)));
            }
        }

        #endregion
    }
}
