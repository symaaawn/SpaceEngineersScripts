using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
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
using UpdateType = Sandbox.ModAPI.Ingame.UpdateType;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("PowerOverview", "0.0.1", LogLevelDc.Debug);
        private const int DisplayWidth = 30;

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        #endregion

        #region properties

        public List<PowerOverviewObserver> PowerOverviewObservers { get; private set; } = new List<PowerOverviewObserver>();

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));
            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            var powerProducers = new List<IMyPowerProducer>();
            GridTerminalSystem.GetBlocksOfType(powerProducers, powerProducer => powerProducer.IsSameConstructAs(Me));

            var displays = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(displays,
                display => MyIni.HasSection(display.CustomData, "energyOverview"));

            PowerOverviewObservers.Add(new PowerOverviewObserver(powerProducers, displays));

            _logger.LogInfo($"Found {powerProducers.Count} power producers and {displays.Count} displays");
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                foreach (var observer in PowerOverviewObservers)
                {
                    observer.UpdateDisplays();
                }
            }
        }

        public class PowerOverviewObserver
        {
            #region properties

            public List<IMyPowerProducer> PowerProducers { get; private set; }
            public List<DisplayPowerOverview> Displays { get; private set; } = new List<DisplayPowerOverview>();

            #endregion

            #region construction

            public PowerOverviewObserver(List<IMyPowerProducer> powerProducers, List<IMyTextPanel> displays)
            {
                PowerProducers = powerProducers;
                foreach (var display in displays)
                {
                    Displays.Add(new DisplayPowerOverview(display));
                }
            }

            #endregion

            public void UpdateDisplays()
            {
                var powerOverviews = new List<PowerOverviewDc>();
                foreach (var powerProducer in PowerProducers)
                {
                    PowerOverviewDc powerOverview;
                    if (powerProducer is IMyBatteryBlock)
                    {
                        var batteryBlock = (IMyBatteryBlock)powerProducer;
                        powerOverview = new PowerOverviewDc(batteryBlock.CustomName, PowerProducerType.Battery, batteryBlock.MaxOutput, batteryBlock.CurrentOutput, batteryBlock.MaxStoredPower, batteryBlock.CurrentStoredPower);
                    }
                    else if (powerProducer is IMyReactor) {
                        var reactor = (IMyReactor)powerProducer;
                        powerOverview = new PowerOverviewDc(reactor.CustomName, PowerProducerType.Reactor, reactor.MaxOutput, reactor.CurrentOutput);
                    }
                    else if (powerProducer is IMySolarPanel)
                    {
                        var solarPanel = (IMySolarPanel)powerProducer;
                        powerOverview = new PowerOverviewDc(solarPanel.CustomName, PowerProducerType.SolarPanel, solarPanel.MaxOutput, solarPanel.CurrentOutput);
                    }
                    else if (powerProducer is IMyWindTurbine)
                    {
                        var windTurbine = (IMyWindTurbine)powerProducer;
                        powerOverview = new PowerOverviewDc(windTurbine.CustomName, PowerProducerType.Battery, windTurbine.MaxOutput, windTurbine.CurrentOutput);
                    }
                    else
                    {
                        powerOverview = new PowerOverviewDc(powerProducer.CustomName, "Unknown", powerProducer.MaxOutput, powerProducer.CurrentOutput);
                    }
                    powerOverviews.Add(powerOverview);
                }

                foreach (var display in Displays)
                {
                    display.RenderDisplay(powerOverviews);
                }
            }
        }
    }
}
