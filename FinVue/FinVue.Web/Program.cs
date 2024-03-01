using System.Net;
using FinVue.Data;
using FinVue.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

builder.Services.AddDataServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    await scope.EnsureDatabaseOnStartupAsync(app.Environment.IsDevelopment());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePages(context => {
    var response = context.HttpContext.Response;
    if (response.StatusCode == (int) HttpStatusCode.Unauthorized) {
        response.Redirect($"{app.Configuration["IdentityUrl"]}/auth/login?returnUrl={app.Configuration["AuthReturnUrl"]}");
    }

    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>();

app.Run();