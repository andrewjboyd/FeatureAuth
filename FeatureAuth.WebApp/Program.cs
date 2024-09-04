using FastEndpoints;
using FeatureAuth;
using FeatureAuth.WebApp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoModule;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Clear();
            context.Response.StatusCode = 401;

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddFeatureAuthDetails<DemoAuth>();

builder.Services.AddToDoModule();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication()
   .UseAuthorization();

app.MapGet("/login", ([FromQuery] string? returnUrl) =>
{
    const string UserGuid = "fba283a4-51df-4e71-932e-2be40f01c4ab";
    var claims = new List<Claim>
        {
            new(ClaimTypes.Name, UserGuid),
            new("folderList", "1"),
            new("DemoAuth", "1"),
        };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    return Results.SignIn(principal, new AuthenticationProperties
    {
        // This should be white listed
        RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/",
    });
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync();

    context.Response.Redirect("/");
});

app.MapGet("/featureAuth", (IFeatureAuthRepository endpointRepository) =>
{
    return endpointRepository.GetDetails();
});

app.MapGet("/", (ClaimsPrincipal claimsPrincipal) =>
{
    return new
    {
        claimsPrincipal.Identity?.IsAuthenticated,
        claims = claimsPrincipal.Claims.Select(c => new { c.Type, c.Value })
    };
});

app.MapGet("/test", [EndpointId<DemoAuth>(DemoAuth.Read)] () =>
{
    return "Hello world";
});

app.UseFastEndpoints();

app.Run();