using AnketSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;




namespace AnketSitesi.Controllers
{
	public class HomeController : Controller
	{
	



		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Home2()
		{
			return View();
		}
      

        public IActionResult Privacy()
		{
			return View();
		}

	
       

        public IActionResult Surveys() 
		{
			return View();
		}
      
    
        public IActionResult Index2()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	

	}
}