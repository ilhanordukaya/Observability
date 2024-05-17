using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenTelemetry.Shared
{
	public static class OpenTelemetryExtensions
	{
		public static void AddOpenTelemetryExt(this IServiceCollection services, IConfiguration  configuration)
		{

			services.Configure<OpenTelemetryConstans>(configuration.GetSection("Opentelemetry"));
			var openTelemetryConstans = (configuration.GetSection("Opentelemetry").Get<OpenTelemetryConstans>())!;

			ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(openTelemetryConstans.ActivitySourceName);

			services.AddOpenTelemetry().WithTracing(options =>
			{
				
				options.AddSource(openTelemetryConstans.ActivitySourceName)
				.ConfigureResource(resource =>
				{
					resource.AddService(openTelemetryConstans.ServiceName, serviceVersion: openTelemetryConstans.ServiceVersion);
				});
				options.AddAspNetCoreInstrumentation(aspnetcoreOptions =>
				{
					aspnetcoreOptions.Filter = (context) =>
					{
						return (!string.IsNullOrEmpty(context.Request.Path.Value)) ? context.Request.Path.Value.Contains("api", StringComparison.InvariantCulture) : false;

					};
					aspnetcoreOptions.RecordException = true;
				});
				options.AddEntityFrameworkCoreInstrumentation(efcoreOptions =>
				{
					efcoreOptions.SetDbStatementForText = true;
					efcoreOptions.SetDbStatementForStoredProcedure = true;
					efcoreOptions.EnrichWithIDbCommand = (activity, dbCommand) =>
					{
						// Bilerek boş bırakıldı. Örnek göstermek için

					};
				});

				options.AddHttpClientInstrumentation(httpOptions =>
				{

					httpOptions.FilterHttpRequestMessage = (request) =>
					{
						return !request.RequestUri.AbsoluteUri.Contains("9200", StringComparison.InvariantCulture);
					};

					httpOptions.EnrichWithHttpRequestMessage = async (activity, request) =>
					{
						var requestContent = "empty";

						if (request.Content != null)
						{
							requestContent = await request.Content.ReadAsStringAsync();
						}
						activity.SetTag("http.request.body", requestContent);
					};

					httpOptions.EnrichWithHttpResponseMessage = async (activity, response) =>
					{
						if (response.Content != null)
						{
							activity.SetTag("http.response.body", await response.Content.ReadAsStringAsync());
						}
					};

				});

				options.AddRedisInstrumentation(redisOptions =>
				{
					redisOptions.SetVerboseDatabaseStatements = true;
				});
				options.AddConsoleExporter();
				options.AddOtlpExporter(); //jaeger

			});
		}
	}
}
