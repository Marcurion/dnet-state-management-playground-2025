using Domain.States;
using Microsoft.AspNetCore.Components;

namespace Presentation.Components;

public class AppStateComponentBase : ComponentBase, IDisposable
{
    [Inject]
    protected IComponentRenderingService RenderingService { get; set; } = default!;
    
    [Parameter]
    public IAppState AppState { get; set; } = default!;
    
    private bool _shouldRender = true;
    
    protected override bool ShouldRender() => _shouldRender;
    
    public void ManualRender(IAppState newState)
    {
        Console.WriteLine("MANUALRENDER TRIGGERED");
        AppState = newState ?? throw new ArgumentNullException(nameof(newState));
        _shouldRender = true;
        StateHasChanged();
        _shouldRender = false;
    }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _shouldRender = true;
        RenderingService.RegisterComponent(this);
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine("AFTER RENDER");
        base.OnAfterRender(firstRender);
        _shouldRender = false;
    }
    
    public virtual void Dispose()
    {
        RenderingService.UnregisterComponent(this);
    }
}