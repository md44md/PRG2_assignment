//==========================================================
// Student Number : S________
// Student Name : 
// Partner Name : 
//==========================================================

// student A - KZ: 1✓,4,6,8, B
// student B - Muhd: 2✓,3✓,5✓,7✓, A

using PRG2_assignment;
using System.Globalization;

// Helper functions
Customer lookupCustomer(List<Customer> customerList, string emailAddress)
{
    Customer orderCustomer = null;
    foreach (Customer c in customerList)
    {
        if (c.EmailAddress == emailAddress)
        {
            orderCustomer = c;
            break;
        }
    }
    return orderCustomer;
}

Restaurant lookupRestaurant(List<Restaurant> restaurantList, string restaurantId)
{
    Restaurant orderRestaurant = null;
    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantId)
        {
            orderRestaurant = r;
            break;
        }
    }
    return orderRestaurant;
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 1

// Create restaurants and main menus, add restaurants to restaurantList
List<Restaurant> restaurantList = new List<Restaurant>();
string[] restaurantLines = File.ReadAllLines("data/restaurants.csv");

for (int i = 1; i < restaurantLines.Length; i++)
{
    string line = restaurantLines[i];
    string[] data = line.Split(',');

    string restaurantId = data[0];
    string restaurantName = data[1];
    string resturantEmail = data[2];

    Restaurant resObj = new Restaurant(restaurantId, restaurantName, resturantEmail);
    restaurantList.Add(resObj);

    Menu menu = new Menu("1", "Main Menu");
    resObj.AddMenu(menu);
}

// Create foodItems, add foodItems to foodItemList add foodItems to restaurant main menu
List<FoodItem> foodItemList = new List<FoodItem>();
string[] fooditemsLines = File.ReadAllLines("data/fooditems.csv");

for (int i = 1; i < fooditemsLines.Length; i++)
{
    string line = fooditemsLines[i];
    string[] data = line.Split(',');

    string restaurantId = data[0];
    string itemName = data[1];
    string desc = data[2];
    double price = Convert.ToDouble(data[3]);

    FoodItem foodObj = new FoodItem(itemName, desc, price, "");
    foodItemList.Add(foodObj);

    Restaurant restaurant = lookupRestaurant(restaurantList, restaurantId);

    if (restaurant != null)
    {
        restaurant.MenuList[0].AddFoodItem(foodObj);
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 2
// Create customers, add customers to customerList
List<Customer> customerList = new List<Customer>();
string[] customerLines = File.ReadAllLines("data/customers.csv");

for (int i = 1; i < customerLines.Length; i++)
{
    string line = customerLines[i];
    string[] data = line.Split(",");

    string name = data[0];
    string email = data[1];

    Customer custObj = new Customer(email, name);
    customerList.Add(custObj);
}

// Create orders, add orders to orderlist, add orders to customers and restaurant
List<Order> orderList = new List<Order>();
string[] orderLines = File.ReadAllLines("data/orders.csv");

for (int i = 1; i < orderLines.Length; i++)
{
    string line = orderLines[i];

    // Split CSV line while ignoring commas inside quotes
    List<string> fields = new List<string>();
    bool insideQuotes = false;
    string currentField = "";

    foreach (char c in line)
    {
        if (c == '"')
        {
            insideQuotes = !insideQuotes;
            continue;
        }

        if (c == ',' && !insideQuotes)
        {
            fields.Add(currentField);
            currentField = "";
        }
        else
        {
            currentField += c;
        }
    }
    fields.Add(currentField); // add last ordered food item field

    // Parse the first 9 fields
    int orderID = Convert.ToInt32(fields[0]);
    string customerEmail = fields[1];
    string restaurantID = fields[2];
    DateTime deliveryDateTime = DateTime.ParseExact(fields[3] + " " + fields[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
    string deliveryAddress = fields[5];
    DateTime createdDateTime = DateTime.ParseExact(fields[6], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
    double totalAmount = Convert.ToDouble(fields[7]);
    string status = fields[8];

    // Match customer and restaurant with orders
    Customer orderCustomer = lookupCustomer(customerList, customerEmail);

    Restaurant orderRestaurant = lookupRestaurant(restaurantList, restaurantID);

    // Create order object
    Order orderObj = new Order(orderID, createdDateTime, totalAmount, status, deliveryDateTime, deliveryAddress, "Credit Card", false, orderCustomer, orderRestaurant, null);
    orderList.Add(orderObj);

    // Create orderedItems
    string itemsStr = fields[9]; // full Items string
    string[] orderedItems = itemsStr.Split('|');

    List<OrderedFoodItem> orderItemList = new List<OrderedFoodItem>();

    foreach (string item in orderedItems)
    {
        string[] parts = item.Split(',');
        if (parts.Length < 2) continue;

        string itemName = parts[0].Trim();
        if (!int.TryParse(parts[1].Trim(), out int qty)) continue;

        FoodItem foodItem = null;
        foreach (Restaurant r in restaurantList)
        {
            foreach (Menu m in r.MenuList)
            {
                foreach (FoodItem f in m.FoodItemList)
                {
                    if (f.ItemName == itemName)
                    {
                        foodItem = f;
                        break;
                    }
                }
                if (foodItem != null) break;
            }
            if (foodItem != null) break;
        }

        double subTotal = foodItem.ItemPrice * qty;

        OrderedFoodItem orderedFoodItemObj = new OrderedFoodItem(foodItem.ItemName, foodItem.ItemDesc, foodItem.ItemPrice, "", qty, subTotal);

        orderObj.AddOrderedFoodItem(orderedFoodItemObj);
    }

    // Add order to customer and restaurant
    orderCustomer.AddOrder(orderObj);
    orderRestaurant.OrderList.Enqueue(orderObj);
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 3
void listRestaurantsMenuItems()
{
    Console.WriteLine("All Restaurants and Menu Items\n==============================");
    foreach (Restaurant r in restaurantList)
    {
        Console.WriteLine($"Restuarant: {r.RestaurantName} ({r.RestaurantId})");
        r.DisplayMenu();
        Console.WriteLine();
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 4
void listAllOrders()
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-10}{"Customer",-20}{"Restaurant",-20}{"Delivery Date/Time",-20}{"Amount",-10}{"Status",-15}");
    Console.WriteLine($"{"--------",-10}{"----------",-20}{"-------------",-20}{"------------------",-20}{"------",-10}{"---------",-15}");

    // Collect all orders from all customers
    List<Order> allOrders = new List<Order>();
    foreach (Customer customer in customerList)
    {
        foreach (Order order in customer.Orders)
        {
            allOrders.Add(order);
        }
    }

    // Sort by OrderId
    allOrders.Sort((a, b) => a.OrderId.CompareTo(b.OrderId));

    // Display all orders
    foreach (Order order in allOrders)
    {
        Console.WriteLine($"{order.OrderId,-10}{order.Customer.CustomerName,-20}{order.Restaurant.RestaurantName,-20}{order.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"),-20}${order.OrderTotal,-9:F2}{order.OrderStatus,-15}");
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 5

void createNewOrder()
{
    //Order info input
    Console.WriteLine("Create New Order\n================");

    // Customer email input
    Console.Write("Enter Customer Email: ");
    string emailAddress = Console.ReadLine();
    Customer orderCustomer = lookupCustomer(customerList, emailAddress);
    if (orderCustomer == null)
    {
        Console.WriteLine("Customer email does not exist");
        return;
    }

    // Restuarant ID input
    Console.Write("Enter Restaurant ID: ");
    string restaurantId = Console.ReadLine().ToUpper();
    Restaurant orderRestaurant = lookupRestaurant(restaurantList, restaurantId);
    if (orderRestaurant == null)
    {
        Console.WriteLine("RestaurantID does not exist");
        return;
    }

    // Delivery datetime input
    DateTime deliveryDateTime;
    try
    {
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string deliveryDate = Console.ReadLine();

        Console.Write("Enter Delivery Time (hh:mm): ");
        string deliveryTime = Console.ReadLine();

        deliveryDateTime = DateTime.ParseExact(
            deliveryDate + " " + deliveryTime,
            "dd/MM/yyyy HH:mm",
            CultureInfo.InvariantCulture
        );

        if (deliveryDateTime <= DateTime.Now)
        {
            Console.WriteLine("Delivery time must be in the future.");
            return;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid date or time format. Please try again.");
        return;
    }

    // Delivery address input
    Console.Write("Enter Delivery Address: ");
    string deliveryAddress = Console.ReadLine();

    // Display restaurant menu
    List<FoodItem> menuItems = orderRestaurant.MenuList[0].FoodItemList;

    Console.WriteLine("\nAvailable Food Items:");
    for (int i = 0; i < menuItems.Count(); i++)
    {
        Console.WriteLine($"{i + 1}. {menuItems[i].ItemName} - ${menuItems[i].ItemPrice:F2}");
    }

    Dictionary<FoodItem, int> selectedItems = new Dictionary<FoodItem, int>();

    // Item input
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");

        try
        {
            int itemChoice = Convert.ToInt32(Console.ReadLine());

            if (itemChoice == 0)
                break;

            if (itemChoice < 1 || itemChoice > menuItems.Count)
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            Console.Write("Enter quantity: ");
            int itemQty = Convert.ToInt32(Console.ReadLine());

            FoodItem selectedFood = menuItems[itemChoice - 1];

            if (selectedItems.ContainsKey(selectedFood))
            {
                selectedItems[selectedFood] += itemQty;
            }

            else
            {
                selectedItems.Add(selectedFood, itemQty);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    // Special request input
    string specialRequest = null;
    Console.Write("Add special request? [Y/N]: ");
    string srChoice = Console.ReadLine().ToUpper();

    if (srChoice == "Y")
    {
        Console.Write("Enter special request: ");
        specialRequest = Console.ReadLine();
    }

    // Display total
    double orderTotal = 0;
    foreach (var item in selectedItems)
    {
        orderTotal += item.Key.ItemPrice * item.Value;
    }

    double deliveryFee = 5.00;
    Console.WriteLine($"\nOrder Total: ${orderTotal:F2} + ${deliveryFee:F2} (delivery) = ${(orderTotal + deliveryFee):F2}");

    // Display payment
    Console.Write("Proceed to payment? [Y/N]: ");
    string paymentChoice = Console.ReadLine().ToUpper();

    if (paymentChoice == "N")
    {
        return;
    }

    Console.WriteLine("\nPayment method:");
    Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
    string paymentMethod = Console.ReadLine().ToUpper();

    if (paymentMethod != "CC" && paymentMethod != "PP" && paymentMethod != "CD")
    {
        Console.WriteLine("Invalid payment method.");
        return;
    }


    // Create orderId
    string[] lines = File.ReadAllLines("data/orders.csv");

    int maxId = 0;
    for (int i = 1; i < lines.Length; i++)
    {
        string[] parts = lines[i].Split(',');
        int id = Convert.ToInt32(parts[0]);
        if (id > maxId)
            maxId = id;
    }

    int orderId = maxId + 1;

    // Create order
    Order newOrder = new Order(orderId, DateTime.Now, orderTotal + deliveryFee, "Pending",
    deliveryDateTime, deliveryAddress, paymentMethod, false, orderCustomer, orderRestaurant, null);

    // Add items to order
    foreach (var item in selectedItems)
    {
        FoodItem food = item.Key;
        int qty = item.Value;

        double subTotal = food.ItemPrice * qty;

        OrderedFoodItem orderedItem = new OrderedFoodItem(
            food.ItemName,
            food.ItemDesc,
            food.ItemPrice,
            specialRequest ?? "",
            qty,
            subTotal
        );

        newOrder.AddOrderedFoodItem(orderedItem);
    }

    // Add to lists
    orderCustomer.Orders.Add(newOrder);
    orderRestaurant.OrderList.Enqueue(newOrder);

    // Append to CSV
    string itemsStr = "";

    for (int i = 0; i < newOrder.OrderedFoodItems.Count; i++)
    {
        OrderedFoodItem item = newOrder.OrderedFoodItems[i];

        itemsStr += $"{item.ItemName}, {item.QtyOrdered}";

        if (i < newOrder.OrderedFoodItems.Count - 1)
            itemsStr += "|";
    }
    itemsStr = $"\"{itemsStr}\"";

    string wPath = "data/orders.csv";

    using (StreamWriter writer = new StreamWriter(wPath, append: true))
        writer.WriteLine(
            $"{newOrder.OrderId}," +
            $"{newOrder.Customer.EmailAddress}," +
            $"{newOrder.Restaurant.RestaurantId}," +
            $"{newOrder.DeliveryDateTime:dd/MM/yyyy}," +
            $"{newOrder.DeliveryDateTime:HH:mm}," +
            $"{newOrder.DeliveryAddress}," +
            $"{newOrder.OrderDateTime:dd/MM/yyyy HH:mm}," +
            $"{newOrder.OrderTotal}," +
            $"{newOrder.OrderStatus}," +
            $"{itemsStr}"
        );

    Console.WriteLine($"Order {newOrder.OrderId} created successfully! Status: Pending");
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 6

// Stack for refunds
Stack<Order> refundStack = new Stack<Order>();

void processOrder()
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");
    Console.Write("Enter Restaurant ID: ");
    string restaurantId = Console.ReadLine().ToUpper();

    // Find restaurant
    Restaurant restaurant = null;
    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantId)
        {
            restaurant = r;
            break;
        }
    }

    if (restaurant == null)
    {
        Console.WriteLine("Restaurant ID not found.");
        return;
    }

    if (restaurant.OrderList.Count == 0)
    {
        Console.WriteLine("No orders in queue for this restaurant.");
        return;
    }

    // Create a temporary queue to process orders
    Queue<Order> tempQueue = new Queue<Order>();
    bool allProcessed = false;

    while (restaurant.OrderList.Count > 0 && !allProcessed)
    {
        Order order = restaurant.OrderList.Dequeue();

        // Display order details
        Console.WriteLine($"\nOrder {order.OrderId}:");
        Console.WriteLine($"Customer: {order.Customer.CustomerName}");
        Console.WriteLine("Ordered Items:");
        foreach (var item in order.OrderedFoodItems)
        {
            Console.WriteLine($"{item.ItemName} - {item.QtyOrdered}");
        }
        Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
        Console.WriteLine($"Order Status: {order.OrderStatus}");

        Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string choice = Console.ReadLine().ToUpper();

        switch (choice)
        {
            case "C":
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Preparing";
                    Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                }
                else
                {
                    Console.WriteLine($"Cannot confirm. Order status is {order.OrderStatus}.");
                }
                tempQueue.Enqueue(order);
                break;

            case "R":
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Rejected";
                    refundStack.Push(order);
                    Console.WriteLine($"Order {order.OrderId} rejected. Refund of ${order.OrderTotal:F2} processed.");
                }
                else
                {
                    Console.WriteLine($"Cannot reject. Order status is {order.OrderStatus}.");
                }
                tempQueue.Enqueue(order);
                break;

            case "S":
                if (order.OrderStatus == "Cancelled")
                {
                    Console.WriteLine($"Skipping cancelled order {order.OrderId}.");
                }
                else
                {
                    Console.WriteLine($"Order {order.OrderId} skipped.");
                }
                tempQueue.Enqueue(order);
                break;

            case "D":
                if (order.OrderStatus == "Preparing")
                {
                    order.OrderStatus = "Delivered";
                    Console.WriteLine($"Order {order.OrderId} delivered. Status: Delivered");
                }
                else
                {
                    Console.WriteLine($"Cannot deliver. Order status is {order.OrderStatus}.");
                }
                tempQueue.Enqueue(order);
                break;

            default:
                Console.WriteLine("Invalid choice. Order returned to queue.");
                tempQueue.Enqueue(order);
                break;
        }

        Console.Write("\nContinue processing? [Y/N]: ");
        string continueChoice = Console.ReadLine().ToUpper();
        if (continueChoice == "N")
        {
            allProcessed = true;
        }
    }

    // Return remaining orders to the queue
    while (restaurant.OrderList.Count > 0)
    {
        tempQueue.Enqueue(restaurant.OrderList.Dequeue());
    }

    // Restore all orders back to the restaurant queue
    while (tempQueue.Count > 0)
    {
        restaurant.OrderList.Enqueue(tempQueue.Dequeue());
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 7

void modifyOrder()
{
    Console.WriteLine("Modify Order\r\n============");

    // Input customer email and search for email
    Console.Write("Enter Customer Email: ");
    string emailAddress = Console.ReadLine();

    Customer orderCustomer = lookupCustomer(customerList, emailAddress);
    if (orderCustomer == null)
    {
        Console.WriteLine("Customer email does not exist");
        return;
    }

    // Add pending orders to a list and display list
    List<Order> pendingOrders = new List<Order>();
    foreach (Order o in orderCustomer.Orders)
    {
        if (o.OrderStatus == "Pending")
        {
            pendingOrders.Add(o);
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders.");
        return;
    }

    Console.WriteLine("\nPending Orders:");
    foreach (Order o in pendingOrders)
    {
        Console.WriteLine(o.OrderId);
    }

    // Prompt for order ID
    Console.Write("Enter Order ID: ");
    int orderId = Convert.ToInt32(Console.ReadLine());

    Order orderToChange = null;
    foreach (Order o in pendingOrders)
    {
        if (o.OrderId == orderId)
        {
            orderToChange = o;
            break;
        }
    }
    if (orderToChange == null)
    {
        Console.WriteLine("Invalid Order ID.");
        return;
    }

    // Display order item info
    Console.WriteLine("Order Items:");
    for (int i = 0; i < orderToChange.OrderedFoodItems.Count; i++)
    {
        var item = orderToChange.OrderedFoodItems[i];
        Console.WriteLine($"{i + 1}. {item.ItemName} - {item.QtyOrdered}");
    }

    Console.WriteLine("Address:");
    Console.WriteLine(orderToChange.DeliveryAddress);

    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(orderToChange.DeliveryDateTime.ToString("d/M/yyyy, HH:mm"));

    // Prompt modification
    Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
    int choice = Convert.ToInt32(Console.ReadLine());

    if (choice == 1) // Items
    {
        double oldTotal = orderToChange.OrderTotal;

        // Change quantity
        Console.Write("Select item number to modify: ");
        int index = Convert.ToInt32(Console.ReadLine()) - 1;

        if (index < 0 || index >= orderToChange.OrderedFoodItems.Count)
        {
            Console.WriteLine("Item number out of range.");
            return;
        }

        Console.Write("Enter new quantity (0 to remove): ");
        int newQty = Convert.ToInt32(Console.ReadLine());

        if (newQty == 0)
        {
            orderToChange.OrderedFoodItems.RemoveAt(index);
        }
        else
        {
            if (newQty < 0)
            {
                Console.WriteLine("Invalid quantity");
            }
            else
            {
                orderToChange.OrderedFoodItems[index].QtyOrdered = newQty;
                orderToChange.OrderedFoodItems[index].SubTotal = newQty * orderToChange.OrderedFoodItems[index].ItemPrice;
            }
        }

        // Recalculate total
        double newTotal = 0;
        foreach (var i in orderToChange.OrderedFoodItems)
        {
            newTotal += i.SubTotal;
        }
        double deliveryFee = 5.00;
        orderToChange.OrderTotal = newTotal + deliveryFee;

        Console.WriteLine($"\nOrder {orderToChange.OrderId} updated. New Total: ${orderToChange.OrderTotal:F2}");
    }

    else if (choice == 2) // Address
    {
        Console.Write("Enter new address: ");
        string newAddress = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newAddress))
        {
            Console.WriteLine("Address cannot be empty.");
            return;
        }

        orderToChange.DeliveryAddress = newAddress.Trim();

        Console.WriteLine($"\nOrder {orderToChange.OrderId} updated. New Address: {orderToChange.DeliveryAddress}");
    }

    else if (choice == 3) // Delivery Time
    {
        Console.Write("Enter new Delivery Time (hh:mm): ");
        string newTime = Console.ReadLine();

        try
        {
            DateTime newDateTime = DateTime.ParseExact(orderToChange.DeliveryDateTime.ToString("dd/MM/yyyy") + " " + newTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (newDateTime <= DateTime.Now)
            {
                Console.WriteLine("Delivery time must be in the future.");
                return;
            }

            orderToChange.DeliveryDateTime = newDateTime;

            Console.WriteLine($"\nOrder {orderToChange.OrderId} updated. New Delivery Time: {newTime}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid time format. Please use hh:mm.");
            return;
        }
    }

    else
    {
        Console.WriteLine("Invalid choice");
        return;
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 8
void deleteOrder()
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");
    Console.Write("Enter Customer Email: ");
    string email = Console.ReadLine();

    // Find customer
    Customer customer = null;
    foreach (Customer c in customerList)
    {
        if (c.EmailAddress == email)
        {
            customer = c;
            break;
        }
    }

    if (customer == null)
    {
        Console.WriteLine("Customer email not found.");
        return;
    }

    // Display pending orders
    List<Order> pendingOrders = new List<Order>();
    foreach (Order order in customer.Orders)
    {
        if (order.OrderStatus == "Pending")
        {
            pendingOrders.Add(order);
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found for this customer.");
        return;
    }

    Console.WriteLine("Pending Orders:");
    foreach (Order order in pendingOrders)
    {
        Console.WriteLine(order.OrderId);
    }

    Console.Write("Enter Order ID: ");
    if (!int.TryParse(Console.ReadLine(), out int orderId))
    {
        Console.WriteLine("Invalid Order ID.");
        return;
    }

    // Find the order
    Order orderToDelete = null;
    foreach (Order order in pendingOrders)
    {
        if (order.OrderId == orderId)
        {
            orderToDelete = order;
            break;
        }
    }

    if (orderToDelete == null)
    {
        Console.WriteLine("Order ID not found in pending orders.");
        return;
    }

    // Display order details
    Console.WriteLine($"Customer: {orderToDelete.Customer.CustomerName}");
    Console.WriteLine("Ordered Items:");
    foreach (var item in orderToDelete.OrderedFoodItems)
    {
        Console.WriteLine($"{item.ItemName} - {item.QtyOrdered}");
    }
    Console.WriteLine($"Delivery date/time: {orderToDelete.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${orderToDelete.OrderTotal:F2}");
    Console.WriteLine($"Order Status: {orderToDelete.OrderStatus}");

    Console.Write("Confirm deletion? [Y/N]: ");
    string confirm = Console.ReadLine().ToUpper();

    if (confirm == "Y")
    {
        // Update order status to Cancelled
        orderToDelete.OrderStatus = "Cancelled";

        // Add to refund stack
        refundStack.Push(orderToDelete);

        Console.WriteLine($"Order {orderToDelete.OrderId} cancelled. Refund of ${orderToDelete.OrderTotal:F2} processed.");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }
}


// -------------------------------------------------------------------------------------------------------------------------------

// Advanced feature A

void ProcessPendingOrders(List<Order> orderList)
{
    // Set current datetime
    DateTime processDateTime;
    while (true)
    {
        try
        {
            Console.Write("Enter current date (dd/MM/yyyy): ");
            string deliveryDate = Console.ReadLine();

            Console.Write("Enter current time (hh:mm): ");
            string deliveryTime = Console.ReadLine();

            processDateTime = DateTime.ParseExact(
                deliveryDate + " " + deliveryTime,
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture
            );
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid date or time format. Please try again.\n");
        }
    }

    // Count number of pending orders
    List<Order> pendingOrdersToday = new List<Order>();

    for (int i = 0; i < orderList.Count; i++)
    {
        Order o = orderList[i];

        if (o.OrderStatus == "Pending" && o.DeliveryDateTime.Date == processDateTime.Date)
        {
            pendingOrdersToday.Add(o);
        }
    }

    int pendingCount = pendingOrdersToday.Count;
    Console.WriteLine($"Total Pending Orders Today: {pendingCount}");

    int preparingCount = 0;
    int rejectedCount = 0;

    foreach (Order order in pendingOrdersToday)
    {
        TimeSpan timeUntilDelivery = order.DeliveryDateTime - processDateTime;

        if (timeUntilDelivery.TotalHours < 1)
        {
            order.OrderStatus = "Rejected";
            rejectedCount++;
        }
        else
        {
            order.OrderStatus = "Preparing";
            preparingCount++;
        }
    }

    int processed = preparingCount + rejectedCount;

    double percentage = 0;
    if (pendingOrdersToday.Count > 0)
    {
        percentage = (double)processed / pendingOrdersToday.Count * 100;
    }

    Console.WriteLine("\nProcessing Summary");
    Console.WriteLine("==================");
    Console.WriteLine($"Orders processed: {processed}");
    Console.WriteLine($"Preparing orders: {preparingCount}");
    Console.WriteLine($"Rejected orders: {rejectedCount}");
    Console.WriteLine($"Automatically processed percentage: {percentage:F2}%");
}


// -------------------------------------------------------------------------------------------------------------------------------

// Program

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurantList.Count()} restaurants loaded!");
Console.WriteLine($"{foodItemList.Count()} food items loaded!");
Console.WriteLine($"{customerList.Count()} customers loaded!");
Console.WriteLine($"{orderList.Count()} orders loaded!");

while (true)
{
    Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Bulk process unprocessed orders for a current day");
    Console.WriteLine("0. Exit");

    Console.Write("Enter your choice: ");

    try
    {
        int choice = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(); //break
        if (choice == 1)
        {
            listRestaurantsMenuItems();
        }
        else if (choice == 2)
        {
            listAllOrders();
        }
        else if (choice == 3)
        {
            createNewOrder();
        }
        else if (choice == 4)
        {
            processOrder();
        }
        else if (choice == 5)
        {
            modifyOrder();
        }
        else if (choice == 6)
        {
            deleteOrder();
        }
        else if (choice == 7)
        {
            ProcessPendingOrders(orderList);
        }
        else if (choice == 0)
        {
            SaveDataOnExit();
            Console.WriteLine("Exitting...");
            return; 
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid choice.");
    }
}