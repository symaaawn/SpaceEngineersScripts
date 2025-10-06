using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryManager
        {
            #region private fields

            private readonly Logger _logger;
            private readonly RefineryActions _refineryActions;
            private readonly RefineryClient _refineryClient;

            #endregion

            #region properties

            private RefineryCollection RefineryCollection { get; set; }

            #endregion

            #region construction

            public RefineryManager(Logger logger, RefineryActions refineryActions, RefineryClient refineryClient, List<IMyRefinery> refineries)
            {
                _logger = logger;
                _refineryActions = refineryActions;
                _refineryClient = refineryClient;
                RefineryCollection = new RefineryCollection(refineries);

                _logger.LogInfo("Initialized RefineryManager");
            }

            #endregion

            #region methods

            public void Update()
            {
                var idleRefineries = RefineryCollection.GetIdleRefineries();

                if (idleRefineries.Count > 0)
                {
                    _refineryClient.SendGetInventoryItems();
                }
                else
                {
                    _logger.LogInfo("No idle refineries found");
                }
            }

            public void ProcessInventoryResponse()
            {
                _logger.LogDebug($"Processing inventory responses");

                var inventoryResponses = _refineryClient.GetPendingMessages();
                
                foreach (var inventoryResponse in inventoryResponses)
                {
                    if(inventoryResponse.Method == "GetInventory")
                    {
                        var getInventoryResponse = inventoryResponse as InventoryServiceMessage_GetInventory;
                        DistributeOres(getInventoryResponse.Inventory);
                    }
                }
            }

            public void DistributeOres(Dictionary<string, MyFixedPoint> inventory)
            {
                if (inventory == null || inventory.Count == 0)
                {
                    _logger.LogWarning("No inventory to distribute");
                    return;
                }

                var idleRefineries = RefineryCollection.GetIdleRefineries();

                foreach (var item in inventory)
                {
                    _logger.LogDebug($" -> Inventory item: {item.Key} = {item.Value}");
                }

                _logger.LogInfo($"Distributing ores to {idleRefineries.Count} idle refineries");

                foreach (var idleRefinery in idleRefineries)
                {
                    _logger.LogDebug($" -> Processing idle refinery {idleRefinery.Name}");
                    var oreToLoad = inventory.FirstOrDefault(i => i.Value > 0);
                    _logger.LogDebug($"{oreToLoad.Key}");
                    var amountToLoad = MyFixedPoint.Min(oreToLoad.Value, (MyFixedPoint)(OreConsumptions.GetOreConsumption(oreToLoad.Key.Split('/')[1]).KgPerSecond * 5f));
                    _refineryClient.SendPushRequest(oreToLoad.Key, amountToLoad, idleRefinery.Name);
                }
            }

            #endregion
        }
    }
}
