using System;
using System.Reflection;
using Chronos.Proxy;
using System.Globalization;

namespace Chronos
{
    /// <summary>
    /// Represents storage for shared services and data providers
    /// </summary>
    [PublicService(typeof(Proxy.ServiceContainer))]
    public interface IServiceContainer
    {
        /// <summary>
        /// Get registered service by type.
        /// </summary>
        /// <returns>Registered service or null if container doesn't have service registered under provided type</returns>
        object Resolve(Type type);

        /// <summary>
        /// Register shared service.
        /// If container already has registration under provided type then it will not be overridden.
        /// Type GUID will be used as primary key for the registration.
        /// </summary>
        /// <param name="service">Service to register</param>
        /// <returns>Returns 'True' if service is registred successfully, otherwise 'False'</returns>
        bool Register(object service);

        /// <summary>
        /// Check that registration with this type is presented in the container.
        /// Type will be used as primary key for the registration.
        /// </summary>
        /// <returns>'True' registration with this type is presented in the container, other wise 'False'</returns>
        bool IsRegistered(Type type);
    }

    public static class ServiceContainerExtensions
    {
        /// <summary>
        /// Get registered service by type.
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Registered service or null if container doesn't have service registered under provided type</returns>
        public static T Resolve<T>(this IServiceContainer container)
        {
            Type serviceInterfaceType = ServiceRegistration.FindServiceInterfaceType(typeof(T));
            return (T) container.Resolve(serviceInterfaceType);
        }

        /// <summary>
        /// Check that registration with this type is presented in the container.
        /// Type GUID will be used as primary key for the registration.
        /// </summary>
        /// <typeparam name="T">Type under which service should be registered</typeparam>
        /// <returns>'True' registration with this type is presented in the container, other wise 'False'</returns>
        public static  bool IsRegistered<T>(this IServiceContainer container)
        {
            Type serviceInterfaceType = ServiceRegistration.FindServiceInterfaceType(typeof(T));
            return container.IsRegistered(serviceInterfaceType);
        }

        public static object BuildServiceProxy(this IServiceContainer container, object service)
        {
            // Building proxy of the service:
            // 1. Build registration of the service
            ServiceRegistration registration = ServiceRegistration.FromService(service);
            // 1. If registration of service has no proxy, then return original service registration
            Type serviceProxyType = registration.ServiceProxyType;
            if (serviceProxyType == null)
            {
                return registration;
            }
            // 2. Get target constructor with 1 argument
            ConstructorInfo constructor = FindServiceProxyContructor(serviceProxyType, registration.ServiceInterfaceType);
            if (constructor == null)
            {
                throw new TempException("Unable to find appropriate proxy constructor");
            }
            // 3. Prepare parameters for constructor
            object[] constructorArguments = new object[1];
            constructorArguments[0] = registration.Service;
            object serviceProxy = Activator.CreateInstance(serviceProxyType, constructorArguments);

            // 4. Now we should initialize proxy if its required
            MethodInfo initializationMethod = FindServiceProxyInitializationMethod(serviceProxyType);
            if (initializationMethod != null)
            {
                // 5. Go through parameters of the method and resolve them one by one
                ParameterInfo[] parameters = initializationMethod.GetParameters();
                object[] initializationArguments = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo parameter = parameters[i];
                    initializationArguments[i] = container.Resolve(parameter.ParameterType);
                    if (initializationArguments[i] == null)
                    {
                        throw new TempException(string.Format("Unable to resolve type {0} when build proxy", parameter.ParameterType));
                    }
                }
                initializationMethod.Invoke(serviceProxy, BindingFlags.Instance | BindingFlags.InvokeMethod, null,
                                            initializationArguments, CultureInfo.CurrentCulture);
            }
            return serviceProxy;
        }
        
        private static MethodInfo FindServiceProxyInitializationMethod(Type serviceProxyType)
        {
            //Go trhough the list of all constructors
            foreach (MethodInfo method in serviceProxyType.GetMethods())
            {
                ServiceProxyInitializationMethodAttribute attribute = method.GetCustomAttribute<ServiceProxyInitializationMethodAttribute>();
                if (attribute != null)
                {
                    return method;
                }
            }
            return null;
        }

        private static ConstructorInfo FindServiceProxyContructor(Type serviceProxyType, Type serviceInterfaceType)
        {
            //Go though the list of all constructors to find target
            foreach (ConstructorInfo constructor in serviceProxyType.GetConstructors())
            {
                //Get parameters
                ParameterInfo[] parameters = constructor.GetParameters();
                //We are looking for contructor with 1 parameter that has type of real object
                if (parameters.Length != 1)
                {
                    continue;
                }
                ParameterInfo firstParameter = parameters[0];
                //Check that first parameter is assignable from serviceInterfaceType
                if (!firstParameter.ParameterType.IsAssignableFrom(serviceInterfaceType))
                {
                    continue;
                }
                return constructor;
            }
            return null;
        }
    }
}
