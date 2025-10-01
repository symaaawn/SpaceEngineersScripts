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
                            ReferenceMatrix = new MatrixD(
                                double.Parse(raw["ReferenceCoordinates.M11"]), double.Parse(raw["ReferenceCoordinates.M12"]), double.Parse(raw["ReferenceCoordinates.M13"]), double.Parse(raw["ReferenceCoordinates.M14"]),
                                double.Parse(raw["ReferenceCoordinates.M21"]), double.Parse(raw["ReferenceCoordinates.M22"]), double.Parse(raw["ReferenceCoordinates.M23"]), double.Parse(raw["ReferenceCoordinates.M24"]),
                                double.Parse(raw["ReferenceCoordinates.M31"]), double.Parse(raw["ReferenceCoordinates.M32"]), double.Parse(raw["ReferenceCoordinates.M33"]), double.Parse(raw["ReferenceCoordinates.M34"]),
                                double.Parse(raw["ReferenceCoordinates.M41"]), double.Parse(raw["ReferenceCoordinates.M42"]), double.Parse(raw["ReferenceCoordinates.M43"]), double.Parse(raw["ReferenceCoordinates.M44"])
                            )
                        };
                    default:
                        return null;
                }
            }

            public override ImmutableDictionary<string, string> Serialize()
            {
                return base.Serialize();
            }
        }
    }
}
