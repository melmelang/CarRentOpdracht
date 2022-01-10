using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DBContextConnection");

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<CarRentingProject_AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DBContext>();

builder.Services.AddMvc()
       .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
       .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
//builder.Services.Configure<MailKitOptions>(options =>
//{
//    options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
//    options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
//    options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
//    options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
//    options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
//    options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];

//    // Set it to TRUE to enable ssl or tls, FALSE otherwise
//    options.Security = false;  // true zet ssl or tls aan
//});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<CarRentingProject_AppUser>>();
    DatabaseSeeder.Initialize(services, userManager);
}

var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("nl-BE")
.AddSupportedCultures(Language.AppSuppLang)
.AddSupportedUICultures(Language.AppSuppLang);

app.UseRequestLocalization(localizationOptions);

app.MapRazorPages();
app.UseMiddleware<SessionUser>();

app.Run();
