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
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program
    {
        public class RelativePositioningSystemServiceMessage : IgcMessageBase
        {
            public override IgcMessageBase Deserialize(ImmutableDictionary<string, string> raw)
            {
                RequestId = int.Parse(raw["RequestId"]);
                Method = raw["Method"];
                switch(Method)
                {
                    case "Update":
                        return new RelativePositioningSystemServiceMessage_Update()
                        {
                            RequestId = RequestId,
                            Method = Method,
                            ReferenceCoordinates = new Vector3D(
                                double.Parse(raw["ReferenceCoordinates.X"]),
                                double.Parse(raw["ReferenceCoordinates.Y"]),
                                double.Parse(raw["ReferenceCoordinates.Z"])
                            )
                        };
                    default:
                        return null;
                }
            }

            public override ImmutableDictionary<string, string> Serialize()
            {
                return ImmutableDictionary<string, string>.Empty;
            }
        }
    }
}
