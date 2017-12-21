using Torch;
using Torch.API;
using Torch.API.Plugins;
using Torch.API.Event;
using Torch.API.Managers;
using KeepInventory.Mngr;
using KeepInventory.Evnts;
using System.Windows.Controls;

namespace KeepInventory
{
    public class KeepInventoryPlugin : TorchPluginBase
    {
        public static KeepInventoryPlugin Instance { get; private set; }
        public KeepInventoryManager InventoryManager { get; private set; }
        public KeepInventoryEventHandler InventoryEventHandler { get; private set; }
        
        // Constructor.
        public KeepInventoryPlugin()
        {
            Instance = this;
        }

        // Initialization.
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
