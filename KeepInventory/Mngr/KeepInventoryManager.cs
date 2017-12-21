﻿using System.IO;
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
using Torch.Utils;
using Torch.API.Event;
using Torch.API.Managers;
using VRage.Game;
using VRage.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using System.Collections.Generic;
using KeepInventory.Data;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace KeepInventory.Mngr
{
    class KeepInventoryManager : EntityManager
    {
        // Main plugin instance.
        public static KeepInventoryPlugin Instance { get; private set; }

        // Data storage fields.
        Persistent<InventoryData> InventorySaveFile { get; private set; }
        private HashSet<IMyPlayer> _queuedPlayers;

        public KeepInventoryManager(ITorchBase torch, KeepInventoryPlugin inst) : base(torch)
        {
            Instance = inst;

            if (InventorySaveFile.Data == null) {
                InventorySaveFile = new Persistent<InventoryData>(Path.Combine(inst.StoragePath, "InventorySaveFile.xml"),
                    new InventoryData());
            }
        }

        /// <summary>
        /// Queue a joining player's inventory to be loaded from saved data, if they had a slot saved.
        /// </summary>
        /// <param name="player">The joining player</param>
        public void QueuePlayerForInventory(IPlayer player)
        {
            // Get a IMyPlayer version of the loading player to test and manipulate against.
            IMyPlayer loadedPlayer = Instance.Torch.CurrentSession.Managers.GetManager<IMultiplayerManagerBase>().GetPlayerBySteamId(player.SteamId);
            
            // Random null check to calm nerves.
            if (loadedPlayer == null) return;

            // If the player has no controlled entity, we can assume the player is in the respawn menu and can be added to queued player list. By
            // adding them to the list, they will be ensured inventory sync on spawn based on the last time they logged off. If they have no slot saved,
            // we don't bother adding them to the queue.
            if (loadedPlayer.Controller.ControlledEntity == null)
            {
                // The loading player was in the game but has not yet spawned. Queue the player to recieve their inventory once they spawn in if they haved a saved inventory.
                if (InventorySaveFile.Data.PlayerInventories.ContainsKey(player.SteamId))
                {
                    _queuedPlayers.Add(loadedPlayer);
                }
            }
        }

        /// <summary>
        /// Load a player's inventory from a stored slot and clean the slot afterwards.
        /// </summary>
        public void LoadInventoryFromSlot()
        {
            // If there are no players who need an inventory, get outta here.
            if (_queuedPlayers.Count == 0) return;

            // Since there are players waiting on their inventory, we need to give it to em.
            foreach (IMyPlayer player in _queuedPlayers) {
                // If the player does not have an entry in storage, we also don't bother trying to restore the inventory.
                if (!InventorySaveFile.Data.PlayerInventories.ContainsKey(player.SteamUserId)) continue;

                ConnectionHelper.SendMessageToPlayer(player.SteamUserId,
                new MessageSyncRestore
                {
                    Inventory = InventorySaveFile.Data.PlayerInventories[player.SteamUserId],
                });
            }
        }

        /// <summary>
        /// Save a player's inventory to storage.
        /// We want KeepInventory
        /// </summary>
        /// <param name="player">The player with an inventory that needs to be saved to storage</param>
        public void SaveInventoryToSlot(IPlayer player)
        {
            // Get a IMyPlayer version of the leaving player to test and manipulate against.
            IMyPlayer leavingPlayer = Instance.Torch.CurrentSession.Managers.GetManager<IMultiplayerManagerBase>().GetPlayerBySteamId(player.SteamId);

            // Random null check to calm nerves.
            if (leavingPlayer == null) return;

            // If the player is disconnecting from a cyro chamber, we don't bother saving his inventory.
            if (leavingPlayer.Controller.ControlledEntity is IMyCryoChamber) return;
            // If the player is dead, we also don't bother saving his inventory to storage.
            if (leavingPlayer.Character.IsDead) return;

            // Now that the leaving player has passed both checks, we can save their inventory to storage.
            InventorySaveFile.Data.PlayerInventories.Add(player.SteamId, leavingPlayer.Character.GetInventory());
        }

        /// <summary>
        /// Clear a player's inventory data from storage if they have received their saved inventory.
        /// </summary>
        /// <param name="player">The player who is getting their inventory erased</param>
        public void CleanInventorySlot(IPlayer player)
        {

        }

        /// <summary>
        /// Syncs player inventory after their inventory is restored.
        /// </summary>
        /// <param name="steamId">The steam Id of the player getting the message</param>
        /// <param name="inventory">The inventory to send</param>
        public static void SendInventorySyncMessageToPlayer(ulong steamId, IMyInventory inventory)
        {
            MyModAPIHelper.MyMultiplayer.Static.SendMessageTo
        }
    }
}