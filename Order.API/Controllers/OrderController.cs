using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
	[ApiController]
	[Route("[api/controller]")]
	public class OrderController : ControllerBase
	{
		[HttpGet]
		public IActionResult Create()
		{
			//var a =10;
			//var b = 0;

			//var c = a/b;

			return Ok();		}
	}
}
