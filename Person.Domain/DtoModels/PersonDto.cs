using PersonService.Domain.Models;

namespace PersonService.Domain.DtoModels
{
    /// <summary>
    /// Data transfer object class for person data
    /// </summary>
    public class PersonDto
    {
        #region Public properties

        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<string> SocialSkills { get; set; }
        public virtual List<PersonSocialMediaAccountDto> SocialMediaAccounts { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDto"/> class.
        /// </summary>
        public PersonDto()
        {
        }

        #endregion
    }
}
