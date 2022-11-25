using Microsoft.OpenApi.Models;

using VareService.Services;

var builder = WebApplication.CreateBuilder(args);

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
        Title = "Vare Service",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IDbContext, DbContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vare Service");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages(); 

app.Run();
