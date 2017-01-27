using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{    
	class Item
	{
        public string itemName;
        //public string usedWith;
        //public string location = Game.locationCurrent;

        public Item()
		{
           
		}

        public Item (string name)
        {
            itemName = name;
        }

        /*public void ItemAssign()
        {
            switch (location)
            {
                default:
                    itemName[0] = "No items";
                    break;
                case "l3":
                    itemName[0] = "Employee's Backpack";
                    itemName[1] = "Climbing Gear";
                    itemName[2] = "Notice of Termination";
                    itemName[3] = "Access Card: Level 1";
                    break;
            }
        } 

        public static void ItemDisplay()
        {
            for (int i = 0; i < itemName.Length; i++)
            {
                Console.WriteLine(itemName[i]);
            }
        }

        public void ItemPickUp()
        {

        }
        */
	}
}
