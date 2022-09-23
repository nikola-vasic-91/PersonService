using PersonService.Domain.Models;

namespace PersonService.Domain.Queries
{
    /// <summary>
    /// Definition for the get persons query
    /// </summary>
    public class GetPersons : IQuery<IList<Person>>
    {
        #region Private fields

        private Guid _correlationId;

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
        /// Initializes a new instance of the <see cref="GetPersons"/> class.
        /// </summary>
        /// <param name="correlationId">Correlation id</param>
        public GetPersons(Guid correlationId)
        {
            _correlationId = correlationId;
        }

        #endregion
    }
}
