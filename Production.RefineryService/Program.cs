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
    public partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("RefineryService", "0.1.0", LogLevelDc.Debug);
        private const string RefineryServiceTag = "RefineryService";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        private readonly RefineryServiceConfiguration _refineryServiceConfiguration;

        private readonly RefineryManager _refineryManager;
        private readonly RefineryActions _refineryActions;
        private readonly RefineryController _refineryController;
        private readonly RefineryClient _refineryClient;

        #endregion

        #region properties

        #endregion

        public Program()
        {
            _refineryServiceConfiguration = new RefineryServiceConfiguration(Me, _ini);

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            _refineryClient = new RefineryClient(_logger, _refineryServiceConfiguration, IGC);
            _refineryActions = new RefineryActions(_logger, GridTerminalSystem);
            _refineryManager = new RefineryManager(_logger, _refineryActions, _refineryClient);
            _refineryController = new RefineryController(_logger, _refineryManager, IGC);
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from. Be aware that the
            // updateSource is a  bitfield  and might contain more than 
            // one update type.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.
        }
    }
}
