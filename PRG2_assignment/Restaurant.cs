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

        public Queue<Order> OrderList { get; set; }
        public List<Menu> MenuList { get; set; }
        public List<SpecialOffer> SpecialOfferList { get; set; }
        


        public Restaurant()
        {
            RestaurantId = "";
            RestaurantName = "";
            RestaurantEmail = "";
            OrderList = new Queue<Order>();
            MenuList = new List<Menu>();
            SpecialOfferList = new List<SpecialOffer>();
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName= restaurantName;
            RestaurantEmail= restaurantEmail;

            OrderList = new Queue<Order>();
            MenuList = new List<Menu>();
            SpecialOfferList = new List<SpecialOffer>();
        }

        public void DisplayOrders()
        {
            int count = 1;
            Console.WriteLine($"--- {RestaurantName}'s Orders ---");
            foreach (Order item in OrderList)
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
            Console.WriteLine($"--- {RestaurantName}'s Menus ---");
            foreach (Menu item in MenuList)
            {
                Console.WriteLine(item);
            }
        }

        public void AddMenu(Menu menu)
        {
            MenuList.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            if (MenuList.Contains(menu))
            {
                MenuList.Remove(menu);
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
