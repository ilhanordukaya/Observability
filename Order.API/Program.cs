using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Order.API.Opentelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpentelemetryConstans>(builder.Configuration.GetSection("Opentelemetry"));

var opentelemetryConstans = (builder.Configuration.GetSection("Opentelemetry").Get<OpentelemetryConstans>())!;
builder.Services.AddOpenTelemetry().WithTracing(options=>

 {
	 
	 options.AddSource(opentelemetryConstans.ActivitySourceName)
	 .ConfigureResource(resource =>
	 {
		 resource.AddService(opentelemetryConstans.ServiceName, serviceVersion:opentelemetryConstans.ServiceVersion);
	 });
	 options.AddAspNetCoreInstrumentation();
	 options.AddConsoleExporter();
	 options.AddOtlpExporter(); //jaeger

 });
ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(opentelemetryConstans.ActivitySourceName);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
