using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LightInject.WebApi.Cors
{
    /// <summary>
    /// Extension methods for enabling LightInject to integrate with Web API CORS
    /// </summary>
    public static class WebApiCorsExtensions
    {
        /// <summary>
        /// Registers all CORS policy types defined in the given assemblies, using the given lifetime
        /// </summary>
        /// <param name="serviceRegistry">The registry to register the services with</param>
        /// <param name="lifetime">The lifetime for the policy objects</param>
        /// <param name="assemblies">Assemblies to scan for types</param>
        public static void RegisterCorsPolicies(this IServiceRegistry serviceRegistry, ILifetime lifetime,
            params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var policyTypes =
                    assembly.GetTypes().Where(t => !t.IsAbstract && typeof(ICorsPolicyProvider).IsAssignableFrom(t));
                foreach (var policyType in policyTypes)
                {
                    serviceRegistry.Register(policyType, lifetime);
                }
            }
        }

        /// <summary>
        /// Registers all CORS policy types defined in the given assemblies, using the default lifetime
        /// </summary>
        /// <param name="serviceRegistry">Registry to register the policies into</param>
        /// <param name="assemblies">Assemblies to scan for types</param>
        public static void RegisterCorsPolicies(this IServiceRegistry serviceRegistry, params Assembly[] assemblies)
        {
            RegisterCorsPolicies(serviceRegistry, new PerRequestLifeTime(), assemblies);
        }

        /// <summary>
        /// Registers all CORS policy types defined in the calling <see cref="Assembly"/>, using the default lifetime
        /// </summary>
        /// <param name="serviceRegistry">Registry to register the policies into</param>
        public static void RegisterCorsPolicies(this IServiceRegistry serviceRegistry)
        {
            RegisterCorsPolicies(serviceRegistry, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Enable CORS for the current <see cref="T:System.Web.Http.HttpConfiguration" />, using the given IServiceContainer
        /// </summary>
        /// <param name="config">The current HttpConfiguration</param>
        /// <param name="container">Service cotnainer with policy types</param>
        public static void EnableCors(this HttpConfiguration config, IServiceContainer container)
        {
            container.EnableCors(config);
        }

        /// <summary>
        /// Use the current IServiceContainer to enable CORS on the given <see cref="T:System.Web.Http.HttpConfiguration" />
        /// </summary>
        /// <param name="container">The current service container with policy types</param>
        /// <param name="config">HttpConfiguration to enable CORS for.</param>
        public static void EnableCors(this IServiceContainer container, HttpConfiguration config)
        {
            var factory = new LightInjectPolicyFactory(container);
            config.SetCorsPolicyProviderFactory(factory);
            config.EnableCors();
        }
    }

    /// <summary>
    /// A simple CORS policy provider factory using LightInject to resolve CORS policies
    /// </summary>
    public class LightInjectPolicyFactory : ICorsPolicyProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightInjectPolicyFactory"/> class.
        /// </summary>
        /// <param name="serviceContainer">Service container to resolve policy types from</param>
        public LightInjectPolicyFactory(IServiceContainer serviceContainer)
        {
            Container = serviceContainer;
        }

        private IServiceContainer Container { get; }

        /// <summary>Gets the <see cref="T:System.Web.Http.Cors.ICorsPolicyProvider" /> for the request.</summary>
        /// <returns>The <see cref="T:System.Web.Http.Cors.ICorsPolicyProvider" />.</returns>
        /// <param name="request">The request.</param>
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return Container.TryGetInstance<ICorsPolicyProvider>();
        }
    }
}