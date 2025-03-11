
using System;

int hashFoodCounter = 0; //tracks number of "#####" foods on the console
Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Store all spawned foods in a list
List<Food> foodList = new List<Food>();

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foodTypes = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

LaunchGame();
while (!shouldExit)
{
    Move();
}

Console.ReadLine();

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Checks if at least one non-freeze food exists in foodList
bool HasNonFreezeFood()
{
    foreach (var f in foodList)
    {
        if (f.foodType != "#####")
            return true;
    }
    return false;
}

// Ensures that at least one non-freeze food is present in foodList.
// If none is found, spawns one forced to be non-freeze and adds it to the list.
void EnsureNonFreezeFood()
{
    if (!HasNonFreezeFood())
    {
        Food goodFood = SpawnFood(forceGood: true);
        foodList.Add(goodFood);
    }
}

// Displays random food at a random location
Food SpawnFood(bool forceGood = true)
{

    //// Update food to a random index
    //food = random.Next(0, foods.Length);

    //// Update food position to a random location
    //foodX = random.Next(0, width - player.Length);
    //foodY = random.Next(0, height - 1);

    //// Update food position to a random location
    //foodX = random.Next(0, width - player.Length);
    //foodY = random.Next(0, height - 1);

    //// Display the food at the location
    //Console.SetCursorPosition(foodX, foodY);
    //Console.Write(foodTypes[food]);

    //// If the spawned food is "#####", increase the counter
    //if (foodTypes[food] == "#####")
    //{
    //    hashFoodCounter++;
    //}

    ////keeps spawning food until one non-"#####" exists
    //while (foodTypes[food] == "#####")
    //{
    //    SpawnFood();
    //}

    Food newFood;

    // If forceGood is true, re-roll until we get a food that's not "#####"
    do
    {
        // Update food to a random index
        int foodIndex = random.Next(0, foodTypes.Length);
        newFood.foodType = foodTypes[foodIndex];
    } while (forceGood && newFood.foodType == "#####");

    // Set random position ensuring the food fits in the window
    newFood.X = random.Next(0, width - player.Length);
    newFood.Y = random.Next(0, height - 1);

    // Display the food
    Console.SetCursorPosition(newFood.X, newFood.Y);
    Console.Write(newFood.foodType);

    // If the spawned food is the freeze food, update the counter.
    if (newFood.foodType == "#####")
    {
        hashFoodCounter++;
    }

    return newFood;
}

// Changes the player to match the food consumed
void ChangePlayer(string foodType)
{
    //player = states[food];
    //Console.SetCursorPosition(playerX, playerY);
    //Console.Write(player);
    //if (NeedToFreezePlayer())
    //{
    //    FreezePlayer();
    //}

    // Update the player's state based on food consumed.
    // In this example, if the food was "#####", the player becomes frozen.
    if (foodType == "#####")
    {
        player = states[2]; // (X_X)
    }
    else if (foodType == "@@@@@")
    {
        player = states[0];
    }
    else if (foodType == "$$$$$")
    {
        player = states[1];
    }

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    // If the player's new state is the freeze state, freeze them
    if (NeedToFreezePlayer())
    {
        FreezePlayer();
    }
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
    // Reset to normal state after freeze
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Reads directional input from the Console and moves the player
void Move()
{
    int lastX = playerX;
    int lastY = playerY;
    string exitMessage = "";

    if (!TerminalResized())
    {
        //traditional switch statement
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                playerY -= (player == "(^-^)") ? 2 : 1;
                break;
            case ConsoleKey.DownArrow:
                playerY += (player == "(^-^)") ? 2 : 1;
                break;
            case ConsoleKey.LeftArrow:
                playerX -= (player == "(^-^)") ? 2 : 1;
                break;
            case ConsoleKey.RightArrow:
                playerX += (player == "(^-^)") ? 2 : 1;
                break;
            case ConsoleKey.Escape:
                exitMessage = "Escape button was pressed. Program exiting.";
                shouldExit = true;
                break;
            default:
                exitMessage = "Non-directional character input detected. Program exiting.";
                shouldExit = true;
                break;
        }

        //modern switch expression. Yeah no I used chatGPT for this. Don't feel particularly positive about the approach. The "Assign values back to variables" line is part of the switch expression and is needed to alter the game's state. After a few minutes I think I do like it, it's just a bit more complicated so you need to know how to make it work. But yeah it's aight
        //var result = (Console.ReadKey(true).Key) switch
        //{
        //    ConsoleKey.UpArrow => (playerX, playerY - 1, exitMessage, shouldExit),
        //    ConsoleKey.DownArrow => (playerX, playerY + 1, exitMessage, shouldExit),
        //    ConsoleKey.LeftArrow => (playerX - 1, playerY, exitMessage, shouldExit),
        //    ConsoleKey.RightArrow => (playerX + 1, playerY, exitMessage, shouldExit),
        //    ConsoleKey.Escape => (playerX, playerY, "Escape button was pressed. Program exiting.", true),
        //    _ => (playerX, playerY, "Non-directional character input detected. Program exiting.", true)
        //};

        //// Assign values back to variables
        //(playerX, playerY, exitMessage, shouldExit) = result;

        //check against all foods
        FoodConsumptionCheck();
    }
    else
    {
        exitMessage = "Console was resized. Program exiting.";
        shouldExit = true;
    }

    if (shouldExit)
    {
        ExitGame(exitMessage);
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();

    // We want to allow freeze food to appear, but also guarantee at least one non-freeze food.
    Food food1 = SpawnFood(forceGood: false);
    //Food food2 = SpawnFood(forceGood: false);
    foodList.Add(food1);
    //foodList.Add(food2);
    EnsureNonFreezeFood();

    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

//clears the console and displayes exit message
void ExitGame(string message)
{
    GetConsoleSize();
    Console.Clear();
    Console.SetCursorPosition((width / 2) - (message.Length / 2), height / 2);
    Console.WriteLine($"{message}");
    Thread.Sleep(1000);
    Environment.Exit(0);
}

//clears the console and displayes the game launch message
void LaunchGame()
{
    string message = "Survive until 10 poisonous food items are on the console!";
    GetConsoleSize();
    Console.Clear();
    Console.SetCursorPosition((width / 2) - (message.Length / 2), height / 2);
    Console.WriteLine($"{message}");
    message = "Press Enter to begin!";
    Console.SetCursorPosition((width / 2) - (message.Length / 2), (height / 2) + 2);
    Console.WriteLine($"{message}");
    Console.ReadLine();

    InitializeGame();
}


//gets the console size so that the exit messages are correctly displayed in the center on the screen
void GetConsoleSize()
{
    height = Console.WindowHeight - 1;
    width = Console.WindowWidth - 5;
}

//method to check if the player model overlaps at all with the food model, resulting in food consumption
void FoodConsumptionCheck()
{
    //// Get the rightmost position of the player and food
    //int playerRight = playerX + player.Length - 1;
    //int currentFoodRight = currentFood.X + currentFood.foodType.Length - 1;

    //// Check if the horizontal ranges overlap
    //bool horizontalOverlap = (playerX <= currentFoodRight) && (currentFood.X <= playerRight);
    //bool verticalMatch = (playerY == currentFood.Y);

    ////true if both are true, which means overlap
    //bool overlapExists = horizontalOverlap && verticalMatch;

    //if (overlapExists)
    //{
    //    //ClearFood();
    //    //ChangePlayer();
    //    //SpawnFood();
    //    //PlayerVictoryCheck();
    //    //return true;

    //    // Clear the consumed food from the console
    //    ClearFood(currentFood);

    //    // Update the player's state based on the food's type
    //    ChangePlayer(currentFood.foodType);

    //    // Spawn a new Food object.
    //    // If you want to guarantee that at least one food is not freeze type,
    //    // you could pass true to forceGood. For now, we'll allow any type.
    //    currentFood = SpawnFood();

    //    return true;
    //}
    //else
    //{
    //    return false;
    //}

    // Use a temporary list to store foods to remove to avoid modifying collection during iteration, which would cause a "Collection was modified; enumeration operation may not execute" exception
    List<Food> consumedFoods = new List<Food>();

    foreach (Food f in foodList.ToList())
    {
        int playerRight = playerX + player.Length - 1;
        int foodRight = f.X + f.foodType.Length - 1;
        bool horizontalOverlap = (playerX <= foodRight) && (f.X <= playerRight);
        bool verticalMatch = (playerY == f.Y);

        if (horizontalOverlap && verticalMatch)
        {
            // Process consumption for this food
            ClearFood(f);
            ChangePlayer(f.foodType);
            consumedFoods.Add(f);

            // Spawn a replacement food (allow any type)
            Food replacement = SpawnFood(forceGood: false);
            foodList.Add(replacement);

            // Run victory check after each consumption
            PlayerVictoryCheck();
        }
    }
    // Remove the consumed foods from the main list
    foreach (Food consumed in consumedFoods)
    {
        foodList.Remove(consumed);
    }

    // Always ensure that at least one non-freeze food exists on the screen.
    EnsureNonFreezeFood();
}

//method to check if the player model becomes (X_X) so that they're frozen and their model is reset
bool NeedToFreezePlayer()
{
    if (player == "(X_X)")
    {
        FreezePlayer();
        return true;
    }
    else
    {
        return false;
    }
}

// Clears a given Food object's representation from the console.
void ClearFood(Food f)
{
    //Console.SetCursorPosition(foodX, foodY);
    //for (int i = 0; i < foodTypes[food].Length; i++)
    //{
    //    Console.Write(" ");
    //}
    Console.SetCursorPosition(f.X, f.Y);
    for (int i = 0; i < f.foodType.Length; i++)
    {
        Console.Write(" ");
    }
}

//method to check if after the player eats a food, there are 5 or more "#####" foods and if yes, exit with a victory message
// If too many "#####" foods exist, player wins
void PlayerVictoryCheck()
{
    if (hashFoodCounter > 10)
    {
        string message = "You've survived, you win!";
        //shouldExit isn't needed but might be useful for future changes
        //shouldExit = true;
        ExitGame(message);
    }
}

public struct Food
{
    public string foodType;
    public int X;
    public int Y;
}
