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
        public Location l1, l2, l3, l4, l5, l6, l7, l8;
        public static string locationCurrent;

        //Exit.Directions direction;

		public bool isRunning = true;

		private List<Item> inventory;

		public Game()
		{
			inventory = new List<Item>();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You are a MOSSAD agent. You are tasked with retrieving the U-238 tamper from a \nprototype Soviet missile, to aid in Israel's nuclear program.");
            Console.ResetColor();

			// build the "map"
			l1 = new Location("Entrance to shaft \nDepth: 100m", "You stand at the top of a large missile silo. There is a small building to the \nright of your location.");

			l2 = new Location("Cabin exterior \nDepth: 100m", "You see a cabin, long since abandoned.");

			l3 = new Location("Cabin interior \nDepth: 100m", "The cabin contains what looks like the scene of a murder; a corpse lies battered and bruised, with half his head missing. His backpack lies discarded next to him.");
            Item employeesBackpack = new Item("Employee's Backpack");
            Item climbingGear = new Item("Climbing Gear");
            Item noticeOfTermination = new Item("Notice of Termination");
            Item accessCard1 = new Item("Access Card: Level 1"); 
            l3.addItem(employeesBackpack);
            l3.addItem(climbingGear);
            l3.addItem(noticeOfTermination);
            l3.addItem(accessCard1);

            l4 = new Location("Old elevator shaft \nDepth: 100m", "You see an old elevator, likely used for descending the shaft. It has very thick, widely spaced bars, and a large control box");


			//l1.addExit(new Exit(Exit.Directions.Forward, l2));
			l1.addExit(new Exit(Exit.Directions.Right, l2));
            l1.addExit(new Exit(Exit.Directions.Forward, l4));

			l2.addExit(new Exit(Exit.Directions.In, l3));
            l2.addExit(new Exit(Exit.Directions.Left, l1));

			l3.addExit(new Exit(Exit.Directions.Out, l2));

            l4.addExit(new Exit(Exit.Directions.Backward, l1));

			currentLocation = l1;
            locationCurrent = "l1";
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
            if (command == "forward" || command == "Forward" || command == "walk forward" || command == "go forward")
                Forward();
            if (command == "backward")
                Backward();
            if (command == "right")
                Right();
            if (command == "left")
                Left();
            if (command == "in")
                In();
            if (command == "out")
                Out();
            if (command == "take item")
                Inventory();
		}

        private void Inventory()
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

        public void Forward()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "l1":
                    currentLocation = l4;
                    locationCurrent = "l4";
                    showLocation();
                    break;
            }
        }
        
        public void Backward()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "l4":
                    currentLocation = l1;
                    locationCurrent = "l1";
                    showLocation();
                    break;
            }
        } 

        public void Right()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "l1":
                    currentLocation = l2;
                    locationCurrent = "l2";
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
                case "l2":
                    currentLocation = l1;
                    locationCurrent = "l1";
                    showLocation();
                    break;
            }
        }

        public void In()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "l2":
                    currentLocation = l3;
                    locationCurrent = "l3";
                    showLocation();
                    break;
            }
        }

        public void Out()
        {
            switch (locationCurrent)
            {
                default:
                    Console.WriteLine("You can't go that way!");
                    break;
                case "l3":
                    currentLocation = l2;
                    locationCurrent = "l2";
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
