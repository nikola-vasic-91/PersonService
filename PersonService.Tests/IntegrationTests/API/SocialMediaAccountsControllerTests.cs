using Newtonsoft.Json;
using PersonService.Domain.DtoModels;
using PersonService.Tests.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace PersonService.Tests.IntegrationTests.API
{
    /// <summary>
    /// Social media accounts integration tests
    /// </summary>
    [Collection("IntegrationTest Collection")]
    public class SocialMediaAccountFeature
    {
        #region Private fields

        private readonly TestFixture _testContext;
        private string socialMediaAccountsEndpoint = "api/SocialMediaAccounts";

        #endregion

        #region Constructor

        public SocialMediaAccountFeature(TestFixture testFixture) => _testContext = testFixture;

        #endregion

        #region Tests

        [Scenario]
        public void GetAllSocialMediaAccounts_Success()
        {
            List<SocialMediaAccountDto> socialMediaAccounts;

            "Given I want to get all social media accounts"
                .x(() =>
                {
                });

            "When I get all social media acccounts"
                .x(async () =>
                {
                    _testContext.ResponseMessage = await _testContext.Client.GetAsync(socialMediaAccountsEndpoint);
                });

            "Then social media accounts all successfully retrieved"
                 .x(async () =>
                 {
                     _testContext.ResponseMessage.EnsureSuccessStatusCode();
                     socialMediaAccounts = JsonConvert.DeserializeObject<List<SocialMediaAccountDto>>(await _testContext.ResponseMessage.Content.ReadAsStringAsync());

                     Assert.NotNull(socialMediaAccounts);
                     Assert.NotEmpty(socialMediaAccounts);
                 });
        }

        #endregion

    }
}
