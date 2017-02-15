using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
	class Game
	{
		Location currentLocation;
        Usables currentUsable;
        public Location lDoors, lPanel, lBack, lFloor, lCeiling, lRight, lShaft;
        public static string locationCurrent;

        //Exit.Directions direction;

		public bool isRunning = true;

		private List<Item> inventory;

        private bool haveCheckedFloor = false;

        public Game()
		{
			inventory = new List<Item>();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The lift has stopped. You are trapped.");
            Console.ResetColor();

			// build the "map"
			lDoors = new Location("Facing Forward.", "You are facing the doors to the lift.");

			lPanel = new Location("Facing Left.", "You are facing the lift panel.");
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

            lShaft = new Location("In the maintenance shaft", "You have access to the lift cables from here");

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
            lCeiling.addExit(new Exit(Exit.Directions.Up, lShaft));

            lShaft.addExit(new Exit(Exit.Directions.Down, lCeiling));

			currentLocation = lDoors;
            locationCurrent = "lDoors";
			showLocation();
		}

		public void showLocation()
		{
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
			Console.WriteLine(currentLocation.getDescription());
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            if (currentLocation.getInventory().Count > 0)
			{
				Console.WriteLine("\nThis location contains the following:\n");

				for ( int i = 0; i < currentLocation.getInventory().Count; i++ )
				{
                    Console.WriteLine(currentLocation.getInventory()[i].itemName);
				}

                
			}

            if (currentLocation.getUsables().Count > 0)
            {
                Console.WriteLine("\nYou can interact with:\n");

                for (int i = 0; i < currentLocation.getUsables().Count; i++)
                {
                    Console.WriteLine(currentLocation.getUsables()[i].usableName);
                }
            }
            Console.ResetColor();

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

        private void UseItem()
        {
            string input = "";

            Console.WriteLine("What would you like to use?");
            input = Console.ReadLine();

            for(int i = 0; i < inventory.Count(); i++)
            {
                if(input == inventory[i].itemName)
                {
                    if(inventory[i].itemName == "key" && locationCurrent == "lBack")
                    {
                        inventory.Remove(inventory[i]);
                    }
                    else
                    {
                        Console.WriteLine("\nYou can't use that here!\n");
                    }
                }
            }
        }

        private void showInventory()
		{
            Console.ForegroundColor = ConsoleColor.Yellow;
            if ( inventory.Count > 0 )
			{
				Console.WriteLine("\nyou have the following on your person:\n");

				foreach ( Item item in inventory )
				{
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

        public void Up()
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

        public void Help()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("In this location, you can move: ");
            foreach (Exit exit in currentLocation.getExits())
            {
                Console.WriteLine(exit.getDirection());
            }
            Console.WriteLine("");
            if (currentLocation.getInventory().Count > 0)
            {
                Console.WriteLine("\nThis location contains the following:\n");
                for (int i = 0; i < currentLocation.getInventory().Count; i++)
                {
                    Console.WriteLine(currentLocation.getInventory()[i].ToString());
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Type 'inventory' to look at your inventory");
            Console.WriteLine("Type 'examine' to observe the area for details");
            Console.WriteLine("Type 'take item' to take an item from an area (item names are case sensitive");
            Console.WriteLine("Type 'use', then the name of the item to use it");
            Console.ResetColor();
        }
    
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
