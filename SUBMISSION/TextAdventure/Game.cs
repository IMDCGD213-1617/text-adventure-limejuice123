using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    //The main game class. Most of the main game functions happen within this class.
    class Game
	{
        //Variables that handle all things to do with locations, and where the player is.
        Location currentLocation;
        public Location lDoors, lPanel, lBack, lFloor, lCeiling, lRight, lShaft;
        public static string locationCurrent;
        public static string currentFloor;

        //Check if the game is running.
        public bool isRunning = true;

        //Lists containing the player inventories and usables in the map.
        private List<Item> inventory;
        private List<Usables> usables;

        //Variables that determine some story elements; gameplay changes when they change.
        private bool haveCheckedFloor = false;
        private bool haveUsedKey = false;
        private bool haveUsedCrowbar = false;

        //Most of the instances of classes in the game are declared in this instance of the game class.
        public Game()
		{
            //Clear the inventories. Prevents leftovers from previous games.
            inventory = new List<Item>();
            usables = new List<Usables>();

            //Quick scene setter. Dark red is used to represent story elements.
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The lift has stopped. You are trapped.");
            Console.ResetColor();

			// build the "map"
			lDoors = new Location("Facing Forward.", "You are facing the doors to the lift.");

			lPanel = new Location("Facing Left.", "You are facing the lift panel.");
            //Adds instances of Usables for players to interact with later.
            Usables alarm = new Usables("alarm");
            Usables floor1 = new Usables("floor1");
            Usables floor2 = new Usables("floor2");
            Usables floor3 = new Usables("floor3");
            Usables openDoors = new Usables("opendoors");
            Usables closeDoors = new Usables("closedoors");
            lPanel.addUsables(alarm);
            lPanel.addUsables(floor1);
            lPanel.addUsables(floor2);
            lPanel.addUsables(floor3);
            lPanel.addUsables(openDoors);
            lPanel.addUsables(closeDoors);

			lFloor = new Location("Facing Down.", "You are facing the floor. The unconscious body of a fellow traveller lies there, starved of oxygen.");

            lBack = new Location("Facing the rear.", "You are facing the back of the lift. There is a hole in the panel.");
            Usables maintenance = new Usables("maintenance");

            lRight = new Location("Facing right.", "You are facing the right of the lift.");

            lCeiling = new Location("Facing the ceiling", "You are facing upwards. You can see a small panel in the top of the lift.");

			lDoors.addExit(new Exit(Exit.Directions.Right, lRight));
            lDoors.addExit(new Exit(Exit.Directions.Left, lPanel));
            lDoors.addExit(new Exit(Exit.Directions.Down, lFloor));
            lDoors.addExit(new Exit(Exit.Directions.Up, lCeiling));

			lRight.addExit(new Exit(Exit.Directions.Right, lBack));
            lRight.addExit(new Exit(Exit.Directions.Left, lDoors));
            lRight.addExit(new Exit(Exit.Directions.Down, lFloor));
            lRight.addExit(new Exit(Exit.Directions.Up, lCeiling));

            lBack.addExit(new Exit(Exit.Directions.Right, lPanel));
            lBack.addExit(new Exit(Exit.Directions.Left, lRight));
            lBack.addExit(new Exit(Exit.Directions.Down, lFloor));
            lBack.addExit(new Exit(Exit.Directions.Up, lCeiling));

            lPanel.addExit(new Exit(Exit.Directions.Right, lDoors));
            lPanel.addExit(new Exit(Exit.Directions.Left, lBack));
            lPanel.addExit(new Exit(Exit.Directions.Down, lFloor));
            lPanel.addExit(new Exit(Exit.Directions.Up, lCeiling));

            lFloor.addExit(new Exit(Exit.Directions.Up, lDoors));

            lCeiling.addExit(new Exit(Exit.Directions.Down, lDoors));

            //Assign default values to the location variables.
            currentLocation = lDoors;
            locationCurrent = "lDoors";
            currentFloor = "floor 1";
			showLocation();
		}

		public void showLocation()
		{
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
			Console.WriteLine(currentLocation.getDescription());
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nYou are on " + currentFloor);
            Console.ResetColor();

            //List the items in the area that the player is in. Yellow indicates things to do with inventory.
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (currentLocation.getInventory().Count > 0)
			{
				Console.WriteLine("\nThis location contains the following:\n");

				for ( int i = 0; i < currentLocation.getInventory().Count; i++ )
				{
                    Console.WriteLine(currentLocation.getInventory()[i].itemName);
				}

                
			}

            //List the usables in the area. Usables also use yellow.
            if (currentLocation.getUsables().Count > 0)
            {
                Console.WriteLine("\nYou can interact with:\n");

                for (int i = 0; i < currentLocation.getUsables().Count; i++)
                {
                    Console.WriteLine(currentLocation.getUsables()[i].usableName);
                }
            }
            Console.ResetColor();

            //List the exits in an area. Exits use Cyan.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nAvailable Exits: \n");

			foreach (Exit exit in currentLocation.getExits() )
			{
				Console.WriteLine(exit.getDirection());
			}

			Console.WriteLine();
            Console.ResetColor();
		}

        // TODO: Implement the input handling algorithm.
		public void doAction(string command)
		{
            if (command == "help")
                Help();
            if (command == "inventory")
                showInventory();
            if (command == "examine")
                Examine();
            if (command == "right")
                Right();
            if (command == "left")
                Left();
            if (command == "up")
                Up();
            if (command == "down")
                Down();
            if (command == "take item")
                TakeItemInventory();
            if (command == "use")
                UseItem();
		}

        //Takes an item. Assigns the player input to a temp variables and checks that against the location inventory. If a match is found, adds that item to inventory.
        private void TakeItemInventory()
        {
            string input = "";

            if (currentLocation.getInventory().Count > 0)
                Console.WriteLine("What do you want to take?");
            else
                Console.WriteLine("There are no items here");

            input = Console.ReadLine();

            Item selected = currentLocation.takeItem(input);
            if (selected != null)
            {
                inventory.Add(selected);
            }
            else
                Console.WriteLine("That item doesn't exist");
        }

        //Handles using Items and Usables.
        private void UseItem()
        {
            Item crowbar = new Item("crowbar");
            string input = "";

            Console.WriteLine("What would you like to use?");
            input = Console.ReadLine();

            //Loop round the usables stored in the current scene.
            for (int y = 0; y < currentLocation.getUsables().Count; y++)
            {
                if (input == currentLocation.getUsables()[y].usableName)
                {
                    switch (currentLocation.getUsables()[y].usableName)
                    {
                        case "floor1":
                            currentFloor = "floor 1";
                            //Removes crowbar from shaft when not on floor 3. Ensures no item duplication.
                            lShaft.removeItem("crowbar");
                            showLocation();
                            break;
                        case "floor2":
                            currentFloor = "floor 2";
                            lShaft.removeItem("crowbar");
                            showLocation();
                            break;
                        case "floor3":
                            currentFloor = "floor 3";
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nThe lift attempts to reach floor 3, but seems to snag on something.");
                            Console.ResetColor();
                            //Add a crowbar to shaft when you're on floor 3.
                            lShaft.addItem(crowbar);                           
                            break;
                        case "opendoors":
                            OpenDoors();
                            break;
                        default:
                            Console.WriteLine("Nothing happens");
                            break;
                    }
                }
            }

            //Does a similar thing for items.
            for (int i = 0; i < inventory.Count(); i++)
            {
                if(input == inventory[i].itemName)
                {
                    if(inventory[i].itemName == "key" && locationCurrent == "lBack")
                    {
                        inventory.Remove(inventory[i]);
                        //using the key in the back of the lift opens up the maintenance shaft.
                        lShaft = new Location("In the maintenance shaft", "You have access to the lift cables from here");
                        lShaft.addExit(new Exit(Exit.Directions.Down, lCeiling));
                        lCeiling.addExit(new Exit(Exit.Directions.Up, lShaft));
                        haveUsedKey = true;
                    }
                    else if (inventory[i].itemName == "crowbar" && locationCurrent == "lDoors")
                    {
                        inventory.Remove(inventory[i]);
                        haveUsedCrowbar = true;
                    }
                    else
                    {
                        Console.WriteLine("\nYou can't use that here!\n");
                    }
                }
            }
        }

        //function that runs the winning of the game.
        private void OpenDoors()
        {
            if (haveUsedCrowbar == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You emerge from the lift with a sense of victory. You have won");
                //Thread.Sleep causes the system to wait for 5 seconds.
                Thread.Sleep(5000);
                //the game ends when isRunning = false;
                isRunning = false;
                return;
            }
            else
            {
                Console.WriteLine("Nothing happens");
            }
        }

        private void showInventory()
		{
            Console.ForegroundColor = ConsoleColor.Yellow;
            //if your inventory has things in it, display them. Else, say that the bag is empty.
            if ( inventory.Count > 0 )
			{
				Console.WriteLine("\nyou have the following on your person:\n");

                //Checks each item in your inventory, then writes them to the console.
                foreach ( Item item in inventory )
				{
                    //ToString() is an override which returns the item name rather than the class type.
                    Console.WriteLine(item.ToString());
				}
			}
			else
			{
				Console.WriteLine("Your bag is empty.");
			}

			Console.WriteLine("");
            Console.ResetColor();
		}

        //Update runs every frame.
        public void Update()
		{
			string currentCommand = Console.ReadLine().ToLower();

          

            // instantly check for a quit
            if (currentCommand == "quit" || currentCommand == "q")
			{
				isRunning = false;
				return;
			}
				
			// otherwise, process commands.
			doAction(currentCommand);
		}

        //Handles turning right. Uses the switch statement based on the static string locationCurrent to change your location.
        public void Right()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "lDoors":
                    currentLocation = lRight;
                    locationCurrent = "lRight";
                    showLocation();
                    break;
                case "lRight":
                    currentLocation = lBack;
                    locationCurrent = "lBack";
                    showLocation();
                    break;
                case "lBack":
                    currentLocation = lPanel;
                    locationCurrent = "lPanel";
                    showLocation();
                    break;
                case "lPanel":
                    currentLocation = lDoors;
                    locationCurrent = "lDoors";
                    showLocation();
                    break;
            }
        }

        //Handles turning left.
        public void Left()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "lDoors":
                    currentLocation = lPanel;
                    locationCurrent = "lPanel";
                    showLocation();
                    break;
                case "lPanel":
                    currentLocation = lBack;
                    locationCurrent = "lBack";
                    showLocation();
                    break;
                case "lBack":
                    currentLocation = lRight;
                    locationCurrent = "lRight";
                    showLocation();
                    break;
                case "lRight":
                    currentLocation = lDoors;
                    locationCurrent = "lDoors";
                    showLocation();
                    break;
            }
        }

        //Handles turning right
        public void Up()
        {
            //Opens the maintenance shaft if the key is used.
            if (haveUsedKey == true)
            {
                switch (locationCurrent)
                {
                    default:
                        currentLocation = lCeiling;
                        locationCurrent = "lCeiling";
                        showLocation();
                        break;
                    case "lFloor":
                        currentLocation = lDoors;
                        locationCurrent = "lDoors";
                        showLocation();
                        break;
                    case "lShaft":
                        Console.WriteLine("You can't go that way!");
                        break;
                    case "lCeiling":
                        currentLocation = lShaft;
                        locationCurrent = "lShaft";
                        showLocation();
                        break;
                }
            }
            //Else don't let the player go up there.
            else
            {
                switch (locationCurrent)
                {
                    default:
                        currentLocation = lCeiling;
                        locationCurrent = "lCeiling";
                        showLocation();
                        break;
                    case "lFloor":
                        currentLocation = lDoors;
                        locationCurrent = "lDoors";
                        showLocation();
                        break;
                }
            }    
        }

        //Handles downward movement.
        public void Down()
        {
            switch (locationCurrent)
            {
                default:
                    currentLocation = lFloor;
                    locationCurrent = "lFloor";
                    showLocation();
                    break;
                case "lFloor":
                    Console.WriteLine("You can't go that way!");
                    break;
                case "lCeiling":
                    currentLocation = lDoors;
                    locationCurrent = "lDoors";
                    showLocation();
                    break;
                case "lShaft":
                    currentLocation = lCeiling;
                    locationCurrent = "lCeiling";
                    showLocation();
                    break;
            }
        }

        //The help functions tells people what they can do in any room.
        public void Help()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("In this location, you can move: ");
            //Check for exits in the current location.
            foreach (Exit exit in currentLocation.getExits())
            {
                Console.WriteLine(exit.getDirection());
            }
            Console.WriteLine("");
            //Check for items in the location inventories.
            if (currentLocation.getInventory().Count > 0)
            {
                Console.WriteLine("\nThis location contains the following:\n");
                for (int i = 0; i < currentLocation.getInventory().Count; i++)
                {
                    Console.WriteLine(currentLocation.getInventory()[i].ToString());
                }
            }
            //Give the player potential options.
            Console.WriteLine("");
            Console.WriteLine("Type 'inventory' to look at your inventory");
            Console.WriteLine("Type 'examine' to observe the area for details");
            Console.WriteLine("Type 'take item' to take an item from an area (item names are case sensitive");
            Console.WriteLine("Type 'use', then the name of an item in your inventory or a usable object in the environment to use it");
            Console.ResetColor();
        }

        //Gives the player more info about a location.
        public void Examine()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            switch (locationCurrent)
            {
                case "lDoors":
                    Console.WriteLine("The entire problem lays in front of you: the doors are tightly sealed.");
                    break;
                case "lPanel":
                    Console.WriteLine("The elevator panel contains the buttons for the floors, opening and closing the doors, and an alarm button.");
                    break;
                case "lBack":
                    Console.WriteLine("On closer inspection, the hole seems deliberate; intended to house something.");
                    break;
                case "lCeiling":
                    Console.WriteLine("The panel in the top is for maintenance, and leads to the shaft. It is sealed tightly");
                    break;
                case "lFloor":
                    if (haveCheckedFloor == false)
                    {
                        //Reveals to the player a key on the ground. They can't pick it up if they haven't examined the floor.
                        haveCheckedFloor = true;
                        Console.WriteLine("The name tag on the unconscious man says 'R.K.' His pocket contains a small key");
                        Item maintenanceKey = new Item("key");
                        lFloor.addItem(maintenanceKey);
                        showLocation();
                    }
                    else
                        Console.WriteLine("Nothing new here");
                    break;
                default:
                    Console.WriteLine("There is nothing particularly of interest in this area");
                    break;
            }
            Console.ResetColor();
        }
    }
}
