using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Tests.Mocks
{
    public class MockedRestService : IRestService
    {
        public async Task<bool> ExecuteDeleteAction<T>(string address, T obj, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            return IsValidData(address, obj);
        }

        public async Task<T> ExecuteGetAction<T>(string address, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            var mockedObject = new Mock<T>();
            if (IsValidData(address,mockedObject.Object))
            {
                return mockedObject.Object;
            }
            return default(T);
        }

        public async Task<bool> ExecutePostAction<T>(string address, T obj, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            return IsValidData(address, obj);
        }

        public async Task<bool> ExecutePutAction<T>(string address, T obj, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            return IsValidData(address, obj);
        }

        private bool IsValidData<T>(string address, T obj)
        {
            if (string.IsNullOrEmpty(address) || obj == null || !Uri.IsWellFormedUriString(address, UriKind.Absolute))
            {
                return false;
            }
            return true;
        }
    }
}