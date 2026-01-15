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
        public class AssemblerClient
        {
            #region private fields

            private readonly string _inventoryRequestTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Request;
            private readonly string _inventoryResponseTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Response;
            private readonly Logger _logger;
            private readonly AssemblerServiceConfiguration _assemblerServiceConfiguration;
            private readonly IMyIntergridCommunicationSystem _igc;
            private readonly IMyBroadcastListener _igcListener;

            private int _messageCount = 0;

            #endregion

            #region properties

            public List<InventoryServiceMessage> PendingMessages { get; private set; }

            #endregion

            #region construction

            public AssemblerClient(Logger logger, AssemblerServiceConfiguration assemblerServiceConfiguration, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;

                _assemblerServiceConfiguration = assemblerServiceConfiguration;
                _inventoryRequestTag = assemblerServiceConfiguration.ShipId + "/" + _inventoryRequestTag;
                _inventoryResponseTag = assemblerServiceConfiguration.ShipId + "/" + _inventoryResponseTag;

                _igc = igc;
                _igcListener = igc.RegisterBroadcastListener(_inventoryResponseTag);
                _igcListener.SetMessageCallback(_inventoryResponseTag);

                PendingMessages = new List<InventoryServiceMessage>();

                _logger.LogInfo("Initialized AssemblerClient");
            }

            #endregion

            #region methods

            public List<InventoryServiceMessage> GetPendingMessages()
            {
                var messages = PendingMessages.ToList();
                PendingMessages.Clear();
                return messages;
            }

            #endregion

            #region response methods

            public void CheckResponses()
            {
            }

            #endregion

            #region request methods

            #endregion
        }
    }
}