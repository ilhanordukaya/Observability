namespace Observability.ConsoleApp
{
	internal class ServiceTwo
	{
		internal async Task<int> WriteToFile(string filePath)
		{
		  using	var activity=ActivitySourceProvider.Source.StartActivity();
			await File.WriteAllTextAsync("myFile.txt",filePath);

			return (await File.ReadAllTextAsync("myFile.txt")).Length;
		}
	}
}