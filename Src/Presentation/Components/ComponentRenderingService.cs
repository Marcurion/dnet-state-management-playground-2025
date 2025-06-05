using Domain.States;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Presentation.Components;

public class ComponentRenderingService : IComponentRenderingService
{
    private readonly List<AppStateComponentBase> _registeredComponents = new();
    private readonly ILogger<ComponentRenderingService> _logger;

    public ComponentRenderingService(ILogger<ComponentRenderingService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void RegisterComponent(AppStateComponentBase component)
    {
        if (component != null && !_registeredComponents.Contains(component))
        {
            _registeredComponents.Add(component);
            _logger.LogInformation("Registered component of type {ComponentType}", component.GetType().Name);
        }
    }

    public void UnregisterComponent(AppStateComponentBase component)
    {
        if (component != null && _registeredComponents.Contains(component))
        {
            _registeredComponents.Remove(component);
            _logger.LogInformation("Unregistered component of type {ComponentType}", component.GetType().Name);
        }
    }

    public Task RenderAppStateComponentAsync(IAppState newState)
    {
        _logger.LogInformation("Rendering {ComponentCount} components with updated state", _registeredComponents.Count);
        
        foreach (var component in _registeredComponents)
        {
            try
            {
                component.ManualRender(newState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering component of type {ComponentType}", component.GetType().Name);
            }
        }

        return Task.CompletedTask;
    }
}