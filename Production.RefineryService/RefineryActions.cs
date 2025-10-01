using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryActions
        {
            #region private fields

            private readonly Logger _logger;
            private readonly IMyGridTerminalSystem _gridTerminalSystem;

            #endregion

            #region construction

            public RefineryActions(Logger logger, IMyGridTerminalSystem myGridTerminalSystem)
            {
                _logger = logger;
                _gridTerminalSystem = myGridTerminalSystem;
            }

            #endregion
        }
    }
}
