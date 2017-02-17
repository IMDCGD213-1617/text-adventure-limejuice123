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

        public Item (string name)
        {
            itemName = name;
        }

        public override string ToString()
        {
            return itemName;
        }
    }
}
