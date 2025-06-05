using Domain.States;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.StateManagement.Specific;
using Presentation.Components;

namespace Presentation.Pages;

public class AppStatePageModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IAppStateWrapper _appStateWrapper;
    private readonly IComponentRenderingService _renderingService;
    private readonly ILogger<AppStatePageModel> _logger;

    [BindProperty]
    public int NewNumber { get; set; }

    public IAppState CurrentState { get; private set; } = default!;

    public AppStatePageModel(
        IMediator mediator, 
        IAppStateWrapper appStateWrapper, 
        IComponentRenderingService renderingService,
        ILogger<AppStatePageModel> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _appStateWrapper = appStateWrapper ?? throw new ArgumentNullException(nameof(appStateWrapper));
        _renderingService = renderingService ?? throw new ArgumentNullException(nameof(renderingService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task OnGetAsync()
    {
        var result = await _mediator.Send(new GetAppStateRequest());
        if (result.IsError)
        {
            _logger.LogError("Failed to get app state: {Errors}", string.Join(", ", result.Errors));
            return;
        }

        CurrentState = result.Value;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Get the current state first
            var getResult = await _mediator.Send(new GetAppStateRequest());
            if (getResult.IsError)
            {
                _logger.LogError("Failed to get app state: {Errors}", string.Join(", ", getResult.Errors));
                return Page();
            }
            
            // Create a copy of the current state and add the new number
            var currentNumbers = getResult.Value.Numbers.ToList();
            currentNumbers.Add(NewNumber);
            
            _logger.LogInformation("Updating state with new number {Number}", NewNumber);

             //Update the state via MediatR
             var info = await _mediator.Send(new GetAppStateRequest());
             var latestHash = info.Value.GetHashCode();
            var result = await _mediator.Send(new SetAppStateRequest { NewState = new AppState(){Numbers = currentNumbers, AppStateChanged = info.Value.AppStateChanged}, LastStateHash = latestHash});
            
             if (result.IsError)
             {
                 _logger.LogError("Failed to update app state: {Errors}", string.Join(", ", result.Errors));
                 return Page();
             }
            
             // Manual render of components with updated state
             await _renderingService.RenderAppStateComponentAsync(result.Value);

            return RedirectToPage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating app state");
            return Page();
        }
    }
}