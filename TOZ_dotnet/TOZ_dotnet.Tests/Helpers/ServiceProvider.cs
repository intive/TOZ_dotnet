using Microsoft.Extensions.DependencyInjection;
using MvcApp;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;

namespace TOZ_dotnet.Tests.Helpers
{
    public class ServiceProvider
    {
        private static ServiceProvider _instance;
        private TestServer _server;
        public static ServiceProvider Instance
        {
            get
            {
                return _instance ?? (_instance = new ServiceProvider());
            }
        }

        private ServiceProvider()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
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