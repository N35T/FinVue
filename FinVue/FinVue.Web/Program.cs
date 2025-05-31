using System.Net;
using FinVue.Core.Entities;
using FinVue.Core.Services;
using FinVue.Data;
using FinVue.Web.Components;
using FinVue.Web.Components.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.MapPost("/addCategory",
    async (CategoryService _categoryService, [FromForm]string catName, [FromForm]string catColor) => {
        var category = new Category(Guid.NewGuid().ToString(), catName, new Color(catColor));

        await _categoryService.AddCategoryAsync(category);
        return Results.Ok();
    });

app.Run();