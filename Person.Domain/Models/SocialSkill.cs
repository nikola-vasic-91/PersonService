namespace PersonService.Domain.Models
{
    /// <summary>
    /// Social skill model class
    /// </summary>
    public class SocialSkill
    {
        #region Public properties

        public Guid SocialSkillId { get; set; }
        public string Name { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        #endregion
    }
}
