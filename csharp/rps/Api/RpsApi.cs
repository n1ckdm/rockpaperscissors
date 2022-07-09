using Microsoft.AspNetCore.Mvc;

namespace RPS.Api;

[ApiController]
[Route("[rps]")]
public class RpsApi : ControllerBase
{
    private readonly RpsAppService _appService;
    private readonly ILogger<RpsApi> _logger;
    public RpsApi(RpsAppService appService, ILogger<RpsApi> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    private async Task<IActionResult> HandleRequest<T>(T request)
    {
        try
        {
            _logger.LogDebug($"Handling HTTP request of type {typeof(T).Name}");
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            await _appService.Handle(request);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Error handling the request");
            _logger.LogTrace(e.ToString());
            return new BadRequestObjectResult(new {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }
    
    [HttpPost(Name = "Create")]
    public async Task<IActionResult> Create(Rps.V1.Create request) =>
        await HandleRequest<Rps.V1.Create>(request);

    [Route("move")]
    [HttpPost(Name = "Move")]
    public async Task<IActionResult> Move(Rps.V1.Move request) =>
        await HandleRequest<Rps.V1.Move>(request);
}