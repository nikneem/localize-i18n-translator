using Localizr.Proxy;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

var myCorsPolicyName = "allow-dev";
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


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

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCors(myCorsPolicyName);
app.UseHttpsRedirection();
app.MapReverseProxy();
app.Run();