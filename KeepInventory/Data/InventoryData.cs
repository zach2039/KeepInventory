using VRage.Game.ModAPI;
using System.Collections.Generic;

namespace KeepInventory.Data
{
    public class InventoryData
    {
        public SerializableDictionary<ulong, IMyInventory> PlayerInventories { get; set; }

        public InventoryData()
        {
            PlayerInventories = new SerializableDictionary<ulong, IMyInventory>();
        }
    }
}
