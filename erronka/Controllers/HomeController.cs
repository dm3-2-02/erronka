using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using erronka.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace erronka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Uri rutaMusika = new Uri("http://localhost:8095/musika");


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Taldea> TodoList = new List<Taldea>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(rutaMusika))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    TodoList = JsonConvert.DeserializeObject<List<Taldea>>(apiResponse);
                }
            }
            return View(TodoList);
        }

        public IActionResult Privacy()
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
