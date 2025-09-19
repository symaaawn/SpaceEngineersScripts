using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
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

            private List<IMyCargoContainer> CargoContainers { get; set; }

            #endregion

            #region construction

            public InventoryManager(Logger logger, InventoryActions inventoryActions, List<IMyCargoContainer> cargoContainers)
            {
                _logger = logger;
                _inventoryActions = inventoryActions;
                CargoContainers = cargoContainers;
            }

            #endregion

            #region methods

            public void Update()
            {
                // Update logic for inventory management:

                // ToDo: Update inventory displays

                // ToDo: Check for low/high stock and trigger transfers or consolidate inventories
            }

            public bool TransferItems(string source, string target, MyItemType itemType, MyFixedPoint amount)
            {
                var sourceCargoContainer = CargoContainers.FirstOrDefault(c => c.CustomName.Equals(source));
                if (sourceCargoContainer == null)
                {
                    _logger.LogError($"Source cargo container '{source}' not found.");
                    return false;
                }

                var targetCargoContainer = CargoContainers.FirstOrDefault(c => c.CustomName.Equals(target));
                if (targetCargoContainer == null)
                {
                    _logger.LogError($"Target cargo container '{target}' not found.");
                    return false;
                }

                return _inventoryActions.TransferItems(sourceCargoContainer.GetInventory(), targetCargoContainer.GetInventory(), itemType, amount);
            }

            #endregion
        }
    }
}
