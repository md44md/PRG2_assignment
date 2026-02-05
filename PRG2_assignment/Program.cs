//==========================================================
// Student Number : S________
// Student Name : 
// Partner Name : 
//==========================================================

// student A - KZ: 1✓,4,6,8
// student B - Muhd: 2✓,3✓,5,7

using PRG2_assignment;
using System.Globalization;

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
    Console.WriteLine("Create New Order\n================");
    Console.Write("Enter Customer Email: ");
    string emailAddress = Console.ReadLine();

    Console.Write("Enter Restaurant ID: ");
    string restaurantID = Console.ReadLine();
    Restaurant createOrderRestaurant = null;
    foreach (Restaurant r in restaurantList)
    {
        if (r.RestaurantId == restaurantID)
        {
            createOrderRestaurant = r;
            break;
        }
    }
    if (createOrderRestaurant == null)
    {
        Console.WriteLine("RestaurantID does not exist");
    }
    Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
    string deliveryDate = Console.ReadLine();
    Console.Write("Enter Delivery Time (hh:mm): ");
    string deliveryTime = Console.ReadLine();
    Console.Write("Enter Delivery Address: ): ");
    string deliveryAddress = Console.ReadLine();

    createOrderRestaurant.MenuList[0].DisplayFoodItems();
}

createNewOrder();
































































































// Feature 6



































































































// Feature 7



































































































// Feature 8


