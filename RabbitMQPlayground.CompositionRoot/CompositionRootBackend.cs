using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Logic.Books;
using RabbitMQPlayground.Logic.Users;
using System.Net.Http;

namespace RabbitMQPlayground.CompositionRoot
{
    public class CompositionRootBackend : ICompositionRoot
    {
        private StandardKernel _kernel;
        private static volatile CompositionRootBackend instance;
        private static object syncRoot = new object();

        public static CompositionRootBackend Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CompositionRootBackend();
                        }
                    }
                }

                return instance;
            }
        }

        public T GetImplementation<T>(string instanceName = null)
        {
            return _kernel.Get<T>(instanceName);
        }

        public T GetImplementation<T>()
        {
            return _kernel.Get<T>();
        }

        public CompositionRootBackend()
        {
            _kernel = new StandardKernel();

            #region Http Context

            _kernel.Bind<IHttpContextAccessor>().To<HttpContextAccessor>().InSingletonScope();

            #endregion Http Context

            #region HttpClientFactory

            _kernel.Bind<ServiceProvider>().ToMethod(context =>
            {
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddHttpClient();

                return serviceCollection.BuildServiceProvider();
            }).InSingletonScope();

            _kernel.Bind<IHttpClientFactory>().ToMethod(context =>
            {
                var serviceProvider = context.Kernel.Get<ServiceProvider>();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                return httpClientFactory;
            });

            #endregion HttpClientFactory

            _kernel.Bind<IBookService>().To<BookService>()
                .WithConstructorArgument(context => context.Kernel.Get<IHttpClientFactory>());
            ;
            _kernel.Bind<IUserService>().To<UserService>()
                .WithConstructorArgument(context => context.Kernel.Get<IHttpClientFactory>());
        }
    }
}