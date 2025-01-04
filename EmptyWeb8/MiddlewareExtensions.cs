

using EmptyWeb8.CustomMiddleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync(ex.Message);
            }
        });
    }

    public static IApplicationBuilder UseCustomCompression(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CompressionMiddleware>();
    }

    public static IApplicationBuilder UseLogRequestResponse(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogRequestResponseMiddleware>();
    }

    public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestCultureMiddleware>();
    }
}
