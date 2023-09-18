using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sub4lazar.Models;
using sub4lazar.Utilities;

namespace sub4lazar.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Subscribe(User user)
    {
        User newUser = new User(user.email);

        bool isUserValid = RegisterUserUtility.validateUser(newUser);
        System.Console.WriteLine("isUserValid=" + isUserValid.ToString());

        if (isUserValid) {
            RegisterUserUtility.uploadUserToDB(newUser);
            RegisterUserUtility.sendVerificationEmail(newUser);
        }

        // System.Console.WriteLine("User created:");
        // System.Console.WriteLine(newUser.email);
        // System.Console.WriteLine(newUser.verified);
        // System.Console.WriteLine(newUser.subscribeCode);
        // System.Console.WriteLine(newUser.unsubscribeCode);

        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
