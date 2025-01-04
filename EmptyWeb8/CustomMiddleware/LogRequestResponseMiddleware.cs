using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;

public class LogRequestResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogRequestResponseMiddleware> _logger;

    public LogRequestResponseMiddleware(RequestDelegate next, ILogger<LogRequestResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestTime = DateTime.UtcNow;
        _logger.LogInformation("Request URL: {Method} {Url}, Path: {Path}, QueryString: {QueryString}", 
            context.Request.Method, 
            context.Request.GetDisplayUrl(), 
            context.Request.Path,
            context.Request.QueryString);
        // _logger.LogInformation("Request Body: {Body}", await new StreamReader(context.Request.Body).ReadToEndAsync());
        var cultureInfo = CultureInfo.CurrentCulture;
        _logger.LogInformation("Request Culture: {Culture}, Date: {Date}", 
            cultureInfo.Name, 
            requestTime.ToString(cultureInfo));
        await _next(context);

        var responseTime = DateTime.UtcNow;
        _logger.LogInformation("Response: {StatusCode} {Elapsed:0.0000}ms", context.Response.StatusCode, (responseTime - requestTime).TotalMilliseconds);
    }
}