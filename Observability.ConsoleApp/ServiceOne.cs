using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observability.ConsoleApp
{
	internal class ServiceOne
	{
		static HttpClient httpClient=new HttpClient();
		internal async Task<int> MakeRequestGoogle()
		{
			using var activity = ActivitySourceProvider.Source.StartActivity
				(kind:System.Diagnostics.ActivityKind.Producer,name:"CustomMakeRequestToGoogle");
			var eventTags = new ActivityTagsCollection();
			
			activity.AddEvent(new("google istek", tags: eventTags));
			var result = await httpClient.GetAsync("https://www.google.com");
			var responseContent=await result.Content.ReadAsStringAsync();
			eventTags.Add("googlebody length", responseContent.Length);
			activity.AddEvent(new("google istek tamamlandı", tags: eventTags));
			return responseContent.Length;
		}
	}
}
