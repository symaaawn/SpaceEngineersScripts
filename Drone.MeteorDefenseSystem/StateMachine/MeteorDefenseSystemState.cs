using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public abstract class MeteorDefenseSystemState
        {
            #region properties

            internal Program MeteorDefenseSystemProgram { get; set; }

            #endregion

            #region construction

            internal MeteorDefenseSystemState(Program meteorDefenseSystemProgram, Color statusColor)
            {
                MeteorDefenseSystemProgram = meteorDefenseSystemProgram;

                foreach (var statusLight in MeteorDefenseSystemProgram.StatusLights)
                {
                    statusLight.Color = statusColor;
                }
            }

            #endregion

            #region state machine

            public abstract void RunStateMachine();

            internal abstract MeteorDefenseSystemStateDc GetState();

            #endregion
        }
    }
}
