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
        public class RelativePositioningSystemServiceMessage_Update : RelativePositioningSystemServiceMessage
        {
            public Vector3D ReferenceCoordinates { get; internal set; }

            public override ImmutableDictionary<string, string> Serialize()
            {
                return ImmutableDictionary<string, string>.Empty
                    .Add("RequestId", RequestId.ToString())
                    .Add("Method", Method)
                    .Add("ReferenceCoordinates.X", ReferenceCoordinates.X.ToString())
                    .Add("ReferenceCoordinates.Y", ReferenceCoordinates.Y.ToString())
                    .Add("ReferenceCoordinates.Z", ReferenceCoordinates.Z.ToString());
            }
        }
    }
}
