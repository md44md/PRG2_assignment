using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class OrderedFoodItem : FoodItem
    {
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }
        public OrderedFoodItem() { }
        public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, string customise, int qtyOrdered, double subTotal)
            : base(itemName, itemDesc, itemPrice, customise)
        {
            QtyOrdered = qtyOrdered;
            SubTotal = subTotal;
        }
        public double CalculateSubtotal()
        {
            return ItemPrice * QtyOrdered;
        }
    }
}
