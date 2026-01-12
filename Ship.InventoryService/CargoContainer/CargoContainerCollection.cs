using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program 
    {
        public class CargoContainerCollection
        {
            #region private fields

            private readonly Logger _logger;
            private List<CargoContainer> _cargoContainers = new List<CargoContainer>();

            private List<MyInventoryItem> _tempMyInventoryItems = new List<MyInventoryItem>();
            private Dictionary<string, MyFixedPoint> _itemCache = new Dictionary<string, MyFixedPoint>();

            #endregion

            #region construction

            public CargoContainerCollection(Logger logger, List<IMyCargoContainer> cargoContainers)
            {
                _logger = logger;

                foreach (var cargoContainer in cargoContainers)
                {
                    _cargoContainers.Add(new CargoContainer(cargoContainer));
                }
                _cargoContainers = _cargoContainers.OrderBy(c => c.FillPriority).ToList();
            }

            #endregion

            #region methods

            public int ContainerCount => _cargoContainers.Count;

            public Dictionary<string, MyFixedPoint> GetInventoryItems()
            {
                _itemCache.Clear();

                foreach (var cargoContainer in _cargoContainers)
                {
                    _tempMyInventoryItems.Clear();

                    cargoContainer.Inventory.GetItems(_tempMyInventoryItems);
                    foreach (var item in _tempMyInventoryItems)
                    {
                        if (_itemCache.ContainsKey(item.Type.ToString()))
                        {
                            _itemCache[item.Type.ToString()] += item.Amount;
                        }
                        else
                        {
                            _itemCache[item.Type.ToString()] = item.Amount;
                        }
                    }
                }

                return _itemCache;
            }

            public bool PullItems(MyItemType itemType, MyFixedPoint amount, IMyInventory sourceInventory)
            {
                var inventoryItem = sourceInventory.FindItem(itemType);
                if (!inventoryItem.HasValue || inventoryItem.Value.Amount == 0)
                {
                    return false;
                }

                CargoContainerTypeDc containerType = CargoContainerTypeDc.Uncategorized;
                switch (itemType.TypeId)
                {
                    case ItemType.Ore:
                        containerType = CargoContainerTypeDc.Ores;
                        break;

                    case ItemType.Ingot:
                        containerType = CargoContainerTypeDc.Ingots;
                        break;

                    case ItemType.Component:
                        containerType = CargoContainerTypeDc.Components;
                        break;

                    case ItemType.Gas:
                        containerType = CargoContainerTypeDc.Gas;
                        break;

                    case ItemType.Tool:
                        containerType = CargoContainerTypeDc.Tools;
                        break;
                }

                return _cargoContainers.FirstOrDefault(c => c.CanItemsBeAdded(amount, itemType) && c.CargoContainerType == containerType).Inventory.TransferItemFrom(sourceInventory, inventoryItem.Value, amount);
            }

            public bool PushItems(MyItemType itemType, MyFixedPoint amount, IMyInventory targetInventory)
            {
                foreach (var cargoContainer in _cargoContainers)
                {
                    var inventoryItem = cargoContainer.Inventory.FindItem(itemType);
                    if (!inventoryItem.HasValue|| inventoryItem.Value.Amount == 0)
                    {
                        continue;
                    }

                    var amountToTransfer = MyFixedPoint.Min(inventoryItem.Value.Amount, amount);
                    var result = cargoContainer.Inventory.TransferItemTo(targetInventory, inventoryItem.Value, amountToTransfer);
                    if (!result)
                    {
                        continue;
                    }

                    amount -= amountToTransfer;
                    if (amount <= 0)
                    {
                        break;
                    }
                }

                return amount <= 0;
            }

            #endregion
        }
    }
}
