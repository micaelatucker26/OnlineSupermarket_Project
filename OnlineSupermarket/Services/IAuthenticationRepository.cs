using Microsoft.AspNetCore.Identity;
using OnlineSupermarket.Models;
using OnlineSupermarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Services
{
    public interface IAuthenticationRepository
    {
        // Logic to read a user by their email
        ApplicationUser ReadApplicationUser(string email);
       // Logic to assign a role given an email and rolename
        bool AssignRole(string email, string roleName);


        // Logic to read a shopping cart by its id
        ShoppingCart ReadShoppingCart(int shoppingCartId);
        // Logic to empty a shopping cart, given its id and an item id
        void EmptyShoppingCart(int shoppingCartId, int itemId);
        // Logic to checkout , given a shopping cart id and an item id
        void Checkout(int shoppingCartId, int itemId);


        // Logic to create an item based on an item object
        Item CreateItem(Item item);
        // Logic to buy an item based on a shopping cart id and an item object
        Item BuyItem( int shoppingCartId, Item item);
        // Logic to read all items from a collection
        ICollection<Item> ReadAllItems();
        // Logic to read an item by its id
        Item ReadItem(int itemId);
        // Logic to update an item by its id and an item object
        void UpdateItem(int id, Item item);
        // Logic to remove an item using the shopping cart id and an item id
        void RemoveItem(int shoppingCartId, int itemId);


        // Logic to create a profile based on a profile object
        Profile CreateProfile(Profile profile);
        // Logic to read the profile by its id
        Profile ReadProfile(string profileId);
        // Logic to update the profile, using the profile object
        void UpdateProfile(Profile profile);
        // Logic to delete a profile byt its id
        void DeleteProfile(string userId);
    }
}
