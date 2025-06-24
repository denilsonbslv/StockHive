using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StockHive.Configure;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    // Este método será chamado para configurar o Swagger
    public void Configure(SwaggerGenOptions options)
    {
        // Para cada versão da API descoberta...
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            // ...adiciona um documento Swagger (ex: "v1", "v2", etc.)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "StockHive API",
            Version = description.ApiVersion.ToString(),
            Description = "Uma API para gerenciamento de estoque.",
            Contact = new OpenApiContact { Name = "Denilson B Silva", Email = "denilson.bslv@gmail.com" },
            License = new OpenApiLicense { Name = "Licença", Url = new Uri("https://exemplo.com/license") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " Esta versão da API foi descontinuada.";
        }

        return info;
    }
}