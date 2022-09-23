namespace PersonService.Domain.Models
{
    /// <summary>
    /// Social media account model class
    /// </summary>
    public class SocialMediaAccount
    {
        #region Public properties

        public Guid SocialMediaAccountId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<PersonSocialMediaAccount> PersonSocialMediaAccounts { get; set; }

        #endregion
    }
}
