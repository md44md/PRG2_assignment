using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }

        public List<OrderedFoodItem> OrderedFoodItems { get; set; }
        public Customer Customer { get; set; }

        public Order() { }
        public Order(int orderId, DateTime orderDateTime, double orderTotal, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid, List<OrderedFoodItem> orderedFoodItems, Customer customer)
        {
            if (orderedFoodItems == null || orderedFoodItems.Count == 0)
                throw new ArgumentException("An order must have at least one OrderedFoodItem.");

            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = orderPaymentMethod;
            OrderPaid = orderPaid;

            Customer = customer ?? throw new ArgumentException("Order must have a Customer.");
            OrderedFoodItems = orderedFoodItems;
        }

        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (var item in OrderedFoodItems)
            {
                item.SubTotal = item.CalculateSubtotal();
                total += item.SubTotal;
            }
            OrderTotal = total;
            return total;
        }
        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedFoodItems.Add(item);
        }
        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            if (OrderedFoodItems.Count <= 1)
            {
                Console.WriteLine("Cannot remove last item. An order must have at least one item.");
                return false;
            }
            else
            {
                if (OrderedFoodItems.Contains(item))
                {
                    OrderedFoodItems.Remove(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine("Ordered Food Items:");
            Console.WriteLine($"{"Item Name",-12}{"Qty",-5}{"Subtotal",-10}");
            foreach (var item in OrderedFoodItems)
            {
                Console.WriteLine($"- {item.ItemName,-12}{item.QtyOrdered,-5}{item.SubTotal,-10:F2}");
            }
        }
        public override string ToString()
        {
            return $"Order ID: {OrderId} DateTime: {OrderDateTime} Total: ${OrderTotal:F2} Status: {OrderStatus} Delivery DateTime: {DeliveryDateTime} Address: {DeliveryAddress} Payment Method: {OrderPaymentMethod} Paid: {OrderPaid}";
        }
    }
}
