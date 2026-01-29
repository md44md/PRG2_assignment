using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class Menu
    {
        public string MenuID { get; set; }
        public string MenuName { get; set; }

        public List<FoodItem> FoodItemList { get; set; }

        public Menu()
        {
            MenuID = "";
            MenuName = "";
            FoodItemList = new List<FoodItem>();
        }

        public Menu(string menuID, string menuName, List<FoodItem> initialFoods)
        {
            MenuID = menuID;
            MenuName = menuName;
            
            FoodItemList = initialFoods ;
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
            Console.WriteLine($"--- {MenuName} ---");
            foreach (FoodItem item in FoodItemList)
            {
                Console.WriteLine(item);
            }
        }

        public override string ToString()
        {
            return $"Menu ID is: {MenuID}, Menu name is: {MenuName}";
        }
    }
}
