using Microsoft.OpenApi.Models;

using VareApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vare Service", Version = "v1" });
});

builder.Services.AddSingleton<IDataService, DataService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vare Service");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();