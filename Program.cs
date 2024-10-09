using GCrypt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SuggestionBox.Data;
using SuggestionBox.Helper;
using SuggestionBox.Interface;
using SuggestionBox.Service;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Program));

var key = CredentialManager.ValidateCredential();

AppDomain.CurrentDomain.FirstChanceException += CatchAllExceptions; 
GCryptBuilder.Create()
.AddTripleDES(a =>
{
    //a.Key = "Suggestion_Box$_*_*_2024_IAK";
    a.Key = key;
    a.Mode = CipherMode.ECB;
    a.Padding = PaddingMode.PKCS7;
})
.BuildStatic();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // Set limit to 10 MB
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.LoginPath = new PathString("/home/Suggestion");
    o.ReturnUrlParameter = "ReturnUrl";
    //o.EventsType = typeof(CustomCookieAuthenticationEvents);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Suggestion}/{id?}"
);

app.Run();

void CatchAllExceptions(object sender, FirstChanceExceptionEventArgs e)
{
    _logger.Error(e.Exception);
}