//==========================================================
// Student Number : S________
// Student Name : 
// Partner Name : 
//==========================================================

// student A - KZ: 1✓,4,6,8
// student B - Muhd: 2✓,3✓,5✓,7✓

using PRG2_assignment;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

// Create foodItems, add foddItems to foodItemList add foodItems to restaurant main menu
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

    Restaurant restaurant = null;

    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantId)
        {
            restaurant = r;
            break;
        }
    }

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

for (int i = 1;i < customerLines.Length; i++)
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
    Customer orderCustomer = null;
    foreach (Customer c in customerList)
    {
        if (c.EmailAddress == customerEmail)
        {
            orderCustomer = c;
            break;
        }
    }
    Restaurant orderRestaurant = null;
    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantID)
        {
            orderRestaurant = r;
            break;
        }
    }

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

        OrderedFoodItem orderedFoodItemObj = new OrderedFoodItem(foodItem.ItemName,foodItem.ItemDesc,foodItem.ItemPrice,"",qty,subTotal);

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




















































































// -------------------------------------------------------------------------------------------------------------------------------

// Feature 5

void createNewOrder()
{
    //Order info input
    Console.WriteLine("Create New Order\n================");

    Console.Write("Enter Customer Email: ");
    string emailAddress = Console.ReadLine();
    Customer orderCustomer = null;
    foreach (Customer c in customerList)
    {
        if (c.EmailAddress == emailAddress)
        {
            orderCustomer = c;
            break;
        }
    }

    if (orderCustomer == null)
    {
        Console.WriteLine("Customer email does not exist");
        return;
    }

    Console.Write("Enter Restaurant ID: ");
    string restaurantId = Console.ReadLine().ToUpper();
    Restaurant orderRestaurant = null;
    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantId)
        {
            orderRestaurant = r;
            break;
        }
    }
    if (orderRestaurant == null)
    {
        Console.WriteLine("RestaurantID does not exist");
        return;
    }

    Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
    string deliveryDate = Console.ReadLine();
    Console.Write("Enter Delivery Time (hh:mm): ");
    string deliveryTime = Console.ReadLine();

    DateTime deliveryDateTime = DateTime.ParseExact(
    deliveryDate + " " + deliveryTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

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
            selectedItems[selectedFood] += itemQty;
        else
            selectedItems.Add(selectedFood, itemQty);
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





















































































// -------------------------------------------------------------------------------------------------------------------------------

// Feature 7

void modifyOrder()
{
    Console.WriteLine("Modify Order\r\n============");

    // Input customer email and search for email
    Console.Write("Enter Customer Email: ");
    string emailAddress = Console.ReadLine();

    Customer orderCustomer = new Customer();
    foreach (Customer c in customerList)
    {
        if (c.EmailAddress == emailAddress)
        {
            orderCustomer = c;
            break;
        }
    }
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

        Console.Write("Enter new quantity (0 to remove): ");
        int newQty = Convert.ToInt32(Console.ReadLine());

        if (newQty == 0)
        {
            orderToChange.OrderedFoodItems.RemoveAt(index);
        }
        else
        {
            orderToChange.OrderedFoodItems[index].QtyOrdered = newQty;
            orderToChange.OrderedFoodItems[index].SubTotal = newQty * orderToChange.OrderedFoodItems[index].ItemPrice;
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
        orderToChange.DeliveryAddress = Console.ReadLine();

        Console.WriteLine($"\nOrder {orderToChange.OrderId} updated. New Address: {orderToChange.DeliveryAddress}");
    }

    else if (choice == 3) // Delivery Time
    {
        Console.Write("Enter new Delivery Time (hh:mm): ");
        string newTime = Console.ReadLine();

        DateTime newDateTime = DateTime.ParseExact(orderToChange.DeliveryDateTime.ToString("dd/MM/yyyy") + " " + newTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

        orderToChange.DeliveryDateTime = newDateTime;

        Console.WriteLine($"\nOrder {orderToChange.OrderId} updated. New Delivery Time: {newTime}");
    }

    else
    {
        Console.WriteLine("Invalid choice");
        return;
    }

    //Console.WriteLine("\nUpdated Order Details");
    //Console.WriteLine("=====================");
    //Console.WriteLine($"Order ID: {orderToChange.OrderId}");
    //Console.WriteLine($"Total: ${orderToChange.OrderTotal:F2}");
    //Console.WriteLine($"Delivery: {orderToChange.DeliveryDateTime:d/M/yyyy HH:mm}");
    //Console.WriteLine($"Address: {orderToChange.DeliveryAddress}");
}































































































// Feature 8
























































// Program

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurantList.Count()} restaurants loaded!");
Console.WriteLine($"{foodItemList.Count()} food items loaded!");
Console.WriteLine($"{customerList.Count()} restaurants loaded!");
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
    Console.WriteLine("0. Exit");

    Console.Write("Enter your choice: ");
    int choice = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine(); //break
    if (choice == 1)
    {
        listRestaurantsMenuItems();
    }
    else if (choice == 2)
    {

    }
    else if (choice == 3)
    {
        createNewOrder();
    }
    else if (choice == 4)
    {

    }
    else if (choice == 5)
    {
        modifyOrder();
    }
    else if (choice == 6)
    {

    }
    else if (choice == 0)
    {
        Console.WriteLine("Exitting...");
        return;
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}