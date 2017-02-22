using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{    
	class Item
	{
        //the itemName string holds the name of the items, and is parsed in in the Game class.
        public string itemName;

        //Parses in the name to be held in an instance.
        public Item (string name)
        {
            itemName = name;
        }

        //Overrides the ToString() command to return the name of the item rather than the class type.
        public override string ToString()
        {
            return itemName;
        }
    }
}
