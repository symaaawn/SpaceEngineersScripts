using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        class RelativePositioningSystemManager
        {
            #region private fields

            private ServiceStateDc serviceState;

            private readonly Logger _logger;
            private readonly RelativePositioningSystemActions _relativePositioningSystemActions;
            private readonly RelativePositioningSystemClient _relativePositioningSystemClient;
            private readonly IMyRemoteControl _referencePoint;

            #endregion

            #region properties

            private StatusLightCollection StatusLightCollection { get; set; }

            #endregion

            #region construction

            public RelativePositioningSystemManager(Logger logger, RelativePositioningSystemActions relativePositioningSystemActions, RelativePositioningSystemClient relativePositioningSystemClient,
                List<IMyLightingBlock> statusLights, IMyRemoteControl referencePoint)
            {
                _logger = logger;
                _relativePositioningSystemActions = relativePositioningSystemActions;
                _relativePositioningSystemClient = relativePositioningSystemClient;
                _referencePoint = referencePoint;
                StatusLightCollection = new StatusLightCollection(statusLights);

                SetServiceState(ServiceStateDc.Auto);

                _logger.LogInfo("Initialized RelativePositioningSystemManager");
            }

            #endregion

            #region public methods

            public void Update()
            {
                switch (serviceState)
                {
                    case ServiceStateDc.Error:
                        break;
                    case ServiceStateDc.Manual:
                        break;
                    case ServiceStateDc.Auto:
                        BroadcastPosition();
                        break;
                }
            }

            public bool SetServiceState(ServiceStateDc newState)
            {
                if (serviceState != newState)
                {
                    serviceState = newState;
                    _logger.LogInfo($"RPS service state changed to {serviceState}");
                    StatusLightCollection.UpdateLights(serviceState);

                    return true;
                }

                return false;
            }

            #endregion

            #region private methods

            private void BroadcastPosition() 
            {
                _relativePositioningSystemClient.SendPosition(_referencePoint.WorldMatrix);
            }

            #endregion
        }
    }
}
