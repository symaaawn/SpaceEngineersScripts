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
        public class StatusLightCollection
        {
            #region private fields

            private readonly List<IMyLightingBlock> _statusLights;

            #endregion

            #region construction

            public StatusLightCollection(List<IMyLightingBlock> statusLights)
            {
                _statusLights = statusLights;
            }

            #endregion

            #region methods

            public void UpdateLights(ServiceStateDc serviceState)
            {
                Color color;
                switch (serviceState)
                {
                    case ServiceStateDc.Error:
                        color = Color.Red;
                        break;
                    case ServiceStateDc.Manual:
                        color = Color.Blue;
                        break;
                    case ServiceStateDc.Auto:
                        color = Color.Green;
                        break;
                    default:
                        color = Color.Yellow;
                        break;
                }
                UpdateLights(color);
            }

            public void UpdateLights(Color color)
            {
                foreach (var statusLight in _statusLights)
                {
                    statusLight.Color = color;
                }
            }

            #endregion
        }
    }
}
