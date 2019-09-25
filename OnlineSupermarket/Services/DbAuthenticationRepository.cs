using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineSupermarket.Data;
using OnlineSupermarket.Models;
using OnlineSupermarket.Models.Entities;

namespace OnlineSupermarket.Services
{
    public class DbAuthenticationRepository : IAuthenticationRepository
    {
        // add the applicationDbContext to use with holding repository data
        private ApplicationDbContext _db;
        // define the user manager to manage all users
        private UserManager<IdentityUser> _userManager;
        // Inject the selected Dbcontext and the user manager into the DbAuthenticationRepository
        public DbAuthenticationRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            // Set the database context to what's being passed in.
            _db = db;
            // Set the user manager to what's being passed in.
            _userManager = userManager;
        }



        // Authentication:------------------------------------------------------------------------------------------------

        // This method is for authentication purposes.
        public IQueryable<IdentityRole> ReadAllRoles()

        {
            return _db.Roles;
        }
        // This method is also for authentication purposes.
        public IQueryable<ApplicationUser> ReadAllUsers()
        {
            ICollection<ApplicationUser> appUsers = new List<ApplicationUser>();
            foreach (var user in _db.Users)
            {
                ApplicationUser usr = new ApplicationUser
                {
                    User = user
                };
                AddRoles(usr);
                appUsers.Add(usr);
            }
            return appUsers.AsQueryable();
        }
        // This method is also used for authentication purposes.
        private void AddRoles(ApplicationUser user)
        {
            var roleIds = _db.UserRoles.Where(ur => ur.UserId == user.User.Id).Select(ur => ur.RoleId);
            foreach (var roleId in roleIds)
            {
                user.Roles.Add(_db.Roles.Find(roleId));
            }
        }

        // This method reads in a user by their email.
        public ApplicationUser ReadApplicationUser(string email)
        {
            // The user is set to null by default.
            ApplicationUser appUser = null;
            // Using eager loading, a user is read by their email from the database.
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            // If the user is there...
            if (user != null)
            {
                // A user is created
                appUser = new ApplicationUser
                {
                    User = user
                };
                // A role is added to the user.
                AddRoles(appUser);
            }
            // The user is returned
            return appUser;
        }

        // This method assigns a role to a user based on their email and roleName
        public bool AssignRole(string email, string roleName)
        {
            // The user is set to null by default.
           // ApplicationUser appUser = null;
            // Using eager loading, a user is read by their email from the database.
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            // If the user is there...
            if (user != null)
            {
                // if the user's rolename is set...
                if (user.UserName != roleName)
                {
                    // the role is added to a user , using the user manager
                    _userManager.AddToRoleAsync(user, roleName).Wait();
                    // return true
                    return true;
                }
            }
            // else return false, the role is not added
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------------
        // End Authentication








       // Shopping Cart-----------------------------------------------------------------------------------------------------
       
        // This method reads a shopping cart by its id.
        public ShoppingCart ReadShoppingCart(int shoppingCartId)
        {
            // From the database, a shopping cart is returned by its shopping cart items and shopping cart id
            return _db.ShoppingCart.Include(s => s.Item).FirstOrDefault(s => s.Id == shoppingCartId);
        }

        // This method empties a shopping cart based on its id and an itemId
        public void EmptyShoppingCart( int shoppingCartId, int itemId)
        {
            // a shopping cart is read in by its id
            var shoppingCart = ReadShoppingCart(shoppingCartId);
            // if the shopping cart is there...
            if (shoppingCart != null)
            {
                // a shopping cart item is read in by its id
                var item = shoppingCart.Item.FirstOrDefault(i => i.Id == itemId);
                // if the item is there...
                if (item != null)
                {
                    // remove the item/items from the shopping cart
                    shoppingCart.Item.Remove(item);
                    // set the amount equal to the amount in stock + the quantity to buy
                    item.AmountInStock += item.QuantityToBuy;
                    // save changes
                    _db.SaveChanges();
                }
            }
        }

        // This method allows a shopper to checkout , given a shoppingCartId and an itemId.
        public void Checkout(int shoppingCartId, int itemId)
        {
            // A shopping cart is read in by its id
            var shoppingCart = ReadShoppingCart(shoppingCartId);
            // if the shopping cart is there...
            if (shoppingCart != null)
            {
                // a shopping cart item is read in by its id
                var item = shoppingCart.Item.FirstOrDefault(i => i.Id == itemId);
                // if the item is there...
                if (item != null)
                {
                    // The item/items is/are removed from the shopping cart.
                    shoppingCart.Item.Remove(item);
                    // Save changes to the database
                    _db.SaveChanges();
                }
            }
        }





        // Item------------------------------------------------------------------------------------------------------

        // This method allows an admin to create an item.
        public Item CreateItem(Item item)
        {
            // The item is added to the database.
            _db.Item.Add(item);
            // Changes are saved.
            _db.SaveChanges();
            // The item is returned.
            return item;
        }
        // This method allows an admin to read all items as a list to display
        public ICollection<Item> ReadAllItems()
        {
            return _db.Item.ToList();
        }

        // This method allows an item to be read in by its id.
        public Item ReadItem(int itemId)
        {
            return _db.Item.FirstOrDefault(i => i.Id == itemId);
        }

        // This method allows an item to be updated by its id and an item object.
        public void UpdateItem( int id, Item item)
        {
            // The item is read in by its id and set to oldItem
            var oldItem = ReadItem(id);
            // If that item is there...
            if(oldItem != null)
            {
                // All data fields are updated into the new item object.
                    oldItem.Name = item.Name;
                    oldItem.Type = item.Type;
                    oldItem.Price = item.Price;
                    oldItem.AmountInStock = item.AmountInStock;
                 // Changes are saved. 
                    _db.SaveChanges();
            }
        }

        // This method removes an item based on a shopping cart id and an itemId
        public void RemoveItem(int shoppingCartId, int itemId)
        {
            // A shopping cart is read in by its id
            var shoppingCart = ReadShoppingCart(shoppingCartId);
            // if the shopping cart is there...
            if (shoppingCart != null)
            {
                // a shopping cart item is read in by its id
                var item = shoppingCart.Item.FirstOrDefault(i => i.Id == itemId);
                // if the item is there....
                if(item != null)
                {
                    // The item/items is/are removed from the shopping cart.
                    shoppingCart.Item.Remove(item);
                    // The amount in stock is increased by the quantity to buy
                    item.AmountInStock += item.QuantityToBuy;
                    // Changes are saved to the database
                    _db.SaveChanges();
                }
            }
        }

        // This method allows a shopper to buy an item, given a shoppingCartId and an item object
        public Item BuyItem(int shoppingCartId, Item item)
        {
            // If the item's amount in stock is not equal to zero...
            if(item.AmountInStock != 0.0)
            {
                // a shopping cart is read in by its id
                var shoppingCart = ReadShoppingCart(shoppingCartId);
                // if the shopping cart is there...
                if(shoppingCart != null)
                {
                    // the item is added to the shopping cart
                    shoppingCart.Item.Add(item);
                    // the item in the shopping cart is set
                    item.ShoppingCart = shoppingCart;
                    // the amount in stock is decreased by the quantity to buy
                    item.AmountInStock = item.AmountInStock - item.QuantityToBuy;
                    // Changes are saved to the database
                    _db.SaveChanges();
                }
            }
            // an item is returned
            return item;
        }





        //-------------------------------------------------------------------------------------------------------------
        // Profile

        // This method allows a shopper to create a profile based on a profile object.
        public Profile CreateProfile( Profile profile)
        {
            // A profile is added to the database.
            _db.Profile.Add(profile);
            // Changes are saved.
            _db.SaveChanges();
            // Return the profile
            return profile;
        }

        // This method allows a profile to be read in by its Id
        public Profile ReadProfile(string profileId)
        {
            // The profile is returned by its id.
            return _db.Profile.FirstOrDefault(p => p.Id.Equals(profileId));
        }

        // This method allows a profile to be updated by a profile object.
        public void UpdateProfile(Profile profile)
        {
            // If the profile is there...
            if(profile != null)
            {
                // The old profile is set to the Profile being pulled from the database by its id
                var oldProfile = _db.Profile.FirstOrDefault(p => p.Id == profile.Id);
            
                // if the profile is found...
                if(oldProfile != null)
                    // All old data fields from that profile are set with new data
                    oldProfile.FirstName = profile.FirstName;
                    oldProfile.LastName = profile.LastName;
                    oldProfile.StreetAddress = profile.StreetAddress;
                    oldProfile.State = profile.State;
                    oldProfile.Zip = profile.Zip;
                // Changes are saved to the database
                    _db.SaveChanges();
            }
        }
        
        // This method allows a shopper to delete a profile by its id
        public void DeleteProfile(string userId)
        {
            // the profile is pulled from the database by its id
            Profile profile = _db.Profile.Find(userId);
            // the profile is removed from the database
            _db.Profile.Remove(profile);
            // changes are saved 
            _db.SaveChanges();
        }
    }
}
