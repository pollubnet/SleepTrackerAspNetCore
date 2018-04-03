#region License
/*
 * SleepTracker
 *
 * Written in 2016 by Marcin Badurowicz <m at badurowicz dot net>
 *
 * To the extent possible under law, the author(s) have dedicated
 * all copyright and related and neighboring rights to this 
 * software to the public domain worldwide. This software is 
 * distributed without any warranty. 
 *
 * You should have received a copy of the CC0 Public Domain 
 * Dedication along with this software. If not, see 
 * <http://creativecommons.org/publicdomain/zero/1.0/>. 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SleepTracker.Models;

namespace SleepTracker.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        // Akcja Index - domyślna akcja, która uruchamia się gdy ktoś wejdzie na stronę
        public IActionResult Index()
        {
            // ładujemy plik z /uploads/data.txt i go parsujemy
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var lines = System.IO.File.ReadAllLines(Path.Combine(uploads, "data.txt"));

            SleepData data = SleepData.Parse(lines);

            // i przekazujemy obiekt do widoku
            return View(data);
        }

        // Akcja Load - pokazuje formularz ładowania nowego pliku
        public IActionResult Load()
        {
            return View();
        }

        // Akcja Load - odbiera formularz ładowania pliku i zapisuje plik do /uploads/data.txt
        [HttpPost]
        public IActionResult Load(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var fName = "data.txt";

            using (var fs = new FileStream(Path.Combine(uploads, fName), FileMode.CreateNew, FileAccess.ReadWrite))
            {
                file.CopyTo(fs);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
