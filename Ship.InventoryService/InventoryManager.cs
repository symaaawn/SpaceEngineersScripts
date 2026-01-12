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
            private readonly IMyGridTerminalSystem _gridTerminalSystem;

            #endregion

            #region properties

            private CargoContainerCollection CargoContainerCollection { get; set; }

            #endregion

            #region construction

            public InventoryManager(Logger logger, IMyGridTerminalSystem gridTerminalSystem, List<IMyCargoContainer> cargoContainers)
            {
                _logger = logger;
                _gridTerminalSystem = gridTerminalSystem;
                CargoContainerCollection = new CargoContainerCollection(logger, cargoContainers);
                _logger.LogInfo($"Initialized InventoryManager with {CargoContainerCollection.ContainerCount} cargo containers.");
            }

            #endregion

            #region methods

            public void Update()
            {
                // Update logic for inventory management:

                // ToDo: Update inventory displays

                // ToDo: Check for low/high stock and trigger transfers or consolidate inventories
            }

            public Dictionary<string, MyFixedPoint> GetInventory()
            {
                return CargoContainerCollection.GetInventoryItems();
            }

            public bool PullItems(InventoryServiceMessage_PullItems pullMessage)
            {
                var sourceInventory = GetOutputInventory(pullMessage.SourceInventory);
                if (sourceInventory == null)
                {
                    _logger.LogError($"Source inventory '{pullMessage.SourceInventory}' not found.");
                    return false;
                }

                var itemType = (MyItemType)MyDefinitionId.Parse(pullMessage.Item);

                var transferred = CargoContainerCollection.PullItems(itemType, pullMessage.Amount, sourceInventory);

                if (!transferred)
                {
                    _logger.LogError($"Failed to pull {pullMessage.Amount} of {pullMessage.Item} from {pullMessage.SourceInventory}.");
                }

                return transferred;
            }

            public bool PushItems(InventoryServiceMessage_PushItems pushMessage)
            {
                _logger.LogDebug($"Attempting to push {pushMessage.Amount} of {pushMessage.Item} to {pushMessage.TargetInventory}.");

                var targetInventory = GetInputInventory(pushMessage.TargetInventory);
                if (targetInventory == null)
                {
                    _logger.LogError($"Target inventory '{pushMessage.TargetInventory}' not found.");
                    return false;
                }

                var itemType = (MyItemType)MyDefinitionId.Parse(pushMessage.Item);

                var transferred = CargoContainerCollection.PushItems(itemType, pushMessage.Amount, targetInventory);

                _logger.LogDebug($"Push operation result: {transferred}");

                if (!transferred)
                {
                    _logger.LogError($"Failed to push {pushMessage.Amount} of {pushMessage.Item} to {pushMessage.TargetInventory}.");
                }

                return transferred;
            }

            #endregion

            #region private methods

            private IMyInventory GetOutputInventory(string productionBlockName)
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

                _logger.LogError($"Block with name {productionBlockName} is not a production block.");
                return null;
            }

            private IMyInventory GetInputInventory(string productionBlockName)
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

                _logger.LogError($"Block with name {productionBlockName} is not a production block.");
                return null;
            }

            #endregion
        }
    }
}
