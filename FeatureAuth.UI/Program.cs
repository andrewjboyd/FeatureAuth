using FeatureAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.RequireAuthenticatedSignIn = false;
    })
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Clear();
            context.Response.StatusCode = 401;

            return Task.CompletedTask;
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/login", ([FromQuery] string? returnUrl) =>
{
    const string UserGuid = "fba283a4-51df-4e71-932e-2be40f01c4ab";
    var claims = new List<Claim>
        {
            new(ClaimTypes.Name, UserGuid),
        };

    var identity = new ClaimsIdentity(claims);
    var principal = new ClaimsPrincipal(identity);

    return Results.SignIn(
        principal,
        new AuthenticationProperties
        {
            RedirectUri = returnUrl ?? "/",
        },
        CookieAuthenticationDefaults.AuthenticationScheme
    );
});

app.MapGet("/logout", () => Results.SignOut(new AuthenticationProperties
{
    RedirectUri = "/"
}));

app.MapGet("/featureAuth", (IFeatureAuthRepository endpointRepository) =>
{
    return endpointRepository.GetDetails();
});

app.Run();