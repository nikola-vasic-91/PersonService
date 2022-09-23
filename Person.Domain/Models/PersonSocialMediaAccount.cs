namespace PersonService.Domain.Models
{
    /// <summary>
    /// Class describing a relation between person and social media account
    /// </summary>
    public class PersonSocialMediaAccount
    {
        #region Public properties

        public string Address { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }
        public Guid SocialMediaAccountId { get; set; }
        public virtual SocialMediaAccount SocialMediaAccount { get; set; }

        #endregion
    }
}
