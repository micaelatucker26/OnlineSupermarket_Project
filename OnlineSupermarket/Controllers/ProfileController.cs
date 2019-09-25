using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSupermarket.Models;
using OnlineSupermarket.Models.Entities;
using OnlineSupermarket.Services;

/// <summary>
/// The profile controller manages the following methods and their responsibilities:
/// The GET and POST create methods allow a profile to be created.
/// The details method shows the profile details.
/// The GET and POST edit method allows a shopper to edit their profile.
/// The GET and POST delete method allows a shopper to delete their profile.
/// <summary>
namespace OnlineSupermarket.Controllers
{
    public class ProfileController : Controller
    {
        // This creates a variable to hold your repository data.
        private IAuthenticationRepository _repo;
        // Inject the repository into the item controller. 
        public ProfileController(IAuthenticationRepository repo)
        {
            // Set the repo to what's being injected into the controller.
            _repo = repo;
        }
        // This method loads the profile create method.
        public IActionResult Create()
        {
            // A user is read in by their role name.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // A userId is read from the Identity user.
            var userId = _repo.ReadApplicationUser(user.User.Id);
            // If the userId is there...
            if (userId != null)
            {
                // redirect to the home index page
                return RedirectToAction("Index", "Home");
            }
            // else, return the create View
            return View();
        }

        // This method creates a profile.
        [HttpPost]
        public IActionResult Create(Profile profile) // ApplicationUser appUser
        {
            // A user is read in by their role name.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // The model is checked for valid data.
            if (ModelState.IsValid)
            {
                // The profile id is set to the identity user's id.
                profile.Id = user.User.Id;
                // A profile is created.
                _repo.CreateProfile(profile);
                // The page is redirected to the main shopping page.
                return RedirectToAction("Index", "Home");
            }
            // all appplication user data in the view is set to the profile id data being read in.
            ViewData["ApplicationUser"] = _repo.ReadProfile(profile.Id);
            // The index view is displayed.
            return View(profile);
        }

        // This method shows the profile details.
        public IActionResult Details()
        {
            // A user is read in by their role name.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // A profile is read in from that user.
            var profile = _repo.ReadProfile(user.User.Id);
            // if the profile is not there...
            if (profile == null)
            {
                // it redirects to the main shopping page
                return RedirectToAction("Index", "Home");
            }
            // else, it returns the details View
            return View(profile);
        }

        // This method loads the edit profile page.
        public IActionResult Edit()
        {
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // An profile is read by its id.
            var profile = _repo.ReadProfile(user.User.Id);
            // The profile is checked if it is available or not.
            if (profile == null)
            {
                // The page redirects back to the index page.
                return RedirectToAction("Index", "Home");
            }
            // An orchestra edit view is displayed.
            return View(profile);
        }

        // This is the edit POST method.
        [HttpPost]
        public IActionResult Edit(Profile profile)
        {
            //var user = _repo.ReadApplicationUser(User.Identity.Name);
            //var userProfile = _repo.ReadProfile(user.User.Id);
            // The model is checked for valid data.
            if (ModelState.IsValid)
            {
                //profile.Id = user.User.Id;
                // The profile is updated by its id.
                _repo.UpdateProfile(profile);
                // It returns back to the index page.
                return RedirectToAction("Index", "Home");
            }
            // An edit view is displayed.
            return View(profile);
        }
        
        // The delete method is here.
        public IActionResult Delete(string userId)
        {
            // An profile is read in by its id.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            var userProfile = _repo.ReadProfile(user.User.Id);
            // The orchestra is checked if it's available.
            if (userProfile == null)
            {
                // If it's not available, the page redirects.
                return RedirectToAction("Index", "Home");
            }
            // The delete view is displayed.
            return View(userProfile);
        }

        // This is the orchestra delete confirmation method.
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string userId)
        {
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            var userProfile = _repo.ReadProfile(user.User.Id);
            // An orchestra is deleted by its id.
            _repo.DeleteProfile(userId);
            // The page redirects back to the index page.
            return RedirectToAction("Index", "Home");
        }
    }
}