using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Tests.Mocks
{
    class MockedAuthService : IAuthService
    {
        private string secretKey;

        public bool IsAuth { get; private set; }

        public MockedAuthService()
        {
            secretKey = "TopSecret!!!";
        }

        public void AddToCookie(HttpContext httpContext, string key, string value, CookieOptions cookieOptions)
        {
            httpContext.Response.Cookies.Append(key, EncryptValue(value), cookieOptions);
        }

        public string ReadCookie(HttpContext httpContext, string key, bool encrypt = false)
        {
            string value;
            try
            {
                if (encrypt)
                {
                    value = DecryptValue(httpContext.Request.Cookies[key]);
                }
                else
                {
                    value = httpContext.Request.Cookies[key];
                }
            }
            catch
            {
                value = null;
            }

            return value;
        }

        public void RemoveCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }

        public void SetIsAuth(bool value)
        {
            IsAuth = value;
        }

        public string EncryptValue(string value)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public string DecryptValue(string value)
        {
            var fullCipher = Convert.FromBase64String(value);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(secretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
