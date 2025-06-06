using System.Reflection;
using Application.Common.Composers;
using Application.Feature1;
using BlazorApp1.Components;
using Domain.Common.Composers;
using FluentValidation;
using Infrastructure.Common.Composer;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.RegisterDomain();
builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(Feature1RequestHandler))));
builder.Services.AddValidatorsFromAssembly(typeof(Feature1Validator).Assembly, ServiceLifetime.Transient);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();