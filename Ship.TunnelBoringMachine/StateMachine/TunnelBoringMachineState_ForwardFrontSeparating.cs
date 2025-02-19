using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_ForwardFrontSeparating : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_ForwardFrontSeparating(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.GreenYellow)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DisengageBorerPistons();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
                TunnelBoringMachineProgram.FrontMergeOff();
                TunnelBoringMachineProgram.FrontConnectorsOff();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Forward)
                {
                    if (TunnelBoringMachineProgram.MergeBlocksFront.All(mergeBlock => !mergeBlock.IsConnected) && TunnelBoringMachineProgram.ConnectorsFront.All(connector => !connector.IsConnected))
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardExtending(TunnelBoringMachineProgram));
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.ForwardFrontSeparating;
            }

            #endregion
        }
    }
}
