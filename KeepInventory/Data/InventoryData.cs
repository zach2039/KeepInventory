using VRage.Game.ModAPI;
using System.Collections.Generic;
using System;

namespace KeepInventory.Data
{
    [Serializable]
    public class InventoryData
    {
        public SerializableDictionary<ulong, List<IMyInventoryItem>> PlayerInventories;
    }
}
