using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;
using VRageMath;

namespace IngameScript
{
    /**
     * <summary>
     * Distributes ores from the source containers to the refineries, while taking into account the refineries' speed, yield and power efficiency.
     * Ores are sorted by their consumption rate. Refineries with higher speed than yield will refine the faster ores first, while refineries with lower speed than yield will refine the slower ores first.
     * </summary>
     * <remarks>
     * Add the tag <c>oreDistributionSystem</c> to the custom data of the refineries and source cargo containers you want to use.
     * </remarks>
     */
    public partial class Program : MyGridProgram
    {        
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("OreDistributionSystem", "0.1.0", LogLevelDc.Debug);
        private const float RefineryBuffer = 5f; // seconds of ore consumption as buffer

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        #endregion

        #region properties

        public List<RefineryOverview> Refineries { get; set; } = new List<RefineryOverview>();
        public List<IMyInventory> SourceInventories { get; set; } = new List<IMyInventory>();

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            MyIniParseResult result;

            var refineries = new List<IMyRefinery>();
            GridTerminalSystem.GetBlocksOfType(refineries, refinery => MyIni.HasSection(refinery.CustomData, "oreDistributionSystem"));
            foreach (var refinery in refineries)
            {
                Refineries.Add(new RefineryOverview(refinery));
            }

            var sourceContainers = new List<IMyCargoContainer>();
            GridTerminalSystem.GetBlocksOfType(sourceContainers, container => MyIni.HasSection(container.CustomData, "oreDistributionSystem"));
            foreach (var sourceContainer in sourceContainers)
            {
                if (sourceContainer.HasInventory)
                {
                    var inventory = sourceContainer.GetInventory(0);
                    SourceInventories.Add(inventory);
                    _logger.LogDebug($"Found source container {sourceContainer.CustomName}");
                }
            }

            _logger.LogDebug($"Found {Refineries.Count} refineries");
            foreach (var refinery in Refineries)
            {
                _logger.LogDebug($"{refinery.Refinery.CustomName}: Speed {refinery.RefineSpeed} / Yield {refinery.YieldRate} / Efficiency {refinery.PowerEfficiency}");
            }

            var statusLights = new List<IMyLightingBlock>();
            GridTerminalSystem.GetBlocksOfType(statusLights, light => MyIni.HasSection(light.CustomData, "oreDistributionSystem"));
            foreach (var statusLight in statusLights)
            {
                if (!_ini.TryParse(statusLight.CustomData, out result))
                    throw new Exception(result.ToString());

                var id = _ini.Get("oreDistributionSystem", "machineName").ToString();
                if (string.IsNullOrEmpty(id))
                    continue;
                _logger.LogDebug($"Found status light {statusLight.CustomName} for {id}");

                Refineries.FirstOrDefault(r => r.Refinery.CustomName.Contains(id))?.StatusLights.Add(statusLight);
            }
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                var ores = new List<InventoryItem>();
                foreach (var sourceInventory in SourceInventories)
                {
                    var items = new List<MyInventoryItem>();
                    sourceInventory.GetItems(items);
                    foreach (var item in items)
                    {
                        ores.Add(new InventoryItem(item, sourceInventory));
                    }
                }
                ores = ores.OrderByDescending(o => OreConsumptions.GetOreConsumption(o.SubType).KgPerSecond).ToList();

                foreach (var refinery in Refineries)
                {
                    if (ores.Count > 0)
                    {
                        var refineryOres = new List<MyInventoryItem>();
                        refinery.Refinery.InputInventory.GetItems(refineryOres);
                        if (refineryOres.All(o => o.Amount < (MyFixedPoint)(OreConsumptions.GetOreConsumption(o.Type.SubtypeId).KgPerSecond * RefineryBuffer)))
                        {
                            if (refinery.YieldRate < refinery.RefineSpeed)
                            {
                                var ore = ores.FirstOrDefault();
                                _logger.LogDebug($"Transferring {ore.Item.Type.SubtypeId} to {refinery.Refinery.CustomName}");
                                refinery.Refinery.InputInventory.TransferItemFrom(ore.Inventory, ore.Item, (MyFixedPoint)(OreConsumptions.GetOreConsumption(ore.SubType).KgPerSecond * RefineryBuffer));
                            }
                            else
                            {
                                var ore = ores.LastOrDefault();
                                _logger.LogDebug($"Transferring {ore.Item.Type.SubtypeId} to {refinery.Refinery.CustomName}");
                                refinery.Refinery.InputInventory.TransferItemFrom(ore.Inventory, ore.Item, (MyFixedPoint)(OreConsumptions.GetOreConsumption(ore.SubType).KgPerSecond * RefineryBuffer));
                            }
                        }
                    }

                    if (refinery.Refinery.IsProducing)
                    {
                        foreach (var statusLight in refinery.StatusLights)
                        {
                            statusLight.Color = Color.Green;
                            statusLight.Enabled = true;
                        }
                    }
                    else
                    {
                        if (refinery.Refinery.IsWorking)
                        {
                            foreach (var statusLight in refinery.StatusLights)
                            {
                                statusLight.Color = Color.Yellow;
                                statusLight.Enabled = true;
                            }
                        }
                        else
                        {
                            foreach (var statusLight in refinery.StatusLights)
                            {
                                statusLight.Color = Color.Red;
                                statusLight.Enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
