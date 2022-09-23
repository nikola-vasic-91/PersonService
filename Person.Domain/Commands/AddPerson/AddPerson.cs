using PersonService.Domain.Models;

namespace PersonService.Domain.Commands
{
    /// <summary>
    /// Add person command definition
    /// </summary>
    public class AddPerson : Command<Guid>
    {
        #region Public properties

        public Person Person { get; set; }

        #endregion

        #region Constructor

        public AddPerson(
            Guid correlationId,
            Person person)
        : base(correlationId)
        {
            Person = person;
        }

        #endregion
    }
}
