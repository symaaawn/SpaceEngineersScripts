using Sandbox.ModAPI.Ingame;
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
            private readonly IMyGridTerminalSystem _gridTerminalSystem;

            #endregion

            #region construction

            public InventoryActions(Logger logger, IMyGridTerminalSystem myGridTerminalSystem)
            {
                _logger = logger;
                _gridTerminalSystem = myGridTerminalSystem;
            }

            #endregion

            #region methods

            public IMyInventory GetTargetInventory(string productionBlockName)
            {
                var productionBlock = _gridTerminalSystem.GetBlockWithName(productionBlockName);

                if (productionBlock == null)
                {
                    _logger.LogError($"Production block with name {productionBlockName} not found.");
                    return null;
                }

                if (productionBlock is IMyProductionBlock)
                {
                    var prodBlock = productionBlock as IMyProductionBlock;
                    return prodBlock.InputInventory;
                }
                else
                {
                    _logger.LogError($"Block with name {productionBlockName} is not a production block.");
                    return null;
                }
            }

            public IMyInventory GetSourceInventory(string productionBlockName)
            {
                var productionBlock = _gridTerminalSystem.GetBlockWithName(productionBlockName);
                if (productionBlock == null)
                {
                    _logger.LogError($"Production block with name {productionBlockName} not found.");
                    return null;
                }
                if (productionBlock is IMyProductionBlock)
                {
                    var prodBlock = productionBlock as IMyProductionBlock;
                    return prodBlock.OutputInventory;
                }
                else
                {
                    _logger.LogError($"Block with name {productionBlockName} is not a production block.");
                    return null;
                }
            }

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
