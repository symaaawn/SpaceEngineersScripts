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
        public class InventoryServiceMessage : IgcMessageBase
        {
            public virtual string Item { get; set; }
            public virtual MyFixedPoint Amount { get; set; }

            public override IgcMessageBase Deserialize(ImmutableDictionary<string, string> raw)
            {
                RequestId = int.Parse(raw["RequestId"]);
                Method = raw["Method"];
                Item = raw["Item"];
                Amount = MyFixedPoint.DeserializeStringSafe(raw["Amount"]);

                switch (Method)
                {
                    case "GetInventory":
                        var inventoryRaw = raw["Inventory"];
                        var inventory = ImmutableDictionary<string, MyFixedPoint>.Empty;
                        foreach (var item in inventoryRaw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var parts = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 2)
                            {
                                var key = parts[0];
                                var value = MyFixedPoint.DeserializeStringSafe(parts[1]);
                                inventory = inventory.Add(key, value);
                            }
                        }

                        return new InventoryServiceMessage_GetInventory()
                        {
                            RequestId = RequestId,
                            Method = Method,
                            Item = Item,
                            Amount = Amount,
                            Inventory = inventory
                        };

                    case "PullItems":
                        return new InventoryServiceMessage_PullItems()
                        {
                            RequestId = RequestId,
                            Method = Method,
                            Item = Item,
                            Amount = Amount,
                            SourceInventory = raw["SourceInventory"]
                        };

                    case "PushItems":
                        return new InventoryServiceMessage_PushItems()
                        {
                            RequestId = RequestId,
                            Method = Method,
                            Item = Item,
                            Amount = Amount,
                            TargetInventory = raw["TargetInventory"]
                        };

                    default:
                        return null;
                        
                }
            }

            public override ImmutableDictionary<string, string> Serialize()
            {
                return base.Serialize()
                    .Add("Item", Item)
                    .Add("Amount", Amount.SerializeString());
            }
        }
    }
}
