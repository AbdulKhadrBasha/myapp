using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using myapp.Models;

namespace myapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        try {            
            ViewBag.OSVersion = Environment.OSVersion;
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            // Read the current CPU usage percentage
            float cpuUsage = cpuCounter.NextValue();

            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

            // Read the available memory in MB
            float availableMemory = memCounter.NextValue();

            // Wait for a moment to get a more accurate reading
            System.Threading.Thread.Sleep(1000);

            // Read CPU usage percentage again
            cpuUsage = cpuCounter.NextValue();
            ViewBag.CpuUsage = cpuUsage;
            ViewBag.MemUsage = availableMemory;
        }
        catch(Exception ex) { 
          var error = ex.ToString();
        }       

        return View();
    }

    public IActionResult Privacy()
    {
        var log = new LogInfo(_configuration);

        var data = log.LogData();
        ViewBag.Data = data;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
