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

        private const string ProgramName = "MeteorDefenseSystemControl";
        private const float PistonMovementSpeed = 0.5F; //0.01F;
        private const float BorerPistonMovementSpeed = 0.5F; //0.001F;
        private const float BorerPistonRetractionSpeed = -0.5F;

        #endregion

        #region private fields

        private readonly MyCommandLine _commandLine = new MyCommandLine();
        private readonly Logger _logger = new Logger();
        private readonly MyIni _ini = new MyIni();

        private TunnelBoringMachineModeDc _mode;
        private TunnelBoringMachineState _state;

        #endregion

        #region properties

        public List<IMyLightingBlock> StatusLights { get; private set; }
        public List<IMyShipMergeBlock> MergeBlocksBack { get; private set;}
        public List<IMyShipMergeBlock> MergeBlocksFront { get; private set; }
        public List<IMyShipConnector> ConnectorsBack { get; private set; }
        public List<IMyShipConnector> ConnectorsFront { get; private set; }
        public List<IMyExtendedPistonBase> PistonsBack { get; private set; }
        public List<IMyExtendedPistonBase> PistonsFront { get; private set; }
        public List<IMyExtendedPistonBase> PistonsBorer { get; private set; }
        public List<IMyShipDrill> Drills { get; private set; }
        public List<IMyShipWelder> Welders { get; private set; }
        public IMySensorBlock Sensor { get; private set; }
        public IMySensorBlock PeripherialSensor { get; private set; }
        public IMyMotorAdvancedStator Rotor { get; private set; }
        public List<IMyProjector> Projectors { get; private set; }


        #endregion

        public Program()
        {
            StatusLights = new List<IMyLightingBlock>();
            MergeBlocksBack = new List<IMyShipMergeBlock>();
            MergeBlocksFront = new List<IMyShipMergeBlock>();
            ConnectorsBack = new List<IMyShipConnector>();
            ConnectorsFront = new List<IMyShipConnector>();
            PistonsBack = new List<IMyExtendedPistonBase>();
            PistonsFront = new List<IMyExtendedPistonBase>();
            PistonsBorer = new List<IMyExtendedPistonBase>();
            Drills = new List<IMyShipDrill>();
            Welders = new List<IMyShipWelder>();
            Projectors = new List<IMyProjector>();

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            GridTerminalSystem.GetBlocksOfType(StatusLights, statusLight => MyIni.HasSection(statusLight.CustomData, "status light"));
            GridTerminalSystem.GetBlocksOfType(MergeBlocksBack, mergeBlocksBack => MyIni.HasSection(mergeBlocksBack.CustomData, "merge blocks back"));
            GridTerminalSystem.GetBlocksOfType(MergeBlocksFront, mergeBlocksFront => MyIni.HasSection(mergeBlocksFront.CustomData, "merge blocks front"));
            GridTerminalSystem.GetBlocksOfType(ConnectorsBack, connectorsBack => MyIni.HasSection(connectorsBack.CustomData, "connector back"));
            GridTerminalSystem.GetBlocksOfType(ConnectorsFront, connectorsFront => MyIni.HasSection(connectorsFront.CustomData, "connector front"));
            GridTerminalSystem.GetBlocksOfType(PistonsBack, pistonsBack => MyIni.HasSection(pistonsBack.CustomData, "pistons back"));
            GridTerminalSystem.GetBlocksOfType(PistonsFront, pistonsFront => MyIni.HasSection(pistonsFront.CustomData, "pistons front"));
            GridTerminalSystem.GetBlocksOfType(PistonsBorer, pistonsBorer => MyIni.HasSection(pistonsBorer.CustomData, "pistons borer"));
            GridTerminalSystem.GetBlocksOfType(Drills, drills => MyIni.HasSection(drills.CustomData, "drills"));
            GridTerminalSystem.GetBlocksOfType(Welders, welders => MyIni.HasSection(welders.CustomData, "welders"));
            GridTerminalSystem.GetBlocksOfType(Projectors, projectors => MyIni.HasSection(projectors.CustomData, "projectors"));

            var sensors = new List<IMySensorBlock>();
            GridTerminalSystem.GetBlocksOfType(sensors, sensor => MyIni.HasSection(sensor.CustomData, "front sensor")); 
            Sensor = sensors.FirstOrDefault();

            if (Sensor != null)
            {
                InitializeSensor(Sensor);
                Sensor.FrontExtend = 10;
            }

            var peripherySensors = new List<IMySensorBlock>();
            GridTerminalSystem.GetBlocksOfType(peripherySensors, sensor => MyIni.HasSection(sensor.CustomData, "peripherial sensor"));
            PeripherialSensor = peripherySensors.FirstOrDefault();

            if (PeripherialSensor != null)
            {
                InitializeSensor(PeripherialSensor);
                PeripherialSensor.BottomExtend = 7.5F;
            }

            var rotors = new List<IMyMotorAdvancedStator>();
            GridTerminalSystem.GetBlocksOfType(rotors, rotor => MyIni.HasSection(rotor.CustomData, "rotor"));
            Rotor = rotors.FirstOrDefault();

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            _mode = TunnelBoringMachineModeDc.Forward;
            _state = new TunnelBoringMachineState_ForwardExtending(this);
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if (_commandLine.TryParse(argument))
            {
                int mode;
                if(int.TryParse(_commandLine.Argument(0), out mode))
                {

                }
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                //RenderDisplay(new string[] { $"{_state.GetState()}", 
                //    $"Front: {PistonsFront.FirstOrDefault().CurrentPosition}", 
                //    $"Mining: {PistonsBorer.FirstOrDefault().CurrentPosition}",
                //    $"Back: {PistonsBack.FirstOrDefault().CurrentPosition}"});
                RunStateMachine();
                _logger.LogInfo($"{_state.GetState()}");
            }
        }

        #region state machine

        public void RunStateMachine()
        {
            _state.RunStateMachine();
        }

        public void ChangeState(TunnelBoringMachineState state)
        {
            _state = state;
            _logger.LogInfo($"Changed state to {_state.GetState()}");
        }

        #endregion

        #region helpers

        public void InitializeSensor(IMySensorBlock sensor)
        {
            sensor.DetectAsteroids = true;
            sensor.DetectPlayers = true;
            sensor.DetectFloatingObjects = false;
            sensor.DetectSmallShips = false;
            sensor.DetectLargeShips = false;
            sensor.DetectStations = false;
            sensor.DetectSubgrids = false;
            sensor.DetectOwner = true;
            sensor.DetectFriendly = false;
            sensor.DetectNeutral = false;
            sensor.DetectEnemy = false;
            sensor.FrontExtend = 0.1F;
            sensor.BackExtend = 0.1F;
            sensor.LeftExtend = 0.1F;
            sensor.RightExtend = 0.1F;
            sensor.TopExtend = 0.1F;
            sensor.BottomExtend = 0.1F;
        }

        public void StartRotor() {
            Rotor.LowerLimitDeg = float.MinValue;
            Rotor.UpperLimitDeg = float.MaxValue;
            Rotor.RotorLock = false;
            Rotor.Torque = 33500000;
            Rotor.TargetVelocityRPM = 1.5F;// 1.5F;
        }

        public void MoveForward()
        {
            PistonsFront.ForEach(piston => piston.Velocity = PistonMovementSpeed);
            PistonsBack.ForEach(piston => piston.Velocity = -PistonMovementSpeed);
        }

        public void MoveBackward()
        {
            PistonsFront.ForEach(piston => piston.Velocity = -PistonMovementSpeed);
            PistonsBack.ForEach(piston => piston.Velocity = PistonMovementSpeed);  
        }

        public void StopPistons()
        {
            PistonsFront.ForEach(piston => piston.Velocity = 0);
            PistonsBack.ForEach(piston => piston.Velocity = 0);
        }

        public void EngageBorerPistons()
        {
            PistonsBorer.ForEach(piston => piston.Velocity = BorerPistonMovementSpeed);
        }

        public void DisengageBorerPistons()
        {
            PistonsBorer.ForEach(piston => piston.Velocity = BorerPistonRetractionSpeed);
        }

        public void DrillsOn()
        {
            Drills.ForEach(drill => drill.Enabled = true);
        }

        public void DrillsOff()
        {
            Drills.ForEach(drill => drill.Enabled = true);
        }

        public void WeldersOn()
        {
            Welders.ForEach(welder => welder.Enabled = true);
        }

        public void WeldersOff()
        {
            Welders.ForEach(welder => welder.Enabled = false);
        }

        public void ProjectorsOn()
        {
            Projectors.ForEach(projector => projector.Enabled = true);
        }

        public void ProjectorsOff()
        {
            Projectors.ForEach(projector => projector.Enabled = false);
        }

        public void FrontMergeOn()
        {
            MergeBlocksFront.ForEach(mergeBlock => mergeBlock.Enabled = true);
        }

        public void FrontMergeOff()
        {
            MergeBlocksFront.ForEach(mergeBlock => mergeBlock.Enabled = false);
        }

        public void BackMergeOn()
        {
            MergeBlocksBack.ForEach(mergeBlock => mergeBlock.Enabled = true);
        }

        public void BackMergeOff()
        {
            MergeBlocksBack.ForEach(mergeBlock => mergeBlock.Enabled = false);
        }

        public void FrontConnectorsOn()
        {
            //ConnectorsFront.ForEach(connector => connector.Enabled = true);
            ConnectorsFront.ForEach(connector => connector.Connect());
        }

        public void FrontConnectorsOff()
        {
            //ConnectorsFront.ForEach(connector => connector.Enabled = false);
            ConnectorsFront.ForEach(connector => connector.Disconnect());
        }

        public void BackConnectorsOn()
        {
            //ConnectorsBack.ForEach(connector => connector.Enabled = true);
            ConnectorsBack.ForEach(connector => connector.Connect());
        }

        public void BackConnectorsOff()
        {
            //ConnectorsBack.ForEach(connector => connector.Enabled = false);
            ConnectorsBack.ForEach(connector => connector.Disconnect());
        }

        #endregion
    }
}
