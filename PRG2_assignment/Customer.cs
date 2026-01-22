using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_assignment
{
    class Customer
    {
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public List<Order> Orders { get; set; }

        public Customer() { }
        public Customer(string emailAddress, string customerName, List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
                throw new ArgumentException("A customer msut have an order.");

            EmailAddress = emailAddress;
            CustomerName = customerName;
            Orders = orders;    
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public void DisplayAllOrders()
        {
            foreach (Order order in Orders)
            {
                Console.WriteLine(order.ToString());
            }
        }
        public bool RemoveOrder(Order order)
        {
            if (Orders.Count <= 1)
            {
                Console.WriteLine("Cannot remove last order. A customer must have at least one order.");
                return false;
            }
            else
            {
                if (Orders.Contains(order))
                {
                    Orders.Remove(order);
                    return true;
                }
                else
                {
                    return false;
                }   
            }
        }
        public override string ToString()
        {
            return $"Email address: {EmailAddress} Customer name: {CustomerName}";
        }
    }
}
