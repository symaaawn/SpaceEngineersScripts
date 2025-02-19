using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_MiningRetracting : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_MiningRetracting(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.DarkOliveGreen)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DisengageBorerPistons();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram.PistonsBorer.All(piston => piston.CurrentPosition == piston.MinLimit))
                {
                    if (TunnelBoringMachineProgram.MergeBlocksFront.All(mergeBlock => mergeBlock.IsConnected))
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardRetracting(TunnelBoringMachineProgram));
                    }

                    if (TunnelBoringMachineProgram.MergeBlocksBack.All(mergeBlock => mergeBlock.IsConnected))
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_ForwardExtending(TunnelBoringMachineProgram));
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.MiningRetracting;
            }

            #endregion
        }
    }
}
