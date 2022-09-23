using FluentValidation;
using PersonService.Domain.DtoModels;

namespace PersonService.Domain.Validators
{
    /// <summary>
    /// Validation class for <see cref="PersonDto"/> data
    /// </summary>
    public class PersonValidator : AbstractValidator<PersonDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonValidator"/> class.
        /// </summary>
        public PersonValidator()
        {
            string lettersPattern = "^[a-zA-Z ]*$";

            RuleFor(x => x).NotNull()
                .WithMessage($"Person cannot be null");

            RuleFor(x => x.FirstName).NotNull()
                .WithMessage($"First name cannot be null");
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage($"First name must have a value");
            RuleFor(x => x.FirstName).Length(3, 100)
                .WithMessage($"First name must be at least 3 characters long");
            RuleFor(x => x.FirstName)
                .Matches(lettersPattern)
                .WithMessage("First name should only be contained of letters");

            RuleFor(x => x.LastName).NotNull()
                .WithMessage("Last name cannot be null");
            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage($"Last name must have a value");
            RuleFor(x => x.LastName).Length(3, 100)
                .WithMessage($"Last name must be at least 3 characters long");
            RuleFor(x => x.LastName)
                .Matches(lettersPattern)
                .WithMessage("Last name should only be contained of letters");

            RuleFor(x => x.SocialSkills).NotNull()
                .WithMessage("Social skills cannot be null");
            RuleFor(x => x.SocialSkills).NotEmpty()
                .WithMessage("Social skills cannot be empty");
            RuleFor(x => x.SocialSkills).Must(x => x.All(ss => !string.IsNullOrWhiteSpace(ss)))
                .WithMessage("All social skills must have a value");

            RuleFor(x => x.SocialMediaAccounts).NotNull()
                .WithMessage("Social media accounts cannot be null");
            RuleFor(x => x.SocialMediaAccounts).NotEmpty()
                .WithMessage("Social media accounts cannot be empty");
            RuleFor(x => x.SocialMediaAccounts).Must(x => x.All(sma => sma != null))
                .WithMessage("All social media accounts cannot be null");
            RuleFor(x => x.SocialMediaAccounts).Must(x => x.All(sma => !string.IsNullOrWhiteSpace(sma.Type)))
                .WithMessage("Social media account types must have a value");
            RuleFor(x => x.SocialMediaAccounts).Must(x => x.All(sma => !string.IsNullOrWhiteSpace(sma.Address)))
                .WithMessage("Social media account addresses must have a value");
        }
    }
}
