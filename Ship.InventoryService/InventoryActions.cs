using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        public class InventoryActions
        {
            #region private fields

            private readonly Logger _logger;

            #endregion

            #region construction

            public InventoryActions(Logger logger)
            {
                _logger = logger;
            }

            #endregion

            #region methods

            public bool TransferItems(IMyInventory sourceInventory, IMyInventory targetInventory, MyItemType itemType, MyFixedPoint amount)
            {
                // Check if the source inventory has enough items
                if (!sourceInventory.ContainItems(amount, itemType))
                {
                    _logger.LogWarning($"Source inventory {sourceInventory.Owner.DisplayName} does not contain enough of item {itemType} to transfer {amount} units.");
                    return false;
                }

                // Check if the target inventory has enough space
                if(!targetInventory.CanItemsBeAdded(amount, itemType))
                {
                    _logger.LogWarning($"Target inventory {targetInventory.Owner.DisplayName} does not have enough space to receive {amount} units of item {itemType}.");
                    return false;
                }

                // Check if the source and target are connected
                if (!sourceInventory.IsConnectedTo(targetInventory))
                {
                    _logger.LogWarning($"Source inventory {sourceInventory.Owner.DisplayName} is not connected to target inventory {targetInventory.Owner.DisplayName}.");
                    return false;
                }

                var inventoryItem = sourceInventory.FindItem(itemType);

                if(inventoryItem == null || inventoryItem.Value.Amount < amount)
                {
                    _logger.LogWarning($"Source inventory {sourceInventory.Owner.DisplayName} does not have enough of item {itemType} to transfer {amount} units.");
                    return false;
                }

                return sourceInventory.TransferItemTo(targetInventory, inventoryItem.Value, amount);
            }

            #endregion
        }
    }
}
