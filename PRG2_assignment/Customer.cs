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

        public Customer() 
        {
            EmailAddress = "";
            CustomerName = "";
            Orders = new List<Order>();
        }
        public Customer(string emailAddress, string customerName)
        {
            EmailAddress = emailAddress;
            CustomerName = customerName;
            Orders = new List<Order>();
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
