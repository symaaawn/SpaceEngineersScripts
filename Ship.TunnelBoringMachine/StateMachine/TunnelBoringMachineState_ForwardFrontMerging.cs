using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_ForwardFrontMerging : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_ForwardFrontMerging(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.GreenYellow)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DisengageBorerPistons();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOn();
                TunnelBoringMachineProgram.WeldersOn();
                TunnelBoringMachineProgram.FrontMergeOn();
                TunnelBoringMachineProgram.FrontConnectorsOn();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Forward)
                {
                    if (TunnelBoringMachineProgram.MergeBlocksFront.All(mergeBlock => mergeBlock.IsConnected) && TunnelBoringMachineProgram.ConnectorsFront.All(connector => connector.IsConnected))
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardBackSeparating(TunnelBoringMachineProgram));
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.ForwardFrontMerging;
            }

            #endregion
        }
    }
}
