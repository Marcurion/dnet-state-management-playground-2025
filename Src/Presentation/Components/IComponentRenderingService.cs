using Domain.States;

namespace Presentation.Components;

public interface IComponentRenderingService
{
    void RegisterComponent(AppStateComponentBase component);
    void UnregisterComponent(AppStateComponentBase component);
    Task RenderAppStateComponentAsync(IAppState newState);
}