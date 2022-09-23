namespace PersonService.Domain.DtoModels
{
    /// <summary>
    /// Data transfer object class for social media account
    /// </summary>
    public class SocialMediaAccountDto
    {
        #region Public properties

        public Guid SocialMediaAccountId { get; set; }
        public string Type { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialMediaAccountDto"/> class.
        /// </summary>
        public SocialMediaAccountDto()
        {
        }

        #endregion
    }
}
