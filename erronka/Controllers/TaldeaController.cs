using erronka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace erronka.Controllers
{
    public class TaldeaController : Controller
    {
        private readonly ILogger<TaldeaController> _logger;
        private Uri rutaMusika = new Uri("http://localhost:8095/musika");

        public TaldeaController(ILogger<TaldeaController> logger)
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


        //Xehetasunak
        public async Task<IActionResult> taldeaXehetasunak(string id)
        {
            Taldea taldea = new Taldea();
            Uri rutaXehetasunak = new Uri(rutaMusika + "/" +id);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(rutaXehetasunak))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    taldea = JsonConvert.DeserializeObject<Taldea>(apiResponse);
                }
            }
            return View(taldea);
        }

        //Berria sortu
        public IActionResult taldeaGehitu()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> taldeaGehitu([Bind("id,izena,sorrera,weborria")] Taldea taldea)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(taldea), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(rutaMusika, content);
                    response.EnsureSuccessStatusCode();

                    //fake rest api honetan ez dira benetan alta ematen. 
                    //Benetako rest api batean linea hau utzi eta hurrengo hiruak kendu
                    return RedirectToAction("Index");

                    //alta eman dela baieztatzeko
                    //var data = await response.Content.ReadAsStringAsync();
                    //Taldea altaTaldea = JsonConvert.DeserializeObject<Taldea>(data);
                    //return RedirectToAction("TodoBaieztatu", altaTaldea);
                }
            }
            return View(taldea);
        }

        // Formularioa datuekin
        public async Task<IActionResult> taldeaAldatu(Taldea taldea)
        {
            if (taldea.id == null)
            {
                return NotFound();
            }
            Taldea taldea2 = new Taldea();
            Uri rutaAldatu = new Uri(rutaMusika, taldea.id.ToString());

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(rutaAldatu))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    taldea2 = JsonConvert.DeserializeObject<Taldea>(apiResponse);
                }
            }
            return View(taldea);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> todoAldatu(string id, [Bind("id,izena,sorrera,weborria")] Taldea taldea)
        {
            if (id != taldea.id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Uri rutaAldatu = new Uri(rutaMusika, id.ToString());
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(taldea), Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync(rutaAldatu, content);
                    response.EnsureSuccessStatusCode();

                    //Aldaketa ondo egin dela baieztatzeko
                    string data = await response.Content.ReadAsStringAsync();
                    Taldea aldatuTaldea = JsonConvert.DeserializeObject<Taldea>(data);
                    //return RedirectToAction("TodoBaieztatu", aldatuTaldea);
                }
            }
            return View(taldea);
        }

        //ezabatu
        //ezabatu nahi dena erakutsi eta konfirmazioa eskatu
        public async Task<IActionResult> taldeaEzabatu(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Taldea taldea = new Taldea();
            Uri rutaAldatu = new Uri(rutaMusika, id.ToString());
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(rutaAldatu))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    taldea = JsonConvert.DeserializeObject<Taldea>(apiResponse);
                }
            }
            return View(taldea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> todoEzabatu(string id)
        {
            Uri rutaAldatu = new Uri(rutaMusika, id.ToString());
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(rutaAldatu))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index"); //todoBaieztatu-ra ere joan gintezke
        }



    }
}
