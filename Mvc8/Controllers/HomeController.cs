using Microsoft.Extensions.Options;

namespace Mvc8.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRegristrationService _externalRegService;
    private readonly IEnumerable<IRegristrationService> _regServices;
    private readonly IOptions<SampleWebSettings> _settings;
    private readonly IRegristrationService _regService;

    public HomeController(ILogger<HomeController> logger, 
        [FromKeyedServices("default")] IRegristrationService regService,
        [FromKeyedServices("external")] IRegristrationService externalRegService,
        IEnumerable<IRegristrationService> regServices,
        IOptions<SampleWebSettings> settings)
    {
        _logger = logger;
        _regService = regService;
        _externalRegService = externalRegService;
        _regServices = regServices;
        _settings = settings;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        ViewBag.DefaultMessage = _regService.Register("Klay");
        ViewBag.ExternalMessage = _regService.Register("Tompson");

        var servicesMsgs = new List<string>();
        foreach (var service in _regServices)
        {
            servicesMsgs.Add(service.Register(service.GetType().Name));
        }
        ViewBag.ServicesMessages = servicesMsgs;

        ViewBag.SampleWebSettings = _settings.Value;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
