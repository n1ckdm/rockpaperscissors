using Microsoft.AspNetCore.Mvc;

namespace RPS.Api;

[Route("/rps")]
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
    
    [HttpPost]
    public async Task<IActionResult> Post(Rps.V1.Create request) =>
        await HandleRequest<Rps.V1.Create>(request);

    [Route("move")]
    [HttpPut]
    public async Task<IActionResult> Put(Rps.V1.Move request) =>
        await HandleRequest<Rps.V1.Move>(request);
}