using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using OpenIddict.Validation.SystemNetHttp;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddOcelot(builder.Configuration);

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
app.UseOcelot().Wait();

app.MapControllers();

app.Run();
