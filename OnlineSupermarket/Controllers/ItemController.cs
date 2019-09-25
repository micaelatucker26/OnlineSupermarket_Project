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
/// The item controller manages the following methods and their responsibilities:
/// The GET and POST create methods allow an item to be created.
/// The item index method that displays all items for an admin.
/// The details method shows the item details given an item id.
/// The GET and POST edit method allows an admin to edit an item by its id.
/// The GET and POST buy method allows an admin to buy an item given an itemId.
/// <summary>
namespace OnlineSupermarket.Controllers
{
    // The Item Controller is authorized to allow a user with an admin role to view item crud.
    [Authorize]
    public class ItemController : Controller
    {
        // This creates a variable to hold your repository data.
        public IAuthenticationRepository _repo;
        // Inject the repository into the item controller.
        public ItemController(IAuthenticationRepository repo)
        {
            // Set the repo to what's being injected into the controller.
            _repo = repo;
        }

        // This method allows an item to be created.
        public IActionResult Create()
        {
            // The user is read in by their role name.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // if they are an admin...
            if(user.HasRole("Admin"))
            {
                // the item create page is displayed
                return View();
            }
            // else, it returns back to the main shopping page
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // This is the POST create profile method. 
        [HttpPost]
        public IActionResult Create(Item item)
        {
            // if the model data is valid...
            if(ModelState.IsValid)
            {
                // create an item using an item object passed into this method
                _repo.CreateItem(item);
                // redirect back to the item index page
                return RedirectToAction("Index", "Item");
            }
            // else, display this page
            return View(item);
        }
   


        // This is the item index method that displays all items for an admin.
        public IActionResult Index()
        {
            // if the user is authenticated...
            if (User.Identity.IsAuthenticated)
            {
                // a user is read in by their role name
                var user = _repo.ReadApplicationUser(User.Identity.Name);
                // if the user is an admin...
                if (user.HasRole("Admin"))
                {
                    // all items are read from the repository and displayed
                    var model = _repo.ReadAllItems();
                    return View(model);
                }
                // else, it redirects back to the main shopping page
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            // else, the page is displayed
            return View();
        }

        // This method shows the item details given an item id.
        public IActionResult Details([Bind(Prefix = "id")] int id)
        {
            // an item is read in by its id
            var item = _repo.ReadItem(id);
            // if the item is not there...
            if (item == null)
            {
                // it redirects back to the item index page
                return RedirectToAction("Index", "Item");
            }
            // return the page 
            return View(item);
        }

        // This method allows an admin to edit an item by its id.
        public IActionResult Edit(int id)
        {
            // a user is read in by its role name
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // if the user is an admin...
            if (user.HasRole("Admin"))
            {
                // an item is read in by its id
                var item = _repo.ReadItem(id);
                // if the item is not there...
                if (item == null)
                {
                    // return the item index page
                    return RedirectToAction("Index", "Item");
                }
                // else display the page of the specific item
                return View(item);
            }
            // else if the user isn't an admin, redirect to the home index page
            else if(!user.HasRole("Admin"))
            {
                // return to the home index page
                return RedirectToAction("Index", "Home");
            }
            // display the edit page given the specific user
            return View(user);
        }

        // The post edit for an item is in this controller.
        [HttpPost]
        public IActionResult Edit(Item item)
        {
            // This checks if the model's state matches what the database has.
            if (ModelState.IsValid)
            {
                // An item is updated from the repo data.
                _repo.UpdateItem(item.Id, item);
                // The page is redirected to the item index page.
                return RedirectToAction("Index", "Item");
            }
            // The page returns to the edit view.
            return View(item);
        }

        public IActionResult Delete(int id)
        {
            var item = _repo.ReadItem(id);
            if(item == null)
            {
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost, ActionName("delete")]
        public IActionResult DeleteConfirmed(int shoppingCartId, int id)
        { 
            _repo.RemoveItem(shoppingCartId,id);
            return RedirectToAction("Index");
        }

        // This is the GET method for buying an item given an itemId.
        public IActionResult Buy([Bind(Prefix = "id")] int itemId)
        {
            // an item is read in by its itemId
            var item = _repo.ReadItem(itemId);
            // if the item isn't there...
            if(item == null)
            {
                // redirect back to the home index page
                return RedirectToAction("Index", "Home");
            }
            // else, return the buy item View
            return View(item);
        }

        // This is the POST method for buying an item given a shoppingCartId and an item object.
        [HttpPost]
        public IActionResult Buy(int shoppingCartId, Item item)
        {
            // This checks if the model's state matches what the database has.
            if (ModelState.IsValid)
            {
                // the itemId is set to zero by default
                item.Id = 0;
                // The buy item method is called from the repository.
                _repo.BuyItem(shoppingCartId, item);
                // It returns back to the home index
                return RedirectToAction("Index", "Home");
            }
            // The buy item page is displayed
            return View(item);
        }
    }
}
