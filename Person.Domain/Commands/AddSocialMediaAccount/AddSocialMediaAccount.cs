using PersonService.Domain.Models;

namespace PersonService.Domain.Commands
{
    /// <summary>
    /// Add social media account command definition
    /// </summary>
    public class AddSocialMediaAccount : Command<Guid>
    {
        #region Public properties

        public SocialMediaAccount SocialMediaAccount { get; set; }

        #endregion

        #region Constructor

        public AddSocialMediaAccount(
            Guid correlationId,
            SocialMediaAccount socialMediaAccount)
        : base(correlationId)
        {
            SocialMediaAccount = socialMediaAccount;
        }

        #endregion
    }
}
