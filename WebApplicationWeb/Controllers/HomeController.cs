using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplicationWeb.Models;

namespace WebApplicationWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = new();
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:34913/")
            };//HttpClient for connecting with api
            HttpResponseMessage response = await client.GetAsync("api/employee");
            if (response.IsSuccessStatusCode)//IsSuccessStatusCode means response = 200(means query execured successfully) checked by throw postman
            {
                var results = response.Content.ReadAsStringAsync().Result;//Result ko read kar string me convert kar get kar liya
                employees = JsonConvert.DeserializeObject<List<Employee>>(results); //list of data me convert karne ke liye Json Formate ka use kar liya
            }
            return View(employees);
        }

        public async Task<IActionResult> Details(int Id)
        {
            Employee employees = await GetEmployeeByID(Id);
            return View(employees);
        }

        private static async Task<Employee> GetEmployeeByID(int Id)
        {
            Employee employees = new();
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:34913/")
            };
            HttpResponseMessage response = await client.GetAsync($"api/employee/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<Employee>(results);
            }
            return employees;
        }

        public async Task<IActionResult> Delete(int Id)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:34913/")
            };
            HttpResponseMessage response = await client.DeleteAsync($"api/employee/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:34913/") //BaseAddress prop ko Uri ke throw Api url bata dia jo hamein automatically Api se connect karwaegi
            };
            var response = await client.PostAsJsonAsync("api/employee", employee);//replace HttpResponseMessage use always var because we don't know which type of we got
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Employee employees = await GetEmployeeByID(Id);
            return View(employees);
        }

        [HttpPost]//Api ke ander hamare pass Put and Delete dono hai isliye edit ka put ke liye alag se method banana pada
        public async Task<IActionResult> Edit(Employee employee)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:34913/") //BaseAddress prop ko Uri ke throw Api url bata dia jo hamein automatically Api se connect karwaegi
            };//same code of create post
            var response = await client.PutAsJsonAsync($"api/employee/{employee.Id}", employee);//replace HttpResponseMessage use always var because we don't know which type of we got
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
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
