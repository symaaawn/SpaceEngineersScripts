using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
     * Meteor Defense System Control
     * </summary>
     * <remarks>
     * This script is responsible for controlling the meteor defense system drones.
     * Calling it with a parameter will change the state of all registered drones.
     * States are described in <c>MeteorDefenseSystemStateDc</c>
     * </remarks>
     */
    partial class Program : MyGridProgram
    {
        #region constants

        private const string ProgramName = "MeteorDefenseSystemControl";

        #endregion

        #region private fields

        private readonly MyCommandLine _commandLine = new MyCommandLine();
        private readonly Logger _logger = new Logger();
        private readonly MyIni _ini = new MyIni();

        IMyBroadcastListener _broadcastListenerRegister;

        #endregion

        #region properties

        public List<MyTuple<string, long>> RegisteredDrones { get; private set; }

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            RegisteredDrones = new List<MyTuple<string, long>>();

            _broadcastListenerRegister = IGC.RegisterBroadcastListener(IgcTagDc.MeteorDefenseSystemRegister);
            _broadcastListenerRegister.SetMessageCallback(IgcTagDc.MeteorDefenseSystemRegister);
        }

        public void Save()
        {
        }

        /**
         * <summary>
         * Main entry point
         * If called with a parameter, the drones will change their state
         * If called with an IGC message from a drone, the drone will register itself
         * </summary>
         * <param name="argument">First argument is the state the drones should change to as int</param>
         * <param name="updateType">The type of update</param>
         */
        public void Main(string argument, UpdateType updateType)
        {
            if (_commandLine.TryParse(argument))
            {
                int newState;
                if (int.TryParse(_commandLine.Argument(0), out newState))
                {
                    IGC.SendBroadcastMessage(IgcTagDc.MeteorDefenseSystemMessage, newState);
                }
            }

            if ((updateType & UpdateType.IGC) != 0)
            {
                while (_broadcastListenerRegister.HasPendingMessage)
                {
                    var igcMessage = _broadcastListenerRegister.AcceptMessage();

                    foreach(var drone in RegisteredDrones)
                    {

                    }

                    RegisteredDrones.Add(new MyTuple<string, long>(igcMessage.Data.ToString(), igcMessage.Source));
                    RegisteredDrones.Distinct();
                    _logger.LogInfo($"Registered drone {igcMessage.Data} with id {igcMessage.Source}");
                }
            }

            //RenderDisplay(ParseDroneArray());
        }

        #region helpers

        public string[] ParseDroneArray()
        {
            var registeredDroneList = new List<string>();

            foreach (var drone in RegisteredDrones)
            {
                registeredDroneList.Add($"{drone.Item1}");
            }

            return registeredDroneList.ToArray();
        }

        #endregion
    }
}
