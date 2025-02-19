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
    partial class Program : MyGridProgram
    {
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
            RegisteredDrones = new List<MyTuple<string, long>>();
            _logger.AddLogger(new DetailAreaLogger(Echo));

            _broadcastListenerRegister = IGC.RegisterBroadcastListener(IgcTagDc.MeteorDefenseSystemRegister);
            _broadcastListenerRegister.SetMessageCallback(IgcTagDc.MeteorDefenseSystemRegister);

            InitializeDisplay("MeteorDefenseSystemControl");
        }

        public void Save()
        {
        }

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
                }
            }

            RenderDisplay(ParseDroneArray());
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
