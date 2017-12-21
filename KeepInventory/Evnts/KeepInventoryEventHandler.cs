using System;
using Torch.API;
using Torch.API.Event;
using VRage.Game.ModAPI;
using Torch.API.Managers;
using VRage.Game.ModAPI.Interfaces;

namespace KeepInventory.Evnts
{
    public class KeepInventoryEventHandler : IEventHandler
    {
        public static KeepInventoryPlugin Instance { get; private set; }

        public KeepInventoryEventHandler(ITorchBase torch, KeepInventoryPlugin inst)
        {
            Instance = inst;
            inst.Torch.Managers.GetManager<IMultiplayerManagerBase>().PlayerJoined += OnJoin;
            inst.Torch.Managers.GetManager<IMultiplayerManagerBase>().PlayerLeft += OnLeave;
            inst.Torch.CurrentSession.KeenSession.CameraAttachedToChanged += OnEntitySwitch;
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
        public void OnEntitySwitch(IMyCameraController oldController, IMyCameraController newController)
        {
            // Check if the player has exited the spawn screen and is controlling a character.
            if (oldController == null && newController is IMyCharacter)
            {
                // If a player has changed controlled entities from nothing to a character, we need to evaluate our queued player list.
                Instance.InventoryManager.LoadInventoryFromSlot();
            }
        }
    }
}
