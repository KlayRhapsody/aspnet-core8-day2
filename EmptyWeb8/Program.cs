
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseCustomExceptionHandling();

app.Use(async (context, next) =>
{
    if (判斷IP是否合法())
    {
        await next();
    }
    else
    {
        await context.Response.WriteAsync("Bad Request");
    }
});

bool 判斷IP是否合法()
{
    return true;
}

app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("<h1>");

    await next();

    await context.Response.WriteAsync("</h1>");

});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("3");

    await next();

    await context.Response.WriteAsync("4");
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("5");
});

app.Run();
