using System.Reflection;
using Application.Common.Composers;
using Application.Feature1;
using Domain.Common.Composers;
using Domain.Extensions;
using ErrorOr;
using FluentValidation;
using Infrastructure.Common.Composer;
using MediatR;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Presentation.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();

// Register component services
builder.Services.AddScoped<AppStateComponent>();
builder.Services.AddSingleton<IComponentRenderingService, ComponentRenderingService>();

// Register domain, application, and infrastructure services
builder.Services.RegisterDomain();
builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(Feature1RequestHandler))));
builder.Services.AddValidatorsFromAssembly(typeof(Feature1Validator).Assembly, ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

Task.Run(async () =>
{
    ErrorOr<int> featureResult = await app.Services.GetRequiredService<IMediator>().Send(new Feature1Request() { RequestInfo = "Test" });
    Console.WriteLine(featureResult.ConcatErrorCodes(" & "));
});

app.Run();

// Expose builder and app for testing
public partial class Program {} // Required for WebApplicationFactory<TEntryPoint>
