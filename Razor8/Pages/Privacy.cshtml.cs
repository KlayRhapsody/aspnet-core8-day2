using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor8.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public string Now { get; set; }

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
        Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void OnGet()
    {
        Now = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void OnPost()
    {
        Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    }
}

