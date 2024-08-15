using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB_API_Business_Tier.Models;

namespace SimpleWebApplication.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return PartialView();
    }
}

