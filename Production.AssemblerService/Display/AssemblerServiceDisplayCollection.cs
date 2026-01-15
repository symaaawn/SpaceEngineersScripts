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
        public class AssemblerServiceDisplayCollection : DisplayCollection
        {
            #region construction

            public AssemblerServiceDisplayCollection(List<IMyTextPanel> displays) : base(displays) { }

            #endregion

            #region methods

            public void UpdateDisplays(AssemblerCollection assemblerCollection, ServiceStateDc serviceState)
            {
                var infos = new List<string>
                {
                    $"Assembler Service State: {serviceState}",
                    $"",
                    string.Format("{0,-12} {1,-6}", "Assembler", "Status"),
                    "--------------------------------"
                };

                foreach (var assembler in assemblerCollection.GetAssemblers().OrderBy(r => r.Name))
                {
                    infos.Add(string.Format("{0,-12} {1,-6}", assembler.Name, assembler.Status));
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
