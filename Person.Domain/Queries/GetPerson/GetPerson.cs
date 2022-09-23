using PersonService.Domain.Models;

namespace PersonService.Domain.Queries
{
    /// <summary>
    /// Definition for the get person query
    /// </summary>
    public class GetPerson : IQuery<Person>
    {
        #region Private fields

        private Guid _correlationId;

        #endregion

        #region Public properties

        public Guid PersonId { get; set; }

        public Guid CorrelationId { 
            get
            {
                return _correlationId;
            } 
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPerson"/> class.
        /// </summary>
        /// <param name="personId">Person id</param>
        /// <param name="correlationId">Correlation id</param>
        public GetPerson(Guid personId, Guid correlationId)
        {
            PersonId = personId;
            _correlationId = correlationId;
        }

        #endregion
    }
}
