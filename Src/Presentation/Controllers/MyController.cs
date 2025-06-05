using Application;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.StateManagement.Specific;
using Domain.States;
using Presentation.Components;

namespace Presentation.Controllers
{
    public class MyController : Controller
    {

        MyManager _manager;

        public MyController(MyManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            _manager.ChangeSomeData();

            return Ok();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AppStateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IComponentRenderingService _renderingService;
        private readonly ILogger<AppStateController> _logger;

        public AppStateController(
            IMediator mediator,
            IComponentRenderingService renderingService,
            ILogger<AppStateController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _renderingService = renderingService ?? throw new ArgumentNullException(nameof(renderingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNumber([FromBody] AddNumberRequest request)
        {
            try
            {
                _logger.LogInformation("Adding number: {Number}", request.Number);
                
                // Get the current state first
                var getResult = await _mediator.Send(new GetAppStateRequest());
                if (getResult.IsError)
                {
                    _logger.LogError("Failed to get app state: {Errors}", string.Join(", ", getResult.Errors));
                    return BadRequest("Failed to get current state");
                }
                
                // Create a copy of the current state and add the new number
                var currentNumbers = getResult.Value.Numbers.ToList();
                currentNumbers.Add(request.Number);
                
                // Update the state via MediatR
                var info = await _mediator.Send(new GetAppStateRequest());
                var latestHash = info.Value.GetHashCode();
                var result = await _mediator.Send(new SetAppStateRequest { 
                    NewState = new AppState(){Numbers = currentNumbers, AppStateChanged = info.Value.AppStateChanged}, 
                    LastStateHash = latestHash
                });
                
                if (result.IsError)
                {
                    _logger.LogError("Failed to update app state: {Errors}", string.Join(", ", result.Errors));
                    return BadRequest("Failed to update state");
                }
                
                // Manual render of components with updated state
                await _renderingService.RenderAppStateComponentAsync(result.Value);
                
                return Ok(new { success = true, newState = result.Value });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating app state");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class AddNumberRequest
    {
        public int Number { get; set; }
    }
}
