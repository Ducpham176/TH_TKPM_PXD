using ASC.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TH_TKPM_PXD.Configuration;
using TH_TKPM_PXD.Data;
using TH_TKPM_PXD.Service;

var builder = WebApplication.CreateBuilder(args);

// ?? ??m b?o ??c ???c appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");

// ?? ??ng ký DbContext (ch? 1 l?n)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ?? ??ng ký ApplicationSettings
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("AppSettings"));

// ?? ??ng ký các d?ch v? khác
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IIdentitySeed, IdentitySeed>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(); 

var app = builder.Build();

// ?? Ki?m tra môi tr??ng ?? áp d?ng middleware phù h?p
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// ?? Seed d? li?u vào DB
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var appSettings = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>();

    Console.WriteLine($"UserManager is null: {userManager == null}");
    Console.WriteLine($"RoleManager is null: {roleManager == null}");
    Console.WriteLine($"ApplicationSettings is null: {appSettings == null}");

    var storageSeed = serviceProvider.GetRequiredService<IIdentitySeed>();
    await storageSeed.Seed(userManager, roleManager, appSettings);
}

app.Run();
