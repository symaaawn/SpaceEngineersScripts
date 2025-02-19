using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class Logger : ILogger
        {
            #region private fields

            private List<ILogger> _loggerList;
            private readonly Dictionary<LogLevelDc, string> _logLevelAbbreviations = new Dictionary<LogLevelDc, string>()
            {
                { LogLevelDc.Debug,   "DBG" },
                { LogLevelDc.Info,    "INF" },
                { LogLevelDc.Warning, "WRN" },
                { LogLevelDc.Error ,  "ERR" },
                { LogLevelDc.Fatal,   "FTL" }

            };

            #endregion

            #region construction

            public Logger()
            {
                _loggerList = new List<ILogger>();
            }

            #endregion

            #region logging

            public void LogDebug(string message)
            {
                Log($"[{_logLevelAbbreviations[LogLevelDc.Debug]}] {message}");
            }

            public void LogInfo(string message)
            {
                Log($"[{_logLevelAbbreviations[LogLevelDc.Info]}] {message}");
            }

            public void LogWarning(string message)
            {
                Log($"[{_logLevelAbbreviations[LogLevelDc.Warning]}] {message}");
            }

            public void LogError(string message)
            {
                Log($"[{_logLevelAbbreviations[LogLevelDc.Error]}] {message}");
            }

            public void LogFatal(string message)
            {
                Log($"[{_logLevelAbbreviations[LogLevelDc.Fatal]}] {message}");
            }

            #endregion

            public void AddLogger(ILogger logger)
            {
                _loggerList.Add(logger);
            }

            public void Log(string message)
            {
                foreach (var logger in _loggerList)
                {
                    logger.Log($"{DateTime.Now.ToShortTimeString()} {message}");
                }
            }
        }

        public interface ILogger
        {
            void Log(string message);
        }
    }
}
