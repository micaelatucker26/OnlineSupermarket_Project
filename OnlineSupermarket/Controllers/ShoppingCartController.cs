using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineSupermarket.Models;
using OnlineSupermarket.Services;

/// <summary>
/// The shopping cart controller manages the following methods and their responsibilities:
/// The index method shows all shopping cart items and allows those items to be deleted.
/// The GET and POST empty method allows a shopper to empty their shopping cart by displaying a page to 
/// empty cart and confirming that action.
/// The GET and POST remove method allows a shopper to remove an item from their cart.
/// The GET and POST checkout method allows a shopper to checkout and confirm that they wanna checkout.
/// <summary>
namespace OnlineSupermarket.Controllers
{
    public class ShoppingCartController : Controller
    {
        // This creates a variable to hold your repository data.
        private IAuthenticationRepository _repo;
        // Inject the repository into the shopping cart controller.
        public ShoppingCartController (IAuthenticationRepository repo)
        {
            // Set the repo to what's being injected into the controller.
            _repo = repo;
        }


        // This method shows all shopping cart items and allows those items to be deleted.
        public IActionResult Index(int shoppingCartId, int itemId) 
        {
            // A user is read in by their name of their role.
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // if the user is an admin...
            if (!user.HasRole("Admin"))
            {
                // the shopping cart is read in by the shopping cart Id
                var model = _repo.ReadShoppingCart(shoppingCartId);
                // the item is read by its id
                var itemData = _repo.ReadItem(itemId);
                // the item data in the view is set to the variable, itemData
                ViewData["Item"] = itemData;
                // the page returns with all items displayed
                return View(itemData);
            }
            // else, the page redirects to the home index page
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // This method allows a shopper to empty their shopping cart by displaying a page to empty cart.
        public IActionResult Empty([Bind(Prefix = "id")] int shoppingCartId, int itemId)
        {
            // The shopping cart is read in by its id.
            var shoppingCart = _repo.ReadShoppingCart(shoppingCartId);
            // if the shopping cart is not there...
            if(shoppingCart == null)
            {
                // redirect to the home index page
                return RedirectToAction("Index", "Home");
            }
            // use eager loading to find the shopping cart item by its id
            var item = shoppingCart.Item.FirstOrDefault(i => i.Id == itemId);
            // if the item is not there...
            if(item == null)
            {
                // redirect back to the shopping cart index page
                return RedirectToAction("Index", "ShoppingCart");
            }
            // else, just display the View
            return View(shoppingCart);
        }

        // This method empties a shopping cart.
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int shoppingCartId, int itemId)
        {
            // The shopping cart is emptied.
            _repo.EmptyShoppingCart(shoppingCartId, itemId);
            // It redirects back to the home index page.
            return RedirectToAction("Index", "Home");
        }

        // This method allows a shopper to remove an item.
        public IActionResult Remove([Bind(Prefix = "id")] int shoppingCartId, int itemId)
        {
            // the shopping cart is read in by its id
            var shoppingCart = _repo.ReadShoppingCart(shoppingCartId);
            // if the shopping cart is not there...
            if(shoppingCart == null)
            {
                // it redirects to the home index page
                return RedirectToAction("Index", "Home");
            }
            // an item from a shopping cart is eager loaded by its id
            var item = shoppingCart.Item.FirstOrDefault(i => i.Id == itemId);
            // if the item is not there...
            if (item == null)
            {
                // it redirects to the shopping cart index page
                return RedirectToAction("Index", "ShoppingCart");
            }
            // else the remove item View is displayed
            return View(item);
        }

        // This creates a delete view for confirming a delete action on an item
        [HttpPost, ActionName("Delete")]
        public IActionResult ItemDeleteConfirmed(int shoppingCartId, int itemId)
        {
            // If the delete is confirmed, it deletes the item from the repository.
            _repo.RemoveItem(shoppingCartId, itemId);
            // It returns back to the shopping cart index page.
            return RedirectToAction("Index", "ShoppingCart");
        }

        // This method allows a shopper to checkout.
        public IActionResult Checkout([Bind(Prefix= "id")] int shoppingCartId, int userId )
        {
            // a user is read in by their role name
            var user = _repo.ReadApplicationUser(User.Identity.Name);
            // if they are an admin...
            if (!user.HasRole("Admin"))
            {
                // the shopping cart is read by its id
                var model = _repo.ReadShoppingCart(shoppingCartId);
                // the View data is set for the items
                ViewData["Item"] = _repo.ReadAllItems();
                // the checkout View is displayed
                return View(model);
            }
            // else, redirect to the shopping cart index
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }
        // This method allows a shopper to confirm that they wanna checkout.
        [HttpPost, ActionName("Delete")]
        public IActionResult CheckoutConfirmed( int shoppingCartId , int itemId )
        {
            // A shopper checks out given the shopping cart Id and the itemId
            _repo.Checkout(shoppingCartId, itemId);
            // it redirects to the home index page
            return RedirectToAction("Index", "Home");
        }
    }
}