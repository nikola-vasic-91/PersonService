using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PersonService.Service.Modules
{
    /// <summary>
    /// Class that contains dependency registrations needed for the current project
    /// </summary>
    public static class ServiceModule
    {
        #region Public methods

        /// <summary>
        /// Registers mediatR dependency
        /// </summary>
        /// <param name="serviceCollection">Service collection needed for the dependency registration</param>
        public static void RegisterMediatR(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(GetServiceAssembly());
        }

        #endregion

        #region Private methods

        private static Assembly GetServiceAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        #endregion
    }
}
