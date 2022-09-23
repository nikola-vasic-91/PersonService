namespace PersonService.Domain.Models
{
    /// <summary>
    /// Person model class
    /// </summary>
    public class Person
    {
        #region Public properties

        public Guid PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<SocialSkill> SocialSkills { get; set; }
        public virtual ICollection<PersonSocialMediaAccount> PersonSocialMediaAccounts { get; set; }

        #endregion
    }
}
