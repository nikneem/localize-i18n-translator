using Localizr.Proxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

var myCorsPolicyName = "allow-dev";
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://localizr.eu.auth0.com/";
    options.Audience = "https://localizr-api.hexmaster.nl";
});


var proxyBuilder = builder.Services
    .AddSingleton<IProxyConfigProvider, SpreaViewProxyConfiguration>()
    .AddReverseProxy()
    .AddTransforms(transformBuilderContext =>
    {
        transformBuilderContext.AddRequestTransform(context =>
        {
            Console.WriteLine(context.ProxyRequest.RequestUri?.AbsoluteUri);

            return ValueTask.CompletedTask;
        });
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(myCorsPolicyName, bldr =>
    {
        bldr.WithOrigins("https://localizr.hexmaster.nl", "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

if (builder.Environment.IsDevelopment())
{
    proxyBuilder.AddServiceDiscoveryDestinationResolver();
}

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCors(myCorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapReverseProxy();
app.Run();