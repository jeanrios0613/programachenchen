using System.Configuration;
using System.Data;
using System.Diagnostics;
using FluentAssertions.Common;
using managerelchenchenvuelve.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace managerelchenchenvuelve.Controllers
{
    public class HomeController: Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Ha ocurrido un error inesperado. Por favor, comuníquese con el administrador del sistema."
            };
            return View(errorViewModel);
        }
    }
}
