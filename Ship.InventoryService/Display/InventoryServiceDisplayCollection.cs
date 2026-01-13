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
        public class InventoryServiceDisplayCollection : DisplayCollection
        {
            #region construction

            public InventoryServiceDisplayCollection(List<IMyTextPanel> displays) : base(displays) { }

            #endregion

            #region methods

            public void UpdateDisplays(CargoContainerCollection cargoContainerCollection, ServiceStateDc serviceState)
            {
                var infos = new List<string>
                {
                    $"Inventory Service State: {serviceState}",
                    $"",
                    string.Format("{0,-12} {1,-10}", "Container", "Status"),
                    "--------------------------------"
                };

                foreach (var cargoContainer in cargoContainerCollection.GetCargoContainers().OrderBy(c => c.Name))
                {
                    infos.Add(string.Format("{0,-12} {1, 8}/{2, 8}", cargoContainer.Name, cargoContainer.Inventory.CurrentVolume.FormatFixedPoint(), cargoContainer.Inventory.MaxVolume.FormatFixedPoint()));
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
