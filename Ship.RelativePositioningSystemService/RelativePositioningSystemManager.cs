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

            private ServiceStateDc State { get; set; } = ServiceStateDc.Active;

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
                UpdateStatus();
                BroadcastPosition();
            }

            #endregion

            #region private methods

            private void UpdateStatus()
            {
                if (_referencePoint == null || !_referencePoint.IsWorking)
                {
                    State = ServiceStateDc.Error;
                    _logger.LogError("No reference point defined");
                }
                else
                {
                    State = ServiceStateDc.Active;
                }
                _relativePositioningSystemActions.UpdateStatusLights(_statusLights, State);
            }

            private void BroadcastPosition() 
            {
                _relativePositioningSystemClient.SendPosition(_referencePoint.WorldMatrix);
            }

            #endregion
        }
    }
}
