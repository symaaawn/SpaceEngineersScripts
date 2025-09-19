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

            private readonly Logger _logger;
            private readonly RelativePositioningSystemActions _relativePositioningSystemActions;
            private readonly RelativePositioningSystemClient _relativePositioningSystemClient;
            private List<IMyLightingBlock> _statusLights;
            private IMyRemoteControl _referencePoint;

            #endregion

            #region construction

            public RelativePositioningSystemManager(Logger logger, RelativePositioningSystemActions relativePositioningSystemActions, RelativePositioningSystemClient relativePositioningSystemClient,
                List<IMyLightingBlock> statusLights, IMyRemoteControl referencePoint)
            {
                _logger = logger;
                _relativePositioningSystemActions = relativePositioningSystemActions;
                _relativePositioningSystemClient = relativePositioningSystemClient;
                _statusLights = statusLights;
                _referencePoint = referencePoint;
            }

            #endregion

            #region public methods

            public void Update()
            {
                BroadcastPosition();
            }

            #endregion

            #region private methods

            private void BroadcastPosition() 
            {
                _relativePositioningSystemClient.SendPosition(_referencePoint.GetPosition());
            }

            #endregion
        }
    }
}
