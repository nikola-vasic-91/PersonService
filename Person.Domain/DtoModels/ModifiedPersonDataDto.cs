namespace PersonService.Domain.DtoModels
{
    /// <summary>
    /// Data transfer object class for modified person data
    /// </summary>
    public class ModifiedPersonDataDto
    {
        #region Public properties

        public int NumberOfVowels { get; set; }
        public int NumberOfConsonants { get; set; }
        public string FullName { get; set; }
        public string ReversedName { get; set; }

        #endregion
    }
}
