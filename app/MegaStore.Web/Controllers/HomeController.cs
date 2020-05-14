using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MegaStore.Web.Models;
using MegaStore.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace MegaStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TelemetryClient _telemetryClient;

        public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            _telemetryClient.TrackEvent("Home Page Requested");

            CreateSale();

            var newSaleMsg = $"New sale created at: {DateTime.Now} on host {Environment.MachineName}";
            _telemetryClient.TrackTrace(newSaleMsg);
            Console.WriteLine(newSaleMsg);

            ViewData["Env"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An error occured in the Home Page");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // This code is modified from https://github.com/sixeyed/docker-on-windows
        public void CreateSale()
        {
            var sale = new Sale()
            {
                CreatedOn = DateTime.Now,
                Description = GetProduct()
                //Description = $" {Environment.MachineName}"
            };

            var eventMessage = new SaleCreatedEvent
            {
                Sale = sale,
                CreatedAt = DateTime.Now
            };

            MessageQueue.Publish(eventMessage);
        }

        private string GetProduct()
        {
            Random rnd = new Random();
            var products = new List<string> { "Otter Bitter", "Otter Amber", "Otter Bright", "Otter Ale", "Otter Head", "Tarka Pure", "Tarka Four", "Yellow Hammer", "Port Stout", "Firefly Bitter", "Stormstay Ale" };
            int index = rnd.Next(products.Count);

            return products[index];
        }
    }
}
