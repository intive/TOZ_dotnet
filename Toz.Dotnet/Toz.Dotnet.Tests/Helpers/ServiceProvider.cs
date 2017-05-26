using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Mocks;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Tests.Helpers
{
    public class ServiceProvider
    {
        private static ServiceProvider _instance;
        private readonly TestServer _server;
        public static ServiceProvider Instance
        {
            get
            {
                return _instance ?? (_instance = new ServiceProvider());
            }
        }

        private ServiceProvider()
        {
            var mockedRestService = new MockedRestService();
            var mockedAuthService = new MockedAuthService();
            _server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>().ConfigureServices(
                services =>
                {
                    services.AddSingleton<IAuthService>(serviceProvider => mockedAuthService);
                    services.AddSingleton<IRestService>(serviceProvider => mockedRestService);
                }));           
        }

        public T Resolve<T>() where T : class
        {
            if(_server?.Host != null)
            {
                return _server.Host.Services.GetService<T>();
            }
            return null;
        }

    }
}