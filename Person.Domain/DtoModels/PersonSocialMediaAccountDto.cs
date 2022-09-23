namespace PersonService.Domain.DtoModels
{
    /// <summary>
    /// Data transfer object class for person social media account
    /// </summary>
    public class PersonSocialMediaAccountDto
    {
        #region Public properties

        public Guid SocialMediaAccountId { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonSocialMediaAccountDto"/> class.
        /// </summary>
        public PersonSocialMediaAccountDto()
        {
        }

        #endregion
    }
}
