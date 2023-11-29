using CuaHangDoAn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CuaHangDoAn.Data;
using AspNetCoreHero.ToastNotification;
using CuaHangDoAn.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOptions();                                        // Kích hoạt Options
//var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
//builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject

builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Identity/Account/Login";
    opt.LogoutPath = "/Identity/Account/Logout";
    opt.AccessDeniedPath = "/";
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CuaHangDoAnContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectString"));

});


builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
        googleOptions.CallbackPath = "/dang-nhap-tu-google";

    });


builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<CuaHangDoAnContext>();

//builder.Services.AddIdentity<AppUser, IdentityRole>().
//    AddEntityFrameworkStores<CuaHangDoAnContext>().
//    AddDefaultTokenProviders();

builder.Services.AddDefaultIdentity<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<CuaHangDoAnContext>();
//builder.Services.AddDefaultIdentity<AppUser>().
//    AddEntityFrameworkStores<CuaHangDoAnContext>().
//    AddDefaultTokenProviders();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});



app.Run();
