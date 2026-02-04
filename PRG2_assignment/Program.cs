// 

using PRG2_assignment;

// Feature 1
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
}

List<Menu> menuList = new List<Menu>();
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

    Menu menu = null;

    // if menu already exists, set menu to that menu
    foreach (Menu m in menuList)
    {
        if (m.MenuId == restaurantId)
        {
            menu = m;
            break;
        }
    }

    // if menu doesnt exist, create new menu object
    if (menu == null)
    {
        menu = new Menu(restaurantId, "Main Menu");
        menuList.Add(menu);
    }

    menu.AddFoodItem(foodObj);
}

foreach (Menu menu in menuList)
{
    foreach (Restaurant restaurant in restaurantList)
    {
        if (restaurant.RestaurantId == menu.MenuId)
        {
            restaurant.AddMenu(menu);
            break;
        }
    }
}

//foreach (var r in restaurantList)
//{
//    r.DisplayMenu();
//}