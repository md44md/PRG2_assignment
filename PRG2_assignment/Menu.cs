using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }

        public List<FoodItem> FoodItemList { get; set; }

        public Menu()
        {
            MenuId = "";
            MenuName = "";
            FoodItemList = new List<FoodItem>();
        }

        public Menu(string menuID, string menuName)
        {
            MenuId = menuID;
            MenuName = menuName;
            FoodItemList = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            FoodItemList.Add(foodItem);
        }

        public bool RemoveFoodItem(FoodItem foodItem)
        {
            if (FoodItemList.Contains(foodItem))
            {
                FoodItemList.Remove(foodItem);
                return true;
            }
            return false;
        }

        public void DisplayFoodItems()
        {
            foreach (FoodItem item in FoodItemList)
            {
                Console.WriteLine($"  - {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice:F2}");
            }
        }

        public override string ToString()
        {
            return $"MenuID: {MenuId} MenuName: {MenuName}";
        }
    }
}
