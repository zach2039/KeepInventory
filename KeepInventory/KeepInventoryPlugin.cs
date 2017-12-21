using NLog;
using Sandbox;
using Sandbox.Engine.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Torch.API.Event;
using Torch.API.Managers;
using VRage.Game;
using VRage.Utils;
using KeepInventory.Mngr;
using KeepInventory.Evnts;

namespace KeepInventory
{
    public class KeepInventoryPlugin : TorchPluginBase, IWpfPlugin
    {
        public static KeepInventoryPlugin Instance { get; private set; }
        public KeepInventoryManager InventoryManager { get; private set; }
        public KeepInventoryEventHandler InventoryEventHandler { get; private set; }
        
        public KeepInventoryPlugin()
        {
            Instance = this;
        }

        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            var invenmgr = new KeepInventoryManager(torch, this);
            torch.Managers.AddManager(invenmgr);
            InventoryManager = invenmgr;

            var evnthdlr = new KeepInventoryEventHandler(torch, this);
            torch.Managers.GetManager<IEventManager>().RegisterHandler(evnthdlr);
            InventoryEventHandler = evnthdlr;
        }
    }
}
