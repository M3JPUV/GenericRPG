using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Inventory
    {
        //Keeps count of the number of potions in our inventory.
        private int inventoryCount;

        //Initialization of a new inventory with 0 potions.
        public Inventory()
        {
            this.inventoryCount = 0;
        }
        //The player receiving a potion after battle.
        public void getPotion()
        {
            inventoryCount++;
        }
        //Returns the number of potions contained in this inventory
        public int PotionCount()
        {
            return inventoryCount;
        }
        //Uses a number of the possessed potions. Checking is done on call. Potions used = dec
        public void decreasePotionCount(int dec)
        {
            this.inventoryCount -= dec;
        }
    }
}
