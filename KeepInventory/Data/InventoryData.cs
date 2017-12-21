using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Torch;
using Torch.API;
using Torch.API.Plugins;
using Torch.Managers;
using VRage.Game;
using VRage.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using System.Collections.Generic;

namespace KeepInventory.Data
{
    class InventoryData
    {
        public Dictionary<ulong, IMyInventory> PlayerInventories { get; set; }
    }
}
