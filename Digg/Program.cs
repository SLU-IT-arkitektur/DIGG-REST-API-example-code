var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddControllers(options =>
{
    // No need for response caching middleware when using strictly client-side caching.
    options.CacheProfiles.Add("30sec",
        new CacheProfile()
        {
            Duration = 30
        });
}).AddXmlDataContractSerializerFormatters();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(2, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.Configure<OpenApiInfo>(
    builder.Configuration.GetSection("OpenApiInfo"));

var app = builder.Build();

app.UseSwagger(c => {
    c.RouteTemplate = "api/{documentname}/openapi.json";
});
app.UseSwaggerUI(opt =>
{
    var apiVersionDescriptionProvider = 
        app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
    {
        opt.SwaggerEndpoint($"/api/{description.GroupName}/openapi.json",
            description.GroupName.ToUpperInvariant());
        opt.RoutePrefix = $"api";
    }
});

app.UseExceptionHandler("/shared/error");
app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));
app.MapControllers();

app.Run();
