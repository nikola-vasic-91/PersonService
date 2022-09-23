using MediatR;
using Microsoft.Extensions.Logging;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;

namespace PersonService.Service.Queries
{
    /// <summary>
    /// Class that contains the social media accounts retrieval logic
    /// </summary>
    public class GetSocialMediaAccountsHandler : IRequestHandler<GetSocialMediaAccounts, IList<SocialMediaAccount>>
    {
        #region Private fields

        private readonly IDataRepository<SocialMediaAccount> _repository;
        private readonly ILogger<GetSocialMediaAccountsHandler> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSocialMediaAccountsHandler"/> class.
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="logger">Logger instance</param>
        public GetSocialMediaAccountsHandler(
            IDataRepository<SocialMediaAccount> repository,
            ILogger<GetSocialMediaAccountsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the social media accounts retrieval
        /// </summary>
        /// <param name="request">Social media account retrieval request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of all social media accounts</returns>
        public async Task<IList<SocialMediaAccount>> Handle(GetSocialMediaAccounts request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"[{nameof(GetSocialMediaAccountsHandler)}][{nameof(Handle)} | {request.CorrelationId}] Initiating database operation for {nameof(GetSocialMediaAccounts)} command...");

                return await _repository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(GetSocialMediaAccountsHandler)}][{nameof(Handle)} | {request.CorrelationId}] An error occurred while handling {nameof(GetSocialMediaAccounts)} request.");
                throw;
            }
        }

        #endregion
    }
}
