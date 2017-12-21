using VRage.Game.ModAPI;
using System.Collections.Generic;

namespace KeepInventory.Data
{
    public class InventoryData
    {
        public Dictionary<ulong, IMyInventory> PlayerInventories { get; set; }
    }
}
