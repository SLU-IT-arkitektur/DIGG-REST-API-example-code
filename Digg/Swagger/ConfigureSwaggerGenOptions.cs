﻿public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly OpenApiInfo _openApiInfo;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider, IOptions<OpenApiInfo> options) 
    {
            _provider = provider;
            _openApiInfo = options.Value;
    }

    /// <summary>
    /// Configure each API discovered for Swagger Documentation.
    /// </summary>
    public void Configure(SwaggerGenOptions options)
    {
        // Add swagger document for every API version discovered.
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Create information about the version of the API.
    /// </summary>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = _openApiInfo.Title,
            Description = _openApiInfo.Description,
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
        {
            info.Description += " (deprecated)";
        }

        return info;
    }
}