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

}