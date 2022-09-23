using MediatR;
using Microsoft.Extensions.Logging;
using PersonService.Domain.Commands;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;

namespace PersonService.Service.Commands
{
    /// <summary>
    /// Class that contains the social media account adding logic
    /// </summary>
    public class AddSocialMediaAccountHandler : IRequestHandler<AddSocialMediaAccount, Guid>
    {
        #region Private fields

        private readonly IDataRepository<SocialMediaAccount> _repository;
        private readonly ILogger<AddSocialMediaAccountHandler> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSocialMediaAccountHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">Logger instance</param>
        public AddSocialMediaAccountHandler(
            IDataRepository<SocialMediaAccount> repository,
            ILogger<AddSocialMediaAccountHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles a request for adding a new social media account
        /// </summary>
        /// <param name="request">The request for adding a new social media account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Id of the added social media account</returns>
        public async Task<Guid> Handle(AddSocialMediaAccount request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError($"[{nameof(AddSocialMediaAccountHandler)}][{nameof(Handle)}] Request with type {nameof(AddSocialMediaAccount)} is null.");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogError($"[{nameof(AddSocialMediaAccountHandler)}][{nameof(Handle)} | {request.CorrelationId}] Adding a new social media account to the database...");

            var result = await _repository.AddAsync(request.SocialMediaAccount);

            return result.SocialMediaAccountId;
        }

        #endregion
    }
}
