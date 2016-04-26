using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.AspNet.Hosting;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }


        public IActionResult Index()
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var lines = System.IO.File.ReadAllLines(Path.Combine(uploads, "data.txt"));

            SleepData data = SleepData.Parse(lines);

            return View(data);
        }

        public IActionResult Load()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Load(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var fName = "data.txt";
            file.SaveAs(Path.Combine(uploads, fName));

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
