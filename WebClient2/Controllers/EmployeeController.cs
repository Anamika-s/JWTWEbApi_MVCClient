using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClient2.Models;

namespace WebClient2.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            if(TempData["msg"]!=null)
            {
                ViewBag.msg = TempData["msg"];
            }
            List<Employee> employees = new List<Employee>();

        using (var client = new HttpClient())
            {
                string endpoint = "https://localhost:44371/api/";
                client.BaseAddress = new Uri(endpoint);

                HttpResponseMessage response = await client.GetAsync("Employees");
                if(response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    employees = JsonConvert.DeserializeObject<List<Employee>>(result);
                    return View(employees.ToList());
                }
                else if(employees.Count==0)
                {
                    ViewBag.msg = "No Records";
                }

                return View();

            }
            
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                Employee employee = null;
                using (var client = new HttpClient())
                {
                    string endpoint = "https://localhost:44371/api/";
                    client.BaseAddress = new Uri(endpoint);
                    HttpResponseMessage response = await client.GetAsync($"Employees/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        employee = new Employee();
                        employee = JsonConvert.DeserializeObject<Employee>(result);
                        return View(employee);
                    }

                }
            }
            else
            {
                ViewBag.msg = "Plaese provide a valid ID";
                return View();
            }

            return View();
            }
             

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            Employee employee = new Employee();
            return View(employee);
        }

        [HttpPost]
         public async Task<ActionResult> Create (Employee employee)
        {
            using (var client = new HttpClient())
            {
                string endpoint = "https://localhost:44371/api/";
                client.BaseAddress = new Uri(endpoint);

                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("Employees", content);
                if(response.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Record inserted";
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Id", "Id Already Exist");
                    return View();
                }
                else
                {
                    TempData["msg"] = response.StatusCode;
                    return RedirectToAction("Index");
                }


            }
        }


        

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}


