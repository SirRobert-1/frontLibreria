using frontendnet.Middlewares;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var UrlWebApi = builder.Configuration["UrlWebApi"];
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<EnviaBearerDelegatingHandler>();

builder.Services.AddTransient<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<AuthClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
});

builder.Services.AddHttpClient<GenerosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
}).AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<UsuariosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
}).AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<RolesClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
}).AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<LibrosClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
}).AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddHttpClient<PerfilClientService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UrlWebApi!);
}).AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
.AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "frontendnet";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LoginPath = "/Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();