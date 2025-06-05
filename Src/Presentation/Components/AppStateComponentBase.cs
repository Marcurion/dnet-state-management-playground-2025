using Domain.States;
using Microsoft.AspNetCore.Components;

namespace Presentation.Components;

public class AppStateComponentBase : ComponentBase, IDisposable
{
    [Inject]
    protected IComponentRenderingService RenderingService { get; set; } = default!;
    
    [Parameter]
    public IAppState AppState { get; set; } = default!;
    
    protected string ListAnimationClass = "";
    protected string ComponentAnimationClass = "";
    
    protected override bool ShouldRender() => true;
    
    public void ManualRender(IAppState newState)
    {
        Console.WriteLine("MANUALRENDER TRIGGERED");
        AppState = newState ?? throw new ArgumentNullException(nameof(newState));
        
        // Reset animation classes to retrigger animations
        ListAnimationClass = "";
        ComponentAnimationClass = "";
        
        InvokeAsync(StateHasChanged);
        
        // Apply animation classes after render with small delay
        _ = Task.Run(async () =>
        {
            await Task.Delay(10); // Small delay ensures DOM reflow
            ListAnimationClass = "list-update-animation";
            ComponentAnimationClass = "component-refresh-animation";
            await InvokeAsync(StateHasChanged);
        });
    }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        RenderingService.RegisterComponent(this);
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        Console.WriteLine("AFTER RENDER");
        base.OnAfterRender(firstRender);
    }
    
    public virtual void Dispose()
    {
        RenderingService.UnregisterComponent(this);
    }
}