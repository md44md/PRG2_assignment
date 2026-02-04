// 

using PRG2_assignment;

// Feature 1
List<Restaurant> restaurantList = new List<Restaurant>();
string[] restaurantLines = File.ReadAllLines("data/restaurants.csv");

// Create restaurants and main menus, add restaurants to restaurantList
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


//// print all menus (testing)
//foreach (var r in restaurantList)
//{
//    r.DisplayMenu();
//}