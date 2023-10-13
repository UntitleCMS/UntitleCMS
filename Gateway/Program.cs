using Gateway.Aggregators;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using OpenIddict.Validation.SystemNetHttp;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);



builder.Services.AddOpenIddict()
    .AddValidation(option =>
    {
        option.SetIssuer("http://authenticationservice/");
        option.UseSystemNetHttp();
        option.UseAspNetCore();
    });
// Allow any certificate ***DANGER FOR PRODUCTION***
builder.Services.AddHttpClient(typeof(OpenIddictValidationSystemNetHttpOptions).Assembly.GetName().Name!)
    .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
//builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
//    option =>
//{
//    option.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//    option.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//});

// body size
builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.MaxRequestBodySize = 20 * 1_000_000
);
builder.Services.Configure<IISServerOptions>(options =>
    options.MaxRequestBodyBufferSize = 20 * 1_000_000
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


//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseWebSockets();
app.UseOcelot().Wait();

app.Run();
