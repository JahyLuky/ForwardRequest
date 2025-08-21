using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

// Read the "Urls" configuration from appsettings.json
var urls = builder.Configuration["Urls"];

builder.WebHost.UseUrls(urls);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
