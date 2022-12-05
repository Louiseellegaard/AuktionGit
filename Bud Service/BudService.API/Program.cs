using Microsoft.OpenApi.Models;

using BudService.Services;

var builder = WebApplication.CreateBuilder(args);

// Allow CORS
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bud Service",
        Version = "v1"
    });
});

// Add internal services to the application
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IDbContext, DbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bud Service");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
