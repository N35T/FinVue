using FinVue.Api.Endpoints;
using FinVue.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables();
    
builder.Services.AddDataServices(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("finvue_user", policy => 
        policy.RequireRole("FINVUE_USER"));

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    await scope.EnsureDatabaseOnStartupAsync(app.Environment.IsDevelopment());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomEndpoints();

app.Run();
