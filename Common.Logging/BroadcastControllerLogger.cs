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
    partial class Program
    {
        /**
         * <summary>
         * Logger that uses a chat broadcast controller to send log messages.
         * Implements <c>ILogger</c>.
         * </summary>
         */
        internal class BroadcastControllerLogger : ILogger
        {
            #region private fields

            private readonly IMyChatBroadcastControllerComponent _chatBroadcastController;

            #endregion

            #region construction

            public BroadcastControllerLogger(Program program)
            {
                var broadcastControllers = new List<IMyBroadcastController>();
                program.GridTerminalSystem.GetBlocksOfType(broadcastControllers);
                broadcastControllers.FirstOrDefault(block => block.IsSameConstructAs(program.Me)).Components.TryGet(out _chatBroadcastController);
                if (_chatBroadcastController == null)
                {
                    throw new Exception("Chat broadcast controller not found.");
                }

                _chatBroadcastController.UseAntenna = true;
            }

            #endregion

            public void Log(string message)
            {
                _chatBroadcastController.SendMessage(message);
            }
        }
    }
}