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
        private const float RefineryBuffer = 25f; // seconds of ore consumption as buffer

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        #endregion

        #region properties

        public List<RefineryOverview> Refineries { get; set; } = new List<RefineryOverview>();
        public List<IMyAssembler> Assemblers { get; set; } = new List<IMyAssembler>();

        public List<IMyInventory> SourceInventories { get; set; } = new List<IMyInventory>();
        public List<IMyInventory> TargetInventories { get; set; } = new List<IMyInventory>();

        public List<IMyLightingBlock> SystemStatusLights { get; set; } = new List<IMyLightingBlock> { };

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            MyIniParseResult result;

            var refineries = new List<IMyRefinery>();
            GridTerminalSystem.GetBlocksOfType(refineries, refinery => MyIni.HasSection(refinery.CustomData, OreDistributionSystem));
            foreach (var refinery in refineries)
            {
                Refineries.Add(new RefineryOverview(refinery));
            }

            var assemblers = new List<IMyAssembler>();
            GridTerminalSystem.GetBlocksOfType(assemblers, assembler => MyIni.HasSection(assembler.CustomData, OreDistributionSystem));
            foreach (var assembler in assemblers)
            {
                Assemblers.Add(assembler);
            }
            _logger.LogDebug($"Found {Assemblers.Count} assemblers");

            var containers = new List<IMyCargoContainer>();
            GridTerminalSystem.GetBlocksOfType(containers, container => MyIni.HasSection(container.CustomData, OreDistributionSystem));
            foreach (var container in containers)
            {
                if (container.HasInventory)
                {
                    if (!_ini.TryParse(container.CustomData, out result))
                        throw new Exception(result.ToString());

                    var containerType = _ini.Get(OreDistributionSystem, "containerType").ToInt32();
                    switch (containerType)
                    {
                        case (int)ContainerTypeDc.Ores:
                            var inventory = container.GetInventory(0);
                            SourceInventories.Add(inventory);
                            _logger.LogDebug($"Found target container {container.CustomName}");
                            break;

                        case (int)ContainerTypeDc.Ingots:
                            var targetInventory = container.GetInventory(0);
                            TargetInventories.Add(targetInventory);
                            _logger.LogDebug($"Found target container {container.CustomName}");
                            break;

                        default:
                            _logger.LogWarning($"Unknown container type for {container.CustomName}");
                            continue;
                    }

                    
                }
            }

            _logger.LogDebug($"Found {Refineries.Count} refineries");
            foreach (var refinery in Refineries)
            {
                _logger.LogDebug($"{refinery.Refinery.CustomName}: Speed {refinery.RefineSpeed} / Yield {refinery.YieldRate} / Efficiency {refinery.PowerEfficiency}");
            }

            var statusLights = new List<IMyLightingBlock>();
            GridTerminalSystem.GetBlocksOfType(statusLights, light => MyIni.HasSection(light.CustomData, OreDistributionSystem));
            foreach (var statusLight in statusLights)
            {
                if (!_ini.TryParse(statusLight.CustomData, out result))
                    throw new Exception(result.ToString());

                var machineName = _ini.Get(OreDistributionSystem, "machineName").ToString();
                if (string.IsNullOrEmpty(machineName))
                {
                    SystemStatusLights.Add(statusLight);
                    continue;
                }
                _logger.LogDebug($"Found status light {statusLight.CustomName} for {machineName}");

                Refineries.FirstOrDefault(r => r.Refinery.CustomName.Contains(machineName))?.StatusLights.Add(statusLight);
            }
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                var ingots = new List<MyInventoryItem>();
                foreach (var targetInventory in TargetInventories)
                {
                    var items = new List<MyInventoryItem>();
                    targetInventory.GetItems(items);
                    ingots.AddRange(items);
                }

                var rushOres = new List<string>();
                foreach (var assembler in Assemblers)
                {
                    var assemblerQueue = new List<MyProductionItem>();
                    assembler.GetQueue(assemblerQueue);
                    foreach (var item in assemblerQueue)
                    {
                        var recipe = GetRecipe(item.BlueprintId.SubtypeName);
                        foreach (var ingotNeed in recipe.GetMaterials())
                        {
                            var amountNeeded = ingotNeed.Value * ((double)item.Amount);
                            var ingot = ingots.FirstOrDefault(i => i.Type.SubtypeId == ingotNeed.Key);
                            if (ingot == null || (ingot != null && (double)ingot.Amount < amountNeeded))
                            {
                                rushOres.Add(ingotNeed.Key);
                                _logger.LogDebug($"Assembler {assembler.CustomName} needs {amountNeeded} of {ingotNeed.Key} but only has {ingot.Amount}");
                            }
                        }
                    }
                }

                var ores = new List<InventoryItem>();
                foreach (var sourceInventory in SourceInventories)
                {
                    var items = new List<MyInventoryItem>();
                    sourceInventory.GetItems(items);
                    foreach (var item in items.Where(i => !i.Type.SubtypeId.Equals("Ice")))
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
                                InventoryItem ore = ores.FirstOrDefault(o => o.SubType == rushOres.FirstOrDefault()) ?? ores.FirstOrDefault();
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
                }

                SetStatusLights();

                SetSystemStatusLights();
            }
        }

        public void SetStatusLights()
        {
            foreach (var refinery in Refineries){
                if (refinery.Refinery.IsProducing)
                {
                    foreach (var statusLight in refinery.StatusLights)
                    {
                        statusLight.Color = Color.Green;
                        statusLight.Enabled = true;
                    }
                }
                else if (refinery.Refinery.IsWorking)
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

        public void SetSystemStatusLights()
        {
            if (Refineries.All(r => r.Refinery.IsProducing))
            {
                foreach (var statusLight in SystemStatusLights)
                {
                    statusLight.Color = Color.Green;
                    statusLight.Enabled = true;
                }
            }
            else if (Refineries.All(r => r.Refinery.IsWorking))
            {
                foreach (var statusLight in SystemStatusLights)
                {
                    statusLight.Color = Color.Yellow;
                    statusLight.Enabled = true;
                }
            }
            else
            {
                foreach (var statusLight in SystemStatusLights)
                {
                    statusLight.Color = Color.Red;
                    statusLight.Enabled = true;
                }
            }
        }
    }
}
