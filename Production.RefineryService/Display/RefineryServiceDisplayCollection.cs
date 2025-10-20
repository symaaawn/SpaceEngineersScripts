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
        public class RefineryServiceDisplayCollection : DisplayCollection
        {
            #region construction

            public RefineryServiceDisplayCollection(List<IMyTextPanel> displays) : base(displays) { }

            #endregion

            #region methods

            public void UpdateDisplays(RefineryCollection refineryCollection, ServiceStateDc serviceState)
            {
                var infos = new List<string>
                {
                    $"Refinery Service State: {serviceState}",
                    $"",
                    string.Format("{0,-14} {1,-6}", "Refinery", "Status"),
                    "----------------------"
                };

                foreach (var refinery in refineryCollection.GetRefineries())
                {
                    infos.Add(string.Format("{0,-14} {1,-6}", refinery.Name, refinery.Status));
                }

                foreach (var display in _displays)
                {
                    display.RenderDisplay(infos);
                }
            }

            #endregion
        }
    }
}
