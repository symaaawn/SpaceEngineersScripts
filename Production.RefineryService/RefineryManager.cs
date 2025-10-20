using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryManager
        {
            #region private fields

            private ServiceStateDc serviceState;

            private readonly Logger _logger;
            private readonly RefineryServiceConfiguration _refineryServiceConfiguration;
            private readonly RefineryActions _refineryActions;
            private readonly RefineryClient _refineryClient;

            #endregion

            #region properties

            private RefineryCollection RefineryCollection { get; set; }
            private StatusLightCollection StatusLightCollection { get; set; }

            #endregion

            #region construction

            public RefineryManager(Logger logger, RefineryServiceConfiguration refineryServiceConfiguration, RefineryActions refineryActions, RefineryClient refineryClient,
                List<IMyRefinery> refineries, List<IMyLightingBlock> statusLights)
            {
                _logger = logger;
                _refineryServiceConfiguration = refineryServiceConfiguration;
                _refineryActions = refineryActions;
                _refineryClient = refineryClient;
                RefineryCollection = new RefineryCollection(refineries);
                StatusLightCollection = new StatusLightCollection(statusLights);

                SetServiceState(ServiceStateDc.Auto);

                _logger.LogInfo("Initialized RefineryManager");
            }

            #endregion

            #region methods

            public void Update()
            {
                switch (serviceState)
                {
                    case ServiceStateDc.Error:
                        return;
                    case ServiceStateDc.Manual:
                        return;
                    case ServiceStateDc.Auto:
                        var idleRefineries = RefineryCollection.GetIdleRefineries();

                        if (idleRefineries.Count > 0)
                        {
                            _refineryClient.SendGetInventoryItems();
                        }
                        else
                        {
                            _logger.LogInfo("No idle refineries found");
                        }
                        return;
                }

            }

            public bool SetServiceState(ServiceStateDc newState)
            {
                if (serviceState != newState)
                {
                    serviceState = newState;
                    _logger.LogInfo($"Refinery service state changed to {serviceState}");
                    StatusLightCollection.UpdateLights(serviceState);

                    return true;
                }

                return false;
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

                _logger.LogInfo($"Distributing ores to {idleRefineries.Count} idle refineries");

                foreach (var idleRefinery in idleRefineries)
                {
                    _logger.LogDebug($"-> Processing idle refinery {idleRefinery.Name}");
                    var oreScores = new Dictionary<string, double>();

                    foreach (var ore in inventory)
                    {
                        var oreConsumption = OreConsumptions.GetOreConsumption(ore.Key.Split('/')[1]);

                        var scoreSpeed = oreConsumption.ConversionRatio * idleRefinery.RefineSpeed;
                        var scoreYield = (1 - oreConsumption.ConversionRatio) * idleRefinery.YieldRate;
                        var totalScore = scoreSpeed + scoreYield;

                        var plannedOreAmount = (MyFixedPoint)(oreConsumption.KgPerSecond * idleRefinery.RefineSpeed);
                        var plannedIngotAmount = plannedOreAmount * idleRefinery.YieldRate * oreConsumption.ConversionRatio;
                        var score = oreConsumption.Priority() * idleRefinery.YieldRate;
                        _logger.LogDebug($"-> Ore: {ore.Key.Split('/')[1]}, Consumption: {plannedOreAmount}kg/s, Production: {plannedIngotAmount}, ScoreSpeed: {scoreSpeed}, ScoreYield: {scoreYield}, TotalScore: {totalScore}");
                        
                        oreScores.Add(ore.Key, totalScore);
                    }
                   
                    var bestOre = oreScores.OrderByDescending(kv => kv.Value).First();
                    var oreToLoad = inventory.FirstOrDefault(o => o.Key == bestOre.Key);

                    var amountToLoad = MyFixedPoint.Min(oreToLoad.Value, (MyFixedPoint)(OreConsumptions.GetOreConsumption(oreToLoad.Key.Split('/')[1]).KgPerSecond * _refineryServiceConfiguration.RefineryBuffer));
                    _refineryClient.SendPushRequest(oreToLoad.Key, amountToLoad, idleRefinery.Name);
                }
            }

            #endregion
        }
    }
}
