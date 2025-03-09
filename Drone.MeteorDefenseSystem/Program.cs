using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    partial class Program : MyGridProgram
    {
        #region private fields

        private readonly MyCommandLine _commandLine = new MyCommandLine();
        private readonly Logger _logger = new Logger();
        private readonly MyIni _ini = new MyIni();

        IMyBroadcastListener _broadcastListenerRps;
        IMyBroadcastListener _broadcastListenerStateControl;

        private MeteorDefenseSystemState _state;

        private string _id;

        private MatrixD _homePosition = new MatrixD();
        private int _angle = 0;

        private bool _registered = false;

        #endregion

        #region properties

        public List<IMyLightingBlock> StatusLights { get; private set; }
        public List<IMyLargeTurretBase> Turrets { get; private set; }
        public List<IMyBatteryBlock> BatteryBlocks { get; private set; }
        public List<IMyCargoContainer> AmmoContainers { get; private set; }
        public float MaxStoredPower { get; private set; }
        public IMyRemoteControl ReferenceControl { get; private set; }

        #endregion


        public Program()
        {
            StatusLights = new List<IMyLightingBlock>();
            Turrets = new List<IMyLargeTurretBase>();
            BatteryBlocks = new List<IMyBatteryBlock>();
            AmmoContainers = new List<IMyCargoContainer>();

            InitializeDisplay("MeteorDefenseSystem");
            _logger.AddLogger(new DetailAreaLogger(Echo));

            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            _id = _ini.Get("config", "id").ToString();

            GridTerminalSystem.GetBlocksOfType(StatusLights, statusLight => MyIni.HasSection(statusLight.CustomData, "status light"));
            GridTerminalSystem.GetBlocksOfType(Turrets, turret => MyIni.HasSection(turret.CustomData, "turret"));
            GridTerminalSystem.GetBlocksOfType(BatteryBlocks, batteryBlock => MyIni.HasSection(batteryBlock.CustomData, "battery block"));
            BatteryBlocks.ForEach(b => MaxStoredPower += b.MaxStoredPower);
            var referenceControls = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType(referenceControls, referenceControl => MyIni.HasSection(referenceControl.CustomData, "reference"));
            ReferenceControl = referenceControls.FirstOrDefault();
            ReferenceControl.FlightMode = FlightMode.OneWay;

            foreach (var turret in Turrets)
            {
                turret.TargetCharacters = false;
                turret.TargetEnemies = false;
                turret.TargetLargeGrids = false;
                turret.TargetMissiles = false;
                turret.TargetNeutrals = false;
                turret.TargetSmallGrids = false;
                turret.TargetStations = false;

                turret.TargetMeteors = true;
            }

            _broadcastListenerRps = IGC.RegisterBroadcastListener(IgcTagDc.RpsMessage);
            _broadcastListenerStateControl = IGC.RegisterBroadcastListener(IgcTagDc.MeteorDefenseSystemMessage);
            _broadcastListenerRps.SetMessageCallback(IgcTagDc.RpsMessage);
            _broadcastListenerStateControl.SetMessageCallback(IgcTagDc.MeteorDefenseSystemMessage);

            _ini.TryParse(Storage);
            _angle = _ini.Get("saveData", "angle").ToInt32(0);
            switch (_ini.Get("saveData", "state").ToInt32(0))
            {
                case 0:
                    _state = new MeteorDefenseSystemState_Idle(this);
                    break;
                case 1:
                    _state = new MeteorDefenseSystemState_Active(this);
                    break;
                case 2:
                    _state = new MeteorDefenseSystemState_Fallback(this);
                    break;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            RenderDisplay(new string[] { _state.GetState().ToString() });
            _logger.LogInfo($"Program initialized, id: {_id}");
        }

        public void Save()
        {
            _ini.Clear();
            _ini.Set("saveData", "state", (int)_state.GetState());
            _ini.Set("saveData", "angle", _angle);

            Storage = _ini.ToString();
        }

        public void Main(string argument, UpdateType updateType)
        {
            if (!_registered)
            {
                IGC.SendBroadcastMessage(IgcTagDc.MeteorDefenseSystemRegister, _id);
                _registered = true;
            }

            if (_commandLine.TryParse(argument))
            {
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                RenderDisplay(new string[] { $"{_state.GetState()}", $"X: {_homePosition.GetRow(3).X}", $"Y: {_homePosition.GetRow(3).Y}", $"Z: {_homePosition.GetRow(3).Z}" });
                RunStateMachine();
                _logger.LogInfo($"{_state.GetState()} {_angle}");
            }

            if ((updateType & UpdateType.IGC) != 0)
            {
                while (_broadcastListenerRps.HasPendingMessage)
                {
                    var igcMessage = _broadcastListenerRps.AcceptMessage();

                    try
                    {
                        _homePosition = (MatrixD) igcMessage.Data;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }

                while (_broadcastListenerStateControl.HasPendingMessage)
                {
                    var igcMessage = _broadcastListenerStateControl.AcceptMessage();

                    switch ((MeteorDefenseSystemStateDc)igcMessage.Data)
                    {
                        case MeteorDefenseSystemStateDc.Idle:
                            ChangeState(new MeteorDefenseSystemState_Idle(this));
                            break;
                        case MeteorDefenseSystemStateDc.Active:
                            ChangeState(new MeteorDefenseSystemState_Active(this));
                            break;
                        case MeteorDefenseSystemStateDc.Fallback:
                        default:
                            ChangeState(new MeteorDefenseSystemState_Fallback(this));
                            break;
                    }
                }
            }
        }

        #region state machine

        public void RunStateMachine()
        {
            _state.RunStateMachine();
        }

        public void ChangeState(MeteorDefenseSystemState state)
        {
            _state = state;
            _logger.LogInfo($"Changed state to {_state.GetState()}");
        }

        #endregion

        #region movement

        public void MoveToRelativePosition(string name, Vector3D position, bool forced = false)
        {
            if (forced) { 
                ReferenceControl.ClearWaypoints();
            }

            var targetPosition = Vector3D.Transform(position, _homePosition);
            ReferenceControl.AddWaypoint(new MyWaypointInfo(name, targetPosition));
        }

        public Vector3D CalculateCircleCoordinates(int angle)
        {
            var radians = (Math.PI / 180) * angle;
            return new Vector3D(100 * Math.Sin(radians), 100 * Math.Cos(radians), 0);
        }

        #endregion
    }
}
