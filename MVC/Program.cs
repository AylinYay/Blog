using AppCore.DataAccess.EntityFramework.Bases;
using Business.Services;
using DataAccess;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Localization
// Web uygulamas�n�n b�lgesel ayar� a�a��daki �ekilde tek seferde konfig�re edilerek t�m projenin bu ayar� kullanmas� sa�lanabilir,
// dolay�s�yla veri formatlama veya d�n��t�rme gibi i�lemlerde her seferinde CultureInfo objesinin kullan�m gereksinimi ortadan kalkar.
// Bu �ekilde sadece tek bir b�lgesel ayar projede kullan�labilir.
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US") // e�er uygulama T�rk�e olacaksa CultureInfo constructor'�n�n parametresini ("tr-TR") yapmak yeterlidir.
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

// Add services to the container.
#region IoC Container
var connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(RepoBase<>), typeof(Repo<>));

builder.Services.AddScoped<IBlogService, BlogService>();
#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});
#endregion

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
