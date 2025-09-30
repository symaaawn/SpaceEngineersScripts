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
        public class InventoryServiceMessage_GetInventory : InventoryServiceMessage
        {
            public ImmutableDictionary<string, MyFixedPoint> Inventory { get; set; } = ImmutableDictionary<string, MyFixedPoint>.Empty;

            public override ImmutableDictionary<string, string> Serialize()
            {
                var items = new List<string>();
                foreach (var kvp in Inventory)
                {
                    string key = kvp.Key;
                    string value = kvp.Value.SerializeString();
                    items.Add(key + "=" + value);
                }
                var inventory = string.Join(";", items);

                return base.Serialize().Add("Inventory", inventory);
            }
        }
    }
}
