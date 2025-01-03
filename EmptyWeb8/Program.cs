var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("1");

    await next();

    await context.Response.WriteAsync("2");
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
