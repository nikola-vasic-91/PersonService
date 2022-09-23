using PersonService.Domain.Models;
using System.Text.RegularExpressions;

namespace PersonService.Domain.Helpers
{
    /// <summary>
    /// Person data helper class
    /// </summary>
    public static class PersonDataHelper
    {
        #region Public methods

        /// <summary>
        /// Returns number of vowels for the specific string
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns>Number of vowels for the specific string</returns>
        public static int GetNumberOfVowels(this string input)
        {
            string vowelsPattern = @"[aeiou]";
            return Regex.Matches(input, vowelsPattern, RegexOptions.IgnoreCase).Count;
        }

        /// <summary>
        /// Returns number of consonants for the specific string
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns>Number of consonants for the specific string</returns>
        public static int GetNumberOfConsonants(this string input)
        {
            string consonantsPattern = @"[a-z-[aeiou]]";
            return Regex.Matches(input, consonantsPattern, RegexOptions.IgnoreCase).Count;
        }

        /// <summary>
        /// Gets full name for the specific person
        /// </summary>
        /// <param name="person">Person entity</param>
        /// <returns>Full name for the specific person</returns>
        public static string GetFullName(this Person person)
        {
            return $"{person?.FirstName ?? string.Empty} {person?.LastName ?? string.Empty}".Trim();
        }

        #endregion
    }
}
