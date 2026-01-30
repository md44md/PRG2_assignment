using PRG2_assignment;

List <Restaurant> restaurantList = new List<Restaurant>();

string[] restaurantLines = File.ReadAllLines("restaurants.csv");

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

string[] fooditemsLines = File.ReadAllLines("fooditem.csv");

for (int i = 1; i < fooditemsLines; i++)
{
    string line = fooditemsLines[i];
    string[] data = line.Split(',');

    string restaurantId = data[0];
    string itemName = data[1];
    string desc = data[2];
    double price = Convert.ToDouble(data[3]);

    FoodItem foodObj = new FoodItem(itemName, desc, price, "");
    foreach (Restaurant res in restaurantList)
    {
        if (res.RestaurantId == restaurantId)
        {
            res.AddMenu(foodObj);
            break;
        }
    }
}