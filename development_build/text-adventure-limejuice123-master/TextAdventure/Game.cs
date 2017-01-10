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

        Exit.Directions direction;

		public bool isRunning = true;

		private List<Item> inventory;

		public Game()
		{
			inventory = new List<Item>();

            Console.WriteLine("You are a MOSSAD agent. You are tasked with retrieving the U-238 tamper from a \nprototype Soviet missile, to aid in Israel's nuclear program.");

			// build the "map"
			Location l1 = new Location("Entrance to shaft \nDepth: 100m", "You stand at the top of a large missile silo. There is a small building to the \nright of your location.");

			Location l2 = new Location("Cabin exterior \nDepth: 100m", "You see a cabin, long since abandoned.");
			Item cabinDoor = new Item();
			l2.addItem(cabinDoor);

			Location l3 = new Location("Cabin interior \nDepth: 100m", "The cabin contains what looks like the scene of a murder; a corpse lies battered and bruised, with half his head missing. His backpack lies discarded next to him.");
            Item employeesBackpack = new Item();
            Item climbingGear = new Item();
            Item noticeOfTermination = new Item();
            Item accessCard1 = new Item(); 
            l3.addItem(employeesBackpack);
            l3.addItem(climbingGear);
            l3.addItem(noticeOfTermination);
            l3.addItem(accessCard1);

			//l1.addExit(new Exit(Exit.Directions.Forward, l2));
			l1.addExit(new Exit(Exit.Directions.Right, l2));

			l2.addExit(new Exit(Exit.Directions.In, l3));

			l3.addExit(new Exit(Exit.Directions.Out, l2));

			currentLocation = l1;
			showLocation();
		}

		public void showLocation()
		{
			Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
			Console.WriteLine(currentLocation.getDescription());

			if (currentLocation.getInventory().Count > 0)
			{
				Console.WriteLine("\nThis location contains the following:\n");

				for ( int i = 0; i < currentLocation.getInventory().Count; i++ )
				{
					Console.WriteLine(currentLocation.getInventory()[i].ToString());
				}
			}
	
			Console.WriteLine("\nAvailable Exits: \n");

			foreach (Exit exit in currentLocation.getExits() )
			{
				Console.WriteLine(exit.getDirection());
			}

			Console.WriteLine();
		}

        // TODO: Implement the input handling algorithm.
		public void doAction(string command)
		{
            if (command == "help")
                Help();
            if (command == "inventory")
                showInventory();
            else
                Console.WriteLine("\nInvalid command. (enter 'help' to display possible actions.)\n");
		}

		private void showInventory()
		{
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

        public void GoToNewLocation()
        {
            switch (direction)
            {
                case Exit.Directions.Forward:
                    break;
                case Exit.Directions.Backward:
                    break;
                case Exit.Directions.Left:
                    break;
                case Exit.Directions.Right: 
                    break;
                case Exit.Directions.Up:
                    break;
                case Exit.Directions.Down:
                    break;
                case Exit.Directions.In:
                    break;
                case Exit.Directions.Out:
                    break;
            }
        }

        public void Help()
        {
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
            Console.WriteLine("Type 'use', then the name of the item to use it");
        }
	}
}
