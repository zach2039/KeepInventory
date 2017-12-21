using Torch;
using Torch.API;
using Torch.API.Plugins;
using Torch.API.Event;
using Torch.API.Managers;
using KeepInventory.Mngr;
using KeepInventory.Evnts;
using System.Windows.Controls;
using NLog;

namespace KeepInventory
{
    public class KeepInventoryPlugin : TorchPluginBase, IWpfPlugin
    {
        public static KeepInventoryPlugin Instance { get; private set; }
        public KeepInventoryManager InventoryManager { get; private set; }
        public KeepInventoryEventHandler InventoryEventHandler { get; private set; }

        public static readonly Logger Log = LogManager.GetLogger("KeepInventory");
        
        // Constructor.
        public KeepInventoryPlugin()
        {
            Instance = this;
        }

        // Initialization.
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
           
            Log.Info("Torch:" + torch);
            Log.Info("Inst:" + Instance);

            InventoryManager = new KeepInventoryManager(torch, Instance);
            torch.Managers.AddManager(InventoryManager);

            InventoryEventHandler = new KeepInventoryEventHandler(torch, Instance);
            torch.Managers.GetManager<IEventManager>().RegisterHandler(InventoryEventHandler);
        }

        public override void Dispose()
        {
            base.Dispose();

            // Attempt to save data on dispose.
            try
            {
                InventoryManager.InventorySaveFile.Save();
            }
            catch
            {
                // TODO: Add logger and spit out some junk.
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public UserControl GetControl()
        {
            return null;
        }
    }
}
