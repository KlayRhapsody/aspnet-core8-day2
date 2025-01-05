var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IRegristrationService, RegristrationService>();
builder.Services.AddSingleton<IRegristrationService, ExternalRegristrationService>();
// builder.Services.AddScoped<IRegristrationService, RegristrationService>();
// builder.Services.AddTransient<IRegristrationService, RegristrationService>();

builder.Services.AddKeyedSingleton<IRegristrationService, ExternalRegristrationService>("external");
builder.Services.AddKeyedSingleton<IRegristrationService, RegristrationService>("default");

builder.Services.Configure<SampleWebSettings>(builder.Configuration.GetSection("SampleWebSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
