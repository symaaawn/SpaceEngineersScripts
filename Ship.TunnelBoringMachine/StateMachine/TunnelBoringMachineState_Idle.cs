using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_Idle : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_Idle(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.Blue)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DisengageBorerPistons();
                TunnelBoringMachineProgram.DrillsOff();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
                TunnelBoringMachineProgram.FrontMergeOff();
                TunnelBoringMachineProgram.FrontConnectorsOff();
                TunnelBoringMachineProgram.BackMergeOff();
                TunnelBoringMachineProgram.BackConnectorsOff();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Forward)
                {
                    TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardExtending(TunnelBoringMachineProgram));
                }
                else if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Reverse)
                {
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.Idle;
            }

            #endregion
        }
    }
}
