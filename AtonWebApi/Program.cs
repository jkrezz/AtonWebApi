using AtonWebApi.Application.Dto;
using AtonWebApi.Domain.Repositories;
using AtonWebApi.Extensions;
using PlanTime.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddSwaggerWithAuth();

builder.Services.AddApplication();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aton API v1"); });
}

await InitializeDatabase(app.Services);

async Task InitializeDatabase(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    await repo.InitializeDatabase();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();