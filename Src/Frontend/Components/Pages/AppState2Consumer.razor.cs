using Domain.States;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Components.Pages;

public partial class AppState2Consumer : ComponentBase, IDisposable
{
    
    [Inject]
    public  IAppStateWrapper<List<int>> _appState2Wrapper { get; set; } = default!;
    
    protected string ListAnimationClass = "";
    protected string ComponentAnimationClass = "";
    
    protected override bool ShouldRender() => _shouldRender;

    private bool _shouldRender = true;
    
    
    public void ManualRender(IAppState<List<int>> newState)
    {
        Console.WriteLine("MANUAL RENDER TRIGGERED (APPSTATE2CONSUMER)");
        
        // Reset animation classes to retrigger animations
        ListAnimationClass = "";
        ComponentAnimationClass = "";

        _shouldRender = true;
        InvokeAsync(StateHasChanged);
        _shouldRender = false;
        
        // Apply animation classes after render with small delay
        _ = Task.Run(async () =>
        {
            await Task.Delay(10); // Small delay ensures DOM reflow
            ListAnimationClass = "list-update-animation";
            ComponentAnimationClass = "component-refresh-animation";
            _shouldRender = true;
            await InvokeAsync(StateHasChanged);
            _shouldRender = false;
        });
    }
    
    protected override void OnInitialized()
    {
        _appState2Wrapper.StateChanged += ManualRender;
        _shouldRender = false;
        base.OnInitialized();
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        // NOTABLE: This renders twice to unset and then set the CSS styles and re-trigger the animation
        Console.WriteLine("AFTER RENDER (APPSTATE2CONSUMER)");
        base.OnAfterRender(firstRender);
    }
    
    public virtual void Dispose()
    {
        _appState2Wrapper.StateChanged -= ManualRender;
    }
}