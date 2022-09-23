using AutoMapper;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Models;
using static PersonService.Domain.Helpers.PersonDataHelper;

namespace PersonService.Domain.Modules
{
    /// <summary>
    /// Class containing mapping definitions
    /// </summary>
    public class MappingProfile : Profile
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<PersonDto, Person>()
                .ForMember(p => p.PersonSocialMediaAccounts, o => o.MapFrom(pdto => MapSocialMediaAccountsForPerson(pdto)))
                .ForMember(p => p.SocialSkills, o => o.MapFrom(pdto => MapSocialSkills(pdto)));

            CreateMap<Person, PersonDto>()
                .ForMember(p => p.SocialMediaAccounts, o => o.MapFrom(pdto => MapSocialMediaAccounts(pdto)))
                .ForMember(p => p.SocialSkills, o => o.MapFrom(pdto =>
                    pdto.SocialSkills.Select(ss => ss.Name).ToList()));

            CreateMap<Person, ModifiedPersonDataDto>()
                .ForMember(mpd => mpd.NumberOfVowels, o => o.MapFrom(p => p.GetFullName().GetNumberOfVowels()))
                .ForMember(mpd => mpd.NumberOfConsonants, o => o.MapFrom(p => p.GetFullName().GetNumberOfConsonants()))
                .ForMember(mpd => mpd.FullName, o => o.MapFrom(p => p.GetFullName()))
                .ForMember(mpd => mpd.ReversedName, o => o.MapFrom(p => new string(p.GetFullName().Reverse().ToArray())));

            CreateMap<SocialMediaAccountDto, SocialMediaAccount>()
                .ReverseMap();

            CreateMap<SocialSkillDto, SocialSkill>()
                .ReverseMap();
        }

        #endregion

        #region Private methods

        private List<PersonSocialMediaAccount> MapSocialMediaAccountsForPerson(PersonDto person) =>
            person.SocialMediaAccounts.Select(sma => new PersonSocialMediaAccount
                {
                    PersonId = person.PersonId,
                    SocialMediaAccountId = sma.SocialMediaAccountId,
                    Address = sma.Address,
                    SocialMediaAccount = sma.SocialMediaAccountId == Guid.Empty ? new SocialMediaAccount
                    {
                        Type = sma.Type
                    } : default
            }).ToList();

        private List<SocialSkill> MapSocialSkills(PersonDto person) =>
            person.SocialSkills.Select(socialSkill => new SocialSkill
                {
                    Name = socialSkill,
                    PersonId = person.PersonId
                }).ToList();

        private List<PersonSocialMediaAccountDto> MapSocialMediaAccounts(Person person) =>
            person.PersonSocialMediaAccounts.Select(psma => new PersonSocialMediaAccountDto
            {
                SocialMediaAccountId = psma.SocialMediaAccount.SocialMediaAccountId,
                Type = psma.SocialMediaAccount.Type,
                Address = psma.Address,
            }).ToList();

        #endregion
    }
}
