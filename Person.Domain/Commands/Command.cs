using MediatR;

namespace PersonService.Domain.Commands
{
    /// <summary>
    /// Command definition
    /// </summary>
    public abstract class Command : IRequest
    {
        #region Private fields

        private Guid _correlationId { get; set; }

        #endregion

        #region Public properties

        public Guid CorrelationId
        {
            get
            {
                return _correlationId;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// <see cref="Command"/> class constructor
        /// </summary>
        /// <param name="correlationId">Correlation id</param>
        protected Command(Guid correlationId)
        {
            _correlationId = correlationId;
        }

        #endregion
    }

    /// <summary>
    /// Command definition for the specific return type
    /// </summary>
    /// <typeparam name="T">Return entity type</typeparam>
    public abstract class Command<T> : IRequest<T>
    {
        #region Private fields

        private Guid _correlationId { get; set; }

        #endregion

        #region Public properties

        public Guid CorrelationId
        {
            get
            {
                return _correlationId;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// <see cref="Command{T}"/> class constructor
        /// </summary>
        /// <param name="correlationId">Correlation id</param>
        protected Command(Guid correlationId)
        {
            _correlationId = correlationId;
        }

        #endregion
    }
}
