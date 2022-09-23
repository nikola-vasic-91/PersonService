using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Models;
using PersonService.Domain.Queries;
using PersonService.Domain.Commands;
using static PersonService.API.Constants.RouteConstants;
using static PersonService.API.Helpers.RequestHeaderHelper;
using System;
using FluentValidation.Results;
using FluentValidation;

namespace PersonService.API.Controllers
{
    /// <summary>
    /// Controller handling the Person data
    /// </summary>
    [ApiController]
    [Route(ApiRoute)]
    public class PersonsController : ControllerBase
    {
        #region Private fields

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonsController> _logger;
        private readonly IValidator<PersonDto> _validator;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator instance</param>
        /// <param name="mapper">Mapper instance</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="validator">Validator instance</param>
        public PersonsController(
            IMediator mediator,
            IMapper mapper,
            ILogger<PersonsController> logger,
            IValidator<PersonDto> validator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
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
        /// Adding a new person
        /// </summary>
        /// <param name="personData">Person data to be added</param>
        /// <returns>Id of the newly added person</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> AddPersonAsync([FromBody] PersonDto personData)
        {
            try
            {
                ValidationResult validationResult = await _validator.ValidateAsync(personData);

                if (!validationResult.IsValid)
                {
                    _logger.LogError($"[{nameof(PersonsController)}][{nameof(AddPersonAsync)} | {CorrelationId}] {nameof(AddPerson)} request failed because of invalid person data.");
                    return BadRequest(string.Join(", ", validationResult.Errors));
                }

                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(AddPersonAsync)} | {CorrelationId}] Initiating {nameof(AddPerson)} command...");

                var personId = await _mediator.Send(new AddPerson(CorrelationId,
                    _mapper.Map<PersonDto, Person>(personData)));

                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(AddPersonAsync)} | {CorrelationId}] {nameof(AddPerson)} command was successfully executed for person with id: {personId}");

                return Ok(personId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(PersonsController)}][{nameof(AddPersonAsync)} | {CorrelationId}] An error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieving all person entities
        /// </summary>
        /// <returns>Collection of all person entities</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersonsAsync()
        {
            try
            {
                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetPersonsAsync)} | {CorrelationId}] Initiating {nameof(GetPersons)} command...");

                var personList = await _mediator.Send(new GetPersons(CorrelationId));

                if (!personList?.Any() ?? true
                )
                {
                    _logger.LogError($"[{nameof(PersonsController)}][{nameof(GetPersonsAsync)} | {CorrelationId}] {nameof(GetPersons)} request failed because no data was found.");
                    return NotFound($"No person was found.");
                }

                var retVal = personList?.Select(person => _mapper.Map<PersonDto>(person));

                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetPersonsAsync)} | {CorrelationId}] {nameof(GetPersons)} command was successfully executed.");

                return Ok(retVal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(PersonsController)}][{nameof(GetPersonsAsync)} | {CorrelationId}] An error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Getting the concrete person for the provided id
        /// </summary>
        /// <param name="personId">Person id</param>
        /// <returns>Person entity for the provided id</returns>
        [HttpGet(PersonIdEndpoint)]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDto>> GetPersonByIdAsync([FromRoute] Guid personId)
        {
            try
            {
                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetPersonByIdAsync)} | {CorrelationId}] Initiating {nameof(GetPerson)} command...");

                var result = await _mediator.Send(new GetPerson(personId, CorrelationId));

                if (result == null)
                {
                    _logger.LogError($"[{nameof(PersonsController)}][{nameof(GetPersonByIdAsync)} | {CorrelationId}] {nameof(GetPerson)} request failed because no data was found for the provided id: {personId}.");
                    return NotFound($"No person was found for the id: {personId}.");
                }

                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetPersonByIdAsync)} | {CorrelationId}] {nameof(GetPerson)} command was successfully executed.");

                return Ok(_mapper.Map<PersonDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(PersonsController)}][{nameof(GetPersonByIdAsync)} | {CorrelationId}] An error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the modified data for the provided person id
        /// </summary>
        /// <param name="personId">Person id</param>
        /// <returns>Modified data for the provided person id</returns>
        [HttpGet(PersonModifiedEndpoint)]
        [ProducesResponseType(typeof(ModifiedPersonDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModifiedPersonDataDto>> GetModifiedPersonDataByIdAsync(Guid personId)
        {
            try
            {
                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetModifiedPersonDataByIdAsync)} | {CorrelationId}] Initiating {nameof(GetPerson)} command...");

                var result = await _mediator.Send(new GetPerson(personId, CorrelationId));

                if (result == null)
                {
                    _logger.LogError($"[{nameof(PersonsController)}][{nameof(GetModifiedPersonDataByIdAsync)} | {CorrelationId}] {nameof(GetPerson)} request failed because no data was found for the provided id: {personId}.");
                    return NotFound($"No person was found for the id: {personId}.");
                }

                _logger.LogInformation($"[{nameof(PersonsController)}][{nameof(GetModifiedPersonDataByIdAsync)} | {CorrelationId}] {nameof(GetPerson)} command was successfully executed.");

                return Ok(_mapper.Map<ModifiedPersonDataDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{nameof(PersonsController)}][{nameof(GetModifiedPersonDataByIdAsync)} | {CorrelationId}] An error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}