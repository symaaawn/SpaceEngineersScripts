using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace IngameScript
{
    partial class Program
    {
        public class InventoryManager
        {
            #region private fields

            private readonly Logger _logger;
            private readonly InventoryActions _inventoryActions;

            #endregion

            #region properties

            private CargoContainerCollection CargoContainerCollection { get; set; }

            #endregion

            #region construction

            public InventoryManager(Logger logger, InventoryActions inventoryActions, List<IMyCargoContainer> cargoContainers)
            {
                _logger = logger;
                _inventoryActions = inventoryActions;
                CargoContainerCollection = new CargoContainerCollection(cargoContainers);
                _logger.LogInfo($"Initialized InventoryManager with {CargoContainerCollection.ContainerCount} cargo containers. ({CargoContainerCollection.ItemCount} items)");
            }

            #endregion

            #region methods

            public void Update()
            {
                // Update logic for inventory management:

                // ToDo: Update inventory displays

                // ToDo: Check for low/high stock and trigger transfers or consolidate inventories
            }

            public List<MyInventoryItem> GetInventory(InventoryServiceMessage_GetInventory getMessage)
            {
                var items = new List<MyInventoryItem>();
                var tempItems = new List<MyInventoryItem>();
                var cargoContainerInventories = CargoContainerCollection.GetInventories();

                foreach (var inventory in cargoContainerInventories)
                {
                    inventory.GetItems(tempItems);
                    items.AddRange(tempItems);
                    tempItems.Clear();
                }

                return items;
            }

            public bool PullItems(InventoryServiceMessage_PullItems pullMessage)
            {
                var sourceInventory = _inventoryActions.GetSourceInventory(pullMessage.SourceInventory);
                if (sourceInventory == null)
                {
                    _logger.LogError($"Source inventory '{pullMessage.SourceInventory}' not found.");
                    return false;
                }

                var itemType = (MyItemType)MyDefinitionId.Parse(pullMessage.Item);

                if (!_inventoryActions.TransferItems(sourceInventory, CargoContainerCollection.GetCargoContainerWithEnoughSpace(itemType, pullMessage.Amount).Inventory, itemType, pullMessage.Amount))
                {
                    _logger.LogError($"Failed to pull items '{pullMessage.Item}' from '{pullMessage.SourceInventory}'.");
                    return false;
                }

                return true;
            }

            public bool PushItems(InventoryServiceMessage_PushItems pushMessage)
            {
                var targetInventory = _inventoryActions.GetTargetInventory(pushMessage.TargetInventory);
                if (targetInventory == null)
                {
                    _logger.LogError($"Target inventory '{pushMessage.TargetInventory}' not found.");
                    return false;
                }

                var itemType = (MyItemType)MyDefinitionId.Parse(pushMessage.Item);

                if (!_inventoryActions.TransferItems(CargoContainerCollection.GetCargoContainerWithItem(itemType, pushMessage.Amount).Inventory, targetInventory, itemType, pushMessage.Amount))
                {
                    _logger.LogError($"Failed to push items '{pushMessage.Item}' to '{pushMessage.TargetInventory}'.");
                    return false;
                }

                return true;
            }

            public bool TransferItems(string source, string target, MyItemType itemType, MyFixedPoint amount)
            {
                var sourceCargoContainer = CargoContainerCollection.GetCargoContainerByName(source);
                if (sourceCargoContainer == null)
                {
                    _logger.LogError($"Source cargo container '{source}' not found.");
                    return false;
                }

                var targetCargoContainer = CargoContainerCollection.GetCargoContainerByName(source);
                if (targetCargoContainer == null)
                {
                    _logger.LogError($"Target cargo container '{target}' not found.");
                    return false;
                }

                return _inventoryActions.TransferItems(sourceCargoContainer.Inventory, targetCargoContainer.Inventory, itemType, amount);
            }

            #endregion

            #region private methods

            private void ConsolidateInventories()
            {

            }

            #endregion
        }
    }
}
