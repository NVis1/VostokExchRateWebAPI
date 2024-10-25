using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VostokExchRateWebAPI.Models;
using VostokExchRateWebAPI.Services;

namespace VostokExchRateWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    public CurrencyController(
        ICurrencyRepository currencyRepository,
        ILogger<CurrencyController> logger,
        IMemoryCache cache
    )
    {
        _curRep = currencyRepository;
        _cache = cache;
        _logger = logger;
    }

    private readonly ICurrencyRepository _curRep;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CurrencyController> _logger;
    private readonly string cacheKey = "currencies";

    /*        
            ===============ENDPOINTS===============
    */

    [HttpGet("Get")]
    [ProducesResponseType(typeof(IEnumerable<CurrencyModel>), 200)]
    public IActionResult Get()
    {
        if (_cache.TryGetValue(cacheKey, out IEnumerable<CurrencyModel>? currencies))
        {
            _logger.Log(LogLevel.Information, "Using cache");
        }
        else
        {
            _logger.Log(LogLevel.Information, "Saving to cache");

            currencies = _curRep.GetAll();

            _cache.Set(cacheKey, currencies, new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromSeconds(60))
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
               .SetPriority(CacheItemPriority.Normal)
            );
        }

        if (ModelState.IsValid)
            return Ok(currencies);

        return BadRequest(ModelState);
    }


    [HttpPost("Save/[[bank]]")]
    [ProducesResponseType(typeof(IEnumerable<CurrencyModel>), 200)]
    public IActionResult Save([FromRoute] string bank = "Vostok")
    {
        CurrencyCollector collector = bank switch
        {
            "Vostok" => new VostokCurrencyCollector(),
            "Mono"   => new MonoCurrencyCollector(),
            _ => new VostokCurrencyCollector(),
        };

        IEnumerable<CurrencyModel>? currencies;

        try { currencies = collector.Get(); }
        catch (ExternalApiException ex) {
            _logger.Log(LogLevel.Error, "");
            return Problem(ex.Message, statusCode: 504);
        }

        if (currencies != null)
            _curRep.SaveToDB(currencies);

        if (ModelState.IsValid)
            return Ok(currencies);

        return BadRequest(ModelState);
    }


    [HttpPost("Clcache")]
    [ProducesResponseType(200)]
    public IActionResult ClearCache()
    {
        _cache.Remove(cacheKey);
        _logger.Log(LogLevel.Information, "Cache has been cleared.");

        if (ModelState.IsValid)
            return Ok();

        return BadRequest(ModelState);
    }
}
