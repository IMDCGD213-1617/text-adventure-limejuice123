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
        public Location lDoors, lPanel, lBack, lFloor, lCeiling, lRight, lShaft;
        public static string locationCurrent;

        //Exit.Directions direction;

		public bool isRunning = true;

		private List<Item> inventory;

		public Game()
		{
			inventory = new List<Item>();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The lift has stopped. You are trapped.");
            Console.ResetColor();

			// build the "map"
			lDoors = new Location("Facing Forward.", "You are facing the doors to the lift.");

			lPanel = new Location("Facing Left.", "You are facing the lift panel.");

			lFloor = new Location("Facing Down.", "You are facing the floor. The unconscious body of a fellow traveller lies there, starved of oxygen.");
            Item crowbar = new Item("Crowbar");
            lFloor.addItem(crowbar);

            lBack = new Location("Facing the rear.", "You are facing the back of the lift. The panel is discoloured in some places.");

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
		}

        private void TakeItemInventory()
        {
            string input = "";
            string[] inputCheck = new string[] { };

            if (currentLocation.getInventory().Count > 0)
                Console.WriteLine("What do you want to take?");
            else
                Console.WriteLine("There are no items here");

            for (int i = 0; i == currentLocation.getInventory().Count; i++)
            {
                inputCheck[i] = currentLocation.getInventory()[i].itemName;
            }

            input = Console.ReadLine();

            if (inputCheck.Contains(input))
            {
                currentLocation.takeItem(input);
            }
            else
                Console.WriteLine("That item doesn't exist");
        } 

        private void showInventory()
		{
            Console.ForegroundColor = ConsoleColor.Yellow;
            if ( inventory.Count > 0 )
			{
				Console.WriteLine("\nA quick look in your bag reveals the following:\n");

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
                case "l2":
                    Console.WriteLine("This looks like a bug-out cabin of sorts. A place where someone might hide for a while");
                    break;
                case "l3":
                    Console.WriteLine("The man's jacket appears to have some papers in the pockets");
                    break;
                case "l4":
                    Console.WriteLine("The bars seem sturdy enough to support a human's weight.");
                    break;
                default:
                    Console.WriteLine("There is nothing particularly of interest in this area");
                    break;
            }
            Console.ResetColor();
        }
    }
}
