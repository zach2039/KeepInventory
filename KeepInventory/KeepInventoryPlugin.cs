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

            InventoryManager = new KeepInventoryManager(Instance.Torch, Instance);
            InventoryEventHandler = new KeepInventoryEventHandler(Instance.Torch, Instance);

            Instance.Torch.Managers.AddManager(InventoryManager);
            Instance.Torch.Managers.GetManager<IEventManager>().RegisterHandler(InventoryEventHandler);

            Torch.SessionLoaded += Torch_SessionLoaded;
        }

        private void Torch_SessionLoaded()
        {
            InventoryEventHandler.Init();
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
