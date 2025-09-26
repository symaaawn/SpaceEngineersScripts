using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryClient
        {
            #region private fields

            private readonly Logger _logger;
            private readonly RefineryServiceConfiguration _refineryServiceConfiguration;
            private readonly IMyIntergridCommunicationSystem _igc;
            private readonly string _broadcastTag = IgcTagDc.RefineryServiceMessage + "/" + IgcTagDc.Request;

            private int _messageCount = 0;

            #endregion

            #region construction

            public RefineryClient(Logger logger, RefineryServiceConfiguration refineryServiceConfiguration, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _refineryServiceConfiguration = refineryServiceConfiguration;
                _igc = igc;

                _broadcastTag = refineryServiceConfiguration.ShipId + "/" + _broadcastTag;
            }

            #endregion

            #region public methods

            public void GetInventoryItems()
            {

            }

            public void SendPullRequest()
            {

            }

            public void SendPushRequest()
            {
                var message = new InventoryServiceMessage_PushItems()
                {
                    RequestId = ++_messageCount,
                    Method = "PushItems",
                    Item = "MyObjectBuilder_Ore/Iron",
                    Amount = 100,
                    TargetInventory = "MyInventory"
                };
                _igc.SendBroadcastMessage(_broadcastTag, message.Serialize());
            }

            #endregion
        }
    }
}
