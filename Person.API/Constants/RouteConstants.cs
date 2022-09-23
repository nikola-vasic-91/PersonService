namespace PersonService.API.Constants
{
    /// <summary>
    /// Class containing route constants
    /// </summary>
    public static class RouteConstants
    {
        #region Api routes

        public const string ApiRoute = "api/[controller]";

        #region Person routes

        public const string PersonIdEndpoint = "{personId:Guid}";
        public const string PersonModifiedEndpoint = PersonIdEndpoint + "/modified";

        #endregion

        #endregion
    }
}