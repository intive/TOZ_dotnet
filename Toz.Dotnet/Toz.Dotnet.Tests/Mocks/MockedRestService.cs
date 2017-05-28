using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;

namespace Toz.Dotnet.Tests.Mocks
{
    public class MockedRestService : IRestService
    {
        public async Task<bool> ExecuteDeleteAction(string address, string token, CancellationToken cancelationToken = new CancellationToken())
        {
            return Uri.IsWellFormedUriString(address, UriKind.Absolute);
        }

        public async Task<T> ExecuteGetAction<T>(string address, string token, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            var mockedObject = new Mock<T>();
            if (IsValidData(address,mockedObject.Object, token))
            {
                return mockedObject.Object;
            }
            return default(T);
        }

        public async Task<bool> ExecutePostAction<T>(string address, T obj, string token, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            return IsValidData(address, obj, token);
        }

        public async Task<string> ExecutePostActionAndReturnId<T>(string address, T obj, string token,
            CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            if (IsValidData(address, obj))
            {
                return "ok";
            }
            return null;
        }

        public async Task<T1> ExecutePostAction<T1, T2>(string address, T2 obj, string token = null, CancellationToken cancelationToken = default(CancellationToken))
        where T1 : class
        where T2 : class
        {
            if (typeof(T1) == typeof(JwtToken) && typeof(T2) == typeof(Login))
            {
                if (IsValidData(address, obj))
                {
                    return (T1)Convert.ChangeType(TestingObjectProvider.Instance.JwtToken, typeof(T1));
                }
                return default(T1);
            }
            else
            {
                var mockedObject = new Mock<T1>();
                if (IsValidData(address, obj, token))
                {
                    return mockedObject.Object;
                }
                return default(T1);
            }
        }

        public async Task<bool> ExecutePutAction<T>(string address, T obj, string token, CancellationToken cancelationToken = new CancellationToken()) where T : class
        {
            return IsValidData(address, obj, token);
        }

        private bool IsValidData<T>(string address, T obj, string token)
        {
            if (string.IsNullOrEmpty(address) || obj == null || !Uri.IsWellFormedUriString(address, UriKind.Absolute) || token == null)
            {
                return false;
            }
            return true;
        }
        
        private bool IsValidData<T>(string address, T obj)
        {
            if (string.IsNullOrEmpty(address) || obj == null || !Uri.IsWellFormedUriString(address, UriKind.Absolute))
            {
                return false;
            }
            return true;
        }

        public Task<bool> ExecutePostMultipartAction(string address, IEnumerable<IFormFile> files, string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}