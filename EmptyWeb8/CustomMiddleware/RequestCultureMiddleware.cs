using System.Globalization;

namespace EmptyWeb8.CustomMiddleware;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureQuery = context.Request.Query["culture"];
        if (string.IsNullOrWhiteSpace(cultureQuery))
        {
            cultureQuery = "en-US"; // 預設文化
        }

        if (CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Any(c => c.Name.Equals(cultureQuery, StringComparison.OrdinalIgnoreCase)))
        {
            var cultureInfo = new CultureInfo(cultureQuery);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
        else
        {
            // 記錄錯誤並回退到預設文化
            Console.WriteLine($"Invalid culture '{cultureQuery}' provided. Falling back to 'en-US'.");
            var defaultCulture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = defaultCulture;
            CultureInfo.CurrentUICulture = defaultCulture;
        }

        await _next(context);
    }
}