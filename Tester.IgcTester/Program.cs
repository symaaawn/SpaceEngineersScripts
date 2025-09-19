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
    partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("IgcTester", "0.0.1", LogLevelDc.Debug);
        private const string ServiceConfigurationSection = "igcTester";

        #endregion

        #region private fields

        private readonly MyCommandLine _commandLine = new MyCommandLine();
        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();
        private readonly IgcTesterConfiguration _igcTesterConfiguration;

        private readonly IMyBroadcastListener _broadcastListener;

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            _igcTesterConfiguration = new IgcTesterConfiguration(Me, _ini);
            _broadcastListener = IGC.RegisterBroadcastListener(_igcTesterConfiguration.ListenerTag);
            _broadcastListener.SetMessageCallback(_igcTesterConfiguration.ListenerTag);

            _logger.LogInfo($"Listener Tag: {_igcTesterConfiguration.ListenerTag}");
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) > 0)
            {
                while (_broadcastListener.HasPendingMessage)
                {
                    MyIGCMessage myIGCMessage = _broadcastListener.AcceptMessage();
                    if (myIGCMessage.Tag == _igcTesterConfiguration.ListenerTag)
                    { // This is our tag
                        if (myIGCMessage.Data is ImmutableDictionary<string, string>)
                        {
                            var message = new RelativePositioningSystemServiceMessage().Deserialize((ImmutableDictionary<string, string>)myIGCMessage.Data);
                            if(message is RelativePositioningSystemServiceMessage_Update)
                            {
                                var updateMessage = (RelativePositioningSystemServiceMessage_Update)message;
                                _logger.LogInfo($"Received message {updateMessage.Method} with id {updateMessage.RequestId}");
                                _logger.LogInfo($"Matrix: {updateMessage.ReferenceMatrix}");
                            }
                        }
                        else // if(msg.Data is XXX)
                        {
                        }
                    }
                    else
                    {
                    }
                }
            }
        }
    }
}
