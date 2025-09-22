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
using VRageMath;

namespace IngameScript
{
    /**
     * <summary>
     * Service that manages inventories of cargo containers on the same construct.
     * </summary>
     * <remarks>
     * Add the tag <c>inventoryService</c> to the custom data of the cargo containers you want to use.
     * </remarks>
     */
    public partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("InventoryService", "0.1.0", LogLevelDc.Debug);
        private const string InventoryServiceTag = "InventoryService";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        private readonly InventoryManager _inventoryManager;

        private readonly InventoryActions _inventoryActions;

        private readonly InventoryController _inventoryController;

        #endregion

        #region properties

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            var cargoContainers = new List<IMyCargoContainer>();
            GridTerminalSystem.GetBlocksOfType(cargoContainers, container => MyIni.HasSection(container.CustomData, InventoryServiceTag));

            _inventoryActions = new InventoryActions(_logger, GridTerminalSystem);

            _inventoryManager = new InventoryManager(_logger, _inventoryActions, cargoContainers);

            _inventoryController = new InventoryController(_logger, _inventoryManager, IGC);
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) != 0)
            {
                _inventoryController.Update();
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                _inventoryManager.Update();
            }
        }
    }
}
