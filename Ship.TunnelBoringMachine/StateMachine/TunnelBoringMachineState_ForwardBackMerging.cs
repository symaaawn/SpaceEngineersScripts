using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_ForwardBackMerging : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_ForwardBackMerging(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.GreenYellow)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DisengageBorerPistons();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
                TunnelBoringMachineProgram.BackMergeOn();
                TunnelBoringMachineProgram.BackConnectorsOn();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Forward)
                {
                    if (TunnelBoringMachineProgram.MergeBlocksBack.All(mergeBlock => mergeBlock.IsConnected) && TunnelBoringMachineProgram.ConnectorsBack.All(connector => connector.IsConnected))
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardFrontSeparating(TunnelBoringMachineProgram));
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.ForwardBackMerging;
            }

            #endregion
        }
    }
}
