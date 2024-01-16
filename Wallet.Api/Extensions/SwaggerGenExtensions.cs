using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wallet.Api.Extensions;

public static class SwaggerGenExtensions
{
    public static void SwaggerGenSetup(this SwaggerGenOptions c, string environmentName, string applicationName)
    {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = $"{applicationName} ({environmentName})",
                    Version = "v1",
                });
            c.DescribeAllParametersInCamelCase();
    }
}