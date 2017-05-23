using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toz.Dotnet.Authorization
{
    public static class CustomAuthorizationExtensions
    {
        public static IApplicationBuilder UseCustiomAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthorizationMiddleware>();
        }
    }
}
