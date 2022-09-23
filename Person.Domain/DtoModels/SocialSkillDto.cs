namespace PersonService.Domain.DtoModels
{
    /// <summary>
    /// Data transfer object class for social skill
    /// </summary>
    public class SocialSkillDto
    {
        #region Public properties

        public Guid SocialSkillId { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialSkillDto"/> class.
        /// </summary>
        public SocialSkillDto()
        {
        }

        #endregion
    }
}
