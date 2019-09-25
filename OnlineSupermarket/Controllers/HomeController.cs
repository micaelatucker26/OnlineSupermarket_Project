using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSupermarket.Models;
using OnlineSupermarket.Models.Entities;
using OnlineSupermarket.Services;
/// <summary>
/// The home controller manages the following methods and their responsibilities:
/// - The main method creates a method for the homepage when clicking the Home button.
/// - The index method accepts a profile object and an item object that can help determine whether a profile exists.It controls 
/// all main shopping code to allow admins permission to access the Index Item page. Shoppers are directed to a page that displays 
/// all products to buy.
/// - The about method contains a ways to load a page with information about the application.
/// </summary>
namespace OnlineSupermarket.Controllers
{
    // The Home Controller is authorized to allow a user with a specific role to view certain pages only if they are logged in.
    [Authorize]
    public class HomeController : Controller
    {
        // This creates a variable to hold your repository data.
        public IAuthenticationRepository _repo;
        // Inject the repository into the home controller.
        public HomeController(IAuthenticationRepository repo)
        {
            // Set the repo to what's being injected into the controller.
            _repo = repo;
        }

        // This creates a method for the homepage when clicking the Home button.
        public IActionResult Main()
        {
            // returns that homepage 
            return View();
        }
 
        // The index method accepts a profile object and an item object that can 
        // help determine whether a profile exists.
        public IActionResult Index(Profile profile)
        {
            // A user is read in by their name of their role.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // A userId is read from the Identity user.
            var userId = _repo.ReadApplicationUser(user.User.Id);
            // If the user is an Admin...
            if (user.HasRole("Admin"))
            { 
                // It returns the item index page.
                return RedirectToAction("Index", "Item");
            }
            // Else, if there is a profile already created...
            else if(profile != null)
            {
                // The items displayed in Main Shopping are loaded and displayed when a shopper logs in.
                var model = _repo.ReadAllItems();
                return View(model);  
            }
            // Else, it redirects to the create profile page.
            else
            {
                return RedirectToAction("Create", "Profile");
            }
        }

        // This method contains a way to load a page with information about the application.
        public IActionResult About()
        {
            // returns that about page
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
