using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.API;
using Torch.API.Event;
using KeepInventory.Mngr;
using KeepInventory;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ModAPI.IMyEntityController;
using VRage.Game.ModAPI;

namespace KeepInventory.Evnts
{
    class KeepInventoryEventHandler : IEventHandler
    {
        public static KeepInventoryPlugin Instance { get; private set; }

        public event Action<IPlayer> PlayerJoined;
        public event Action<IPlayer> PlayerLeft;
        public event Action<IMyControllableEntity, IMyControllableEntity> ControlledEntityChanged;

        public KeepInventoryEventHandler(ITorchBase torch, KeepInventoryPlugin inst)
        {
            Instance = inst;

            PlayerJoined += OnJoin;
            PlayerLeft += OnLeave;
            ControlledEntityChanged += OnEntitySwitch;
        }

        /// <summary>
        /// OnJoin will handle the preparation of a inventory save slot
        /// in the inventory save file for the joining player.
        /// </summary>
        /// <param name="player">The player joining the server</param>
        public void OnJoin(IPlayer player)
        {
            // Call manager to handle joining player.
            Instance.InventoryManager.QueuePlayerForInventory(player);
        }
        
        /// <summary>
        /// OnLeave will handle the saving of inventory to a player's respective inventory
        /// slot in the inventory save file when the player leaves the server.
        /// </summary>
        /// <param name="player">The player leaving the server</param>
        public void OnLeave(IPlayer player)
        {
            // Call manager to handle leaving player.
            Instance.InventoryManager.SaveInventoryToSlot(player);
        }

        /// <summary>
        /// OnEntitySwitch will handle when a player has switched controlled entities.
        /// We will check if the player was queued to receive an inventory. If the player was, we will handle 
        /// it via a KeepInventoryManager call.
        /// </summary>
        public void OnEntitySwitch(IMyControllableEntity oldEntity, IMyControllableEntity newEntity)
        {
            // Check if the player has exited the spawn screen and is controlling a character.
            if (oldEntity == null && newEntity is IMyCharacter)
            {
                // If a player has changed controlled entities from nothing to a character, we need to evaluate our queued player list.
                Instance.InventoryManager.LoadInventoryFromSlot();
            }
        }
    }
}
