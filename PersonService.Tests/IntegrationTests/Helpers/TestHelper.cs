using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace PersonService.Tests.IntegrationTests.Helpers
{
    /// <summary>
    /// Test helper class
    /// </summary>
    public static class TestHelper
    {
        #region Public methods

        /// <summary>
        /// Convert model data to byte array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ByteArrayContent ConvertToByte<T>(T model)
        {
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        #endregion
    }
}
