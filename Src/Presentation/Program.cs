using System.Reflection;
using Application.Common.Composers;
using Application.Feature1;
using Domain.Common.Composers;
using Domain.Extensions;
using ErrorOr;
using FluentValidation;
using Infrastructure.Common.Composer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.RegisterDomain();
builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(Feature1RequestHandler))));
builder.Services.AddValidatorsFromAssembly(typeof(Feature1Validator).Assembly, ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

Task.Run(async () =>
{
    ErrorOr<int> featureResult = await app.Services.GetRequiredService<IMediator>().Send(new Feature1Request() { RequestInfo = "Test" });
    Console.WriteLine(featureResult.ConcatErrorCodes(" & "));
});

app.Run();

// Expose builder and app for testing
public partial class Program {} // Required for WebApplicationFactory<TEntryPoint>
