using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Queries;
using static PersonService.API.Constants.RouteConstants;
using PersonService.API.Helpers;
using PersonService.Domain.Exceptions;

namespace PersonService.API.Controllers
{
    /// <summary>
    /// Controller handling the Person data
    /// </summary>
    [ApiController]
    [Route(ApiRoute)]
    public class SocialMediaAccountsController : ControllerBase
    {
        #region Private fields

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SocialMediaAccountsController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialMediaAccountsController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator instance</param>
        /// <param name="mapper">Mapper instance</param>
        /// <param name="logger">Logger instance</param>
        public SocialMediaAccountsController(IMediator mediator, IMapper mapper, ILogger<SocialMediaAccountsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Correlation id for the concrete request
        /// </summary>
        public Guid CorrelationId
        {
            get
            {
                return Request?.Headers?.GetCorrelationId() ?? Guid.Empty;
            }
        }

        #endregion

        #region Endpoints implementation

        /// <summary>
        /// Retrieving all social media account entities
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of all social media accounts entities</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SocialMediaAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SocialMediaAccountDto>> GetSocialMediaAccountsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"[{nameof(SocialMediaAccountsController)}][{nameof(GetSocialMediaAccountsAsync)} | {CorrelationId}] Initiating {nameof(GetSocialMediaAccounts)} command...");

                var socialMediaAccountList = await _mediator.Send(new GetSocialMediaAccounts(CorrelationId), cancellationToken);

                if (!socialMediaAccountList?.Any() ?? true
                )
                {
                    _logger.LogError($"[{nameof(SocialMediaAccountsController)}][{nameof(GetSocialMediaAccountsAsync)} | {CorrelationId}] {nameof(GetSocialMediaAccounts)} request failed because no data was found.");
                    return NotFound($"No social media account was found.");
                }

                var retVal = socialMediaAccountList?.Select(socialMediaAccount => _mapper.Map<SocialMediaAccountDto>(socialMediaAccount));

                _logger.LogInformation($"[{nameof(SocialMediaAccountsController)}][{nameof(GetSocialMediaAccountsAsync)} | {CorrelationId}] {nameof(GetSocialMediaAccounts)} command was successfully executed.");

                return Ok(retVal);
            }
            catch (OperationFailedException ex)
            {
                _logger.LogError(ex, $"[{nameof(SocialMediaAccountsController)}][{nameof(GetSocialMediaAccountsAsync)} | {CorrelationId}] An error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}