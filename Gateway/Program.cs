using Gateway.Aggregators;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenIddict.Validation.SystemNetHttp;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddOcelot(Path.Combine(Directory.GetCurrentDirectory(),"Ocelot"),builder.Environment);

builder.Services.AddOpenIddict()
    .AddValidation(option =>
    {
        option.SetIssuer(Environment.GetEnvironmentVariable("AUTH_ISSUER")
            ?? "https://p.villsource.tk/api/auth/v2"
            ?? throw new Exception("Authentication Issuer not found."));
        option.UseSystemNetHttp();
        option.UseAspNetCore();
    });

// Allow any certificate ***DANGER FOR PRODUCTION***
builder.Services.AddHttpClient(typeof(OpenIddictValidationSystemNetHttpOptions).Assembly.GetName().Name!)
    .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });


// Set max Body size
_ = int.TryParse(Environment.GetEnvironmentVariable("MAX_REQUEST_BODY_SIZE")
    ??"50", out int maxBodySizeMegabyte);
builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.MaxRequestBodySize = maxBodySizeMegabyte * 1_000_000
);
builder.Services.Configure<IISServerOptions>(options =>
    options.MaxRequestBodyBufferSize = maxBodySizeMegabyte * 1_000_000
);


builder.Services
    .AddOcelot(builder.Configuration)
    .AddSingletonDefinedAggregator<PostsAggregator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseOcelot().Wait();

app.Run();
