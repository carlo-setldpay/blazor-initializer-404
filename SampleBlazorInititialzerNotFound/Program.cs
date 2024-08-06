using Microsoft.AspNetCore.Authentication.Cookies;
using SampleBlazorInititialzerNotFound.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(x =>
    {
        x.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                return Task.CompletedTask;
            }
        };
    });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.Run();