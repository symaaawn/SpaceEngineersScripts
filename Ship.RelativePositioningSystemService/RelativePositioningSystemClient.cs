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
        class RelativePositioningSystemClient
        {
            #region private fields

            private readonly Logger _logger;
            private readonly BaseConfiguration _programmingBlockConfiguration;
            private readonly IMyIntergridCommunicationSystem _igc;
            private readonly string _broadcastTag = IgcTagDc.RelativePositioningSystemServiceMessage + "/" + IgcTagDc.Update;

            private int _messageCount = 0;

            #endregion

            #region construction

            public RelativePositioningSystemClient(Logger logger, BaseConfiguration programmingBlockConfiguration, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _programmingBlockConfiguration = programmingBlockConfiguration;
                _igc = igc;

                _broadcastTag = programmingBlockConfiguration.ShipId + "/" + _broadcastTag;
            }

            #endregion

            #region public methods

            public void SendPosition(MatrixD position)
            {
                var message = new RelativePositioningSystemServiceMessage_Update()
                {
                    RequestId = ++_messageCount,
                    Method = "Update",
                    ReferenceMatrix = position
                };
                _igc.SendBroadcastMessage(_broadcastTag, message.Serialize());
                _logger.LogInfo($"Sending position to tag {_broadcastTag}");
                _logger.LogInfo($"Message ID: {message.RequestId}");
                _logger.LogInfo($"Position: {position}");
            }

            #endregion
        }
    }
}
