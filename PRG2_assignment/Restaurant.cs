using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

        public Queue<Order> OrderItemList { get; set; }
        public List<Menu> MenuItemList { get; set; }
        public List<SpecialOffer> SpecialOfferList { get; set; }
        


        public Restaurant()
        {
            RestaurantId = "";
            RestaurantName = "";
            RestaurantEmail = "";
            OrderItemList = new Queue<Order>();
            MenuItemList = new List<Menu>();
            SpecialOfferList = new List<SpecialOffer>();
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName= restaurantName;
            RestaurantEmail= restaurantEmail;

            OrderItemList = new Queue<Order>();
            MenuItemList = new List<Menu>();
            SpecialOfferList = new List<SpecialOffer>();
        }

        public void DisplayOrders()
        {
            int count = 1;
            Console.WriteLine($"--- {RestaurantName}'s Orders ---");
            foreach (Order item in OrderItemList)
            {
                Console.WriteLine($"Position {count} - {item}");
                count ++;
            }
        }

        public void DisplaySpecialOffers()
        {
            Console.WriteLine($"--- {RestaurantName}'s Special Offers ---");
            foreach (SpecialOffer item in SpecialOfferList)
            {
                Console.WriteLine(item);
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine($"--- {RestaurantName}'s Menu ---");
            foreach (Menu item in MenuItemList)
            {
                Console.WriteLine(item);
            }
        }

        public void AddMenu(Menu menu)
        {
            MenuItemList.Add(menu);
        }

        public bool RemoveMenu(Menu menuItem)
        {
            if (MenuItemList.Contains(menuItem))
            {
                MenuItemList.Remove(menuItem);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Restaurant ID: {RestaurantId}, Restaurant Name: {RestaurantName}, Restaurant Email: {RestaurantEmail}";
        }
    }
}
