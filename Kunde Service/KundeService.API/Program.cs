using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using KundeService.Services;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Allow CORS
	var AllowSomeStuff = "_AllowSomeStuff";
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(name: AllowSomeStuff, builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
	});

	// Create globally available HttpClient for accessing the gateway
	builder.Services.AddHttpClient("gateway", client =>
	{
		client.BaseAddress = new Uri("http://gateway:4000/");
		client.DefaultRequestHeaders.Add(
			HeaderNames.Accept, "application/json");
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
			Title = "Kunde Service",
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
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kunde Service");
	});

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();
	app.MapRazorPages();

	app.Run();

}
catch (Exception ex)
{
	logger.Error(ex, "Stopped program because of exception");
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}