using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class MeteorDefenseSystemState_Idle : MeteorDefenseSystemState
        {
            #region construction

            internal MeteorDefenseSystemState_Idle(Program meteorDefenseSystemProgram)
                : base(meteorDefenseSystemProgram, Color.Yellow)
            {
                meteorDefenseSystemProgram.Turrets.ForEach(t => t.Enabled = false);
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {

            }

            internal override MeteorDefenseSystemStateDc GetState()
            {
                return MeteorDefenseSystemStateDc.Idle;
            }

            #endregion

        }
    }
}
