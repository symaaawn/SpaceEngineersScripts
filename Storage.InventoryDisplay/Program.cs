﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sandbox.Game;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using UpdateType = Sandbox.ModAPI.Ingame.UpdateType;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region constants

        private const string ProgramName = "InventoryDisplay";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger();
        private readonly MyIni _ini = new MyIni();

        #endregion

        #region properties

        public List<CargoContainerObserver> CargoContainerObservers { get; private set; } = new List<CargoContainerObserver>();

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            var displays = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(displays,
                display => MyIni.HasSection(display.CustomData, "inventoryDisplay"));

            var cargoContainers = new List<IMyCargoContainer>();
            GridTerminalSystem.GetBlocksOfType(cargoContainers,
                cargoContainer => MyIni.HasSection(cargoContainer.CustomData, "inventoryDisplay"));

            var cargoContainerIdDictionary = new Dictionary<string, IMyCargoContainer>();

            foreach (var cargoContainer in cargoContainers)
            {
                MyIniParseResult result;
                if (!_ini.TryParse(cargoContainer.CustomData, out result))
                    throw new Exception(result.ToString());

                var id = _ini.Get("inventoryDisplay", "inventoryId").ToString();
                if (string.IsNullOrEmpty(id))
                    continue;

                var observingDisplays = new List<IMyTextPanel>();
                foreach (var display in displays)
                {
                    if (display.CustomData.Contains(id))
                    {
                        observingDisplays.Add(display);
                    }
                }

                if (observingDisplays.Count == 0)
                    continue;

                CargoContainerObservers.Add(new CargoContainerObserver(this, cargoContainer, observingDisplays));
            }
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                foreach (var display in CargoContainerObservers)
                {
                    display.UpdateDisplays();
                }
            }
        }

        private static List<string> ContainedItemString(List<MyInventoryItem> items)
        {
            var itemList = new List<string>();
            var itemsSorted = items.OrderBy(i => i == null).ThenBy(i => i.Type.ToString()).ToList();
            foreach (var item in itemsSorted)
            {
                var amount = item.Amount.RawValue / 1000000f;
                var amountString = amount < 1000 ? $"{amount:N3} " : $"{amount / 1000:N3}K";
                var typeString = StringHelpers.Truncate(item.Type.SubtypeId.ToString(), 15);
                itemList.Add($"{typeString,-15} {amountString,10}");
            }

            return itemList;
        }

        public class CargoContainerObserver
        {
            #region private fields

            private readonly Program _program;
            private readonly MyIni _ini = new MyIni();

            #endregion

            #region properties

            public string Id { get; private set; }
            public IMyCargoContainer Container { get; private set; }
            public List<StorageInventoryDisplay> Displays { get; private set; } = new List<StorageInventoryDisplay>();

            #endregion

            #region construction

            public CargoContainerObserver(Program program, IMyCargoContainer container, List<IMyTextPanel> displays)
            {
                _program = program;
                Container = container;
                Id = _ini.Get("inventoryDisplay", "inventoryId").ToString();
                Displays.AddList(displays.Select(d => new StorageInventoryDisplay(d)).ToList());
                _program._logger.LogInfo($"Found {displays.Count} displays for cargo container {container.CustomName}");
            }

            #endregion

            public void UpdateDisplays()
            {
                var items = new List<MyInventoryItem>();
                var inventory = Container.GetInventory();
                inventory.GetItems(items);
                var inventoryCapacity = $"{inventory.CurrentVolume.ToString():N3} / {inventory.MaxVolume.ToString():N3}";
                var itemList = new List<string>
                {
                    $"Inventory: {inventoryCapacity}"
                };

                itemList.AddRange(ContainedItemString(items));
                foreach (var display in Displays)
                {
                    display.RenderDisplay(inventory.CurrentVolume.ToIntSafe(), inventory.MaxVolume.ToIntSafe(), itemList.ToArray());
                }
            }
        }
    }
}