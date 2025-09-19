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
            public MatrixD ReferenceMatrix { get; internal set; }

            public override ImmutableDictionary<string, string> Serialize()
            {
                return ImmutableDictionary<string, string>.Empty
                    .Add("RequestId", RequestId.ToString())
                    .Add("Method", Method)
                    .Add("ReferenceCoordinates.M11", ReferenceMatrix.M11.ToString())
                    .Add("ReferenceCoordinates.M12", ReferenceMatrix.M12.ToString())
                    .Add("ReferenceCoordinates.M13", ReferenceMatrix.M13.ToString())
                    .Add("ReferenceCoordinates.M14", ReferenceMatrix.M14.ToString())
                    .Add("ReferenceCoordinates.M21", ReferenceMatrix.M21.ToString())
                    .Add("ReferenceCoordinates.M22", ReferenceMatrix.M22.ToString())
                    .Add("ReferenceCoordinates.M23", ReferenceMatrix.M23.ToString())
                    .Add("ReferenceCoordinates.M24", ReferenceMatrix.M24.ToString())
                    .Add("ReferenceCoordinates.M31", ReferenceMatrix.M31.ToString())
                    .Add("ReferenceCoordinates.M32", ReferenceMatrix.M32.ToString())
                    .Add("ReferenceCoordinates.M33", ReferenceMatrix.M33.ToString())
                    .Add("ReferenceCoordinates.M34", ReferenceMatrix.M34.ToString())
                    .Add("ReferenceCoordinates.M41", ReferenceMatrix.M41.ToString())
                    .Add("ReferenceCoordinates.M42", ReferenceMatrix.M42.ToString())
                    .Add("ReferenceCoordinates.M43", ReferenceMatrix.M43.ToString())
                    .Add("ReferenceCoordinates.M44", ReferenceMatrix.M44.ToString());
            }
        }
    }
}
