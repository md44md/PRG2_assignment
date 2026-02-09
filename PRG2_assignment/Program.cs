//==========================================================
// Student Number : S________
// Student Name : 
// Partner Name : 
//==========================================================

// student A - KZ: 1✓,4,6,8
// student B - Muhd: 2✓,3✓,5,7

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

// Create foodItems, add foodItems to restaurant main menu
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

// Create orders
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

//createNewOrder();


// -------------------------------------------------------------------------------------------------------------------------------

// Feature 6























































































// Feature 7



































































































// Feature 8


