using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        class RelativePositioningSystemActions
        {
            #region private fields

            private readonly Logger _logger;

            #endregion

            #region construction

            public RelativePositioningSystemActions(Logger logger)
            {
                _logger = logger;
            }

            #endregion

            #region status light methods

            public void UpdateStatusLights(List<IMyLightingBlock> statusLights, ServiceStateDc serviceState)
            {
                switch (serviceState) { 
                    case ServiceStateDc.Error:
                        foreach (var statusLight in statusLights)
                        {
                            if (!statusLight.IsWorking)
                                continue;
                            statusLight.Color = Color.Red;
                            statusLight.Enabled = true;
                        }
                        break;
                case ServiceStateDc.Manual:
                    foreach (var statusLight in statusLights)
                    {
                        if (!statusLight.IsWorking)
                            continue;
                        statusLight.Color = Color.Blue;
                        statusLight.Enabled = true;
                    }
                    break;
                case ServiceStateDc.Idle:
                    foreach (var statusLight in statusLights)
                    {
                        if (!statusLight.IsWorking)
                            continue;
                        statusLight.Color = Color.Yellow;
                        statusLight.Enabled = true;
                    }
                    break;
                case ServiceStateDc.Active:
                    foreach (var statusLight in statusLights)
                    {
                        if (!statusLight.IsWorking)
                            continue;
                        statusLight.Color = Color.Green;
                        statusLight.Enabled = true;
                    }
                    break;
                default:
                    _logger.LogError($"Unknown service state: {serviceState}");
                    break;
                }
            }

            #endregion

            #region reference control methods



            #endregion
        }
    }
}
