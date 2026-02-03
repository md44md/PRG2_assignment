using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace PRG2_assignment
{
    class FoodItem
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public string Customise { get; set; }
        public FoodItem() 
        {
            ItemName = "";
            ItemDesc = "";
            ItemPrice = 0.0;
            Customise = "";
        }
        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise)
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = customise;
        }
        public override string ToString()
        {
            return $"Item name: {ItemName} Item desc: {ItemDesc} Item price:{ItemPrice} Customise: {Customise}";
        }
    }
}
