using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryManager
        {
            #region private fields

            private readonly Logger _logger;
            private readonly RefineryActions _refineryActions;
            private readonly RefineryClient _refineryClient;

            #endregion

            #region properties

            #endregion

            #region construction

            public RefineryManager(Logger logger, RefineryActions refineryActions, RefineryClient refineryClient)
            {
                _logger = logger;
                _refineryActions = refineryActions;
                _refineryClient = refineryClient;   
            }

            #endregion
        }
    }
}
