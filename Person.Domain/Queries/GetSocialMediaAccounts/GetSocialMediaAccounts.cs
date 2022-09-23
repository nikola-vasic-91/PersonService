using PersonService.Domain.Models;

namespace PersonService.Domain.Queries
{
    /// <summary>
    /// Definition for the get social media accounts query
    /// </summary>
    public class GetSocialMediaAccounts : IQuery<IList<SocialMediaAccount>>
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
        /// Initializes a new instance of the <see cref="GetSocialMediaAccounts"/> class.
        /// </summary>
        /// <param name="correlationId">Correlation id</param>
        public GetSocialMediaAccounts(Guid correlationId)
        {
            _correlationId = correlationId;
        }

        #endregion
    }
}
