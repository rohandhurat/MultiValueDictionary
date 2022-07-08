using Microsoft.Extensions.DependencyInjection;
using System;

namespace MultiValueDictionary
{
    public static class IoC
    {
        public static readonly IServiceCollection services;

        //instantiate new servicecollection
        static IoC()
        {
            services = new ServiceCollection();
        }

        //register Interface, Class or service, implementation
        public static void Register<TInterface, TClass>()
        where TClass : class, TInterface
        where TInterface : class
        {
            services.AddScoped<TInterface, TClass>();
        }

        //function to resolve and return the object
        public static T Get<T>()
        {
            return services.BuildServiceProvider().GetRequiredService<T>();

        }
    }
}
