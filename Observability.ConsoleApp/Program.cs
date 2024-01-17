
using Observability.ConsoleApp;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



using var traceProvider = Sdk.CreateTracerProviderBuilder().
  AddSource(OpentelemetryConstans.ActivitySourceName)
 .ConfigureResource(configure =>
{
	configure.AddService(OpentelemetryConstans.ServiceName, serviceVersion: OpentelemetryConstans.ServiceVersion).
		   AddAttributes(new List<KeyValuePair<string, object>>()
			 {   new KeyValuePair<string, object>("host.machineName", Environment.MachineName),
					new KeyValuePair<string, object>("host.enviroment", "dev")

			 });
}).AddConsoleExporter().AddOtlpExporter().Build();

var serviceHelper = new ServiceHelper();
await serviceHelper.Work1();



