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
        /** 
         * <summary>
         * Composite logger that can use multiple loggers that are implementing the <c>ILogger</c> interface.
         * </summary>
         */
        public class Logger : ILogger
        {
            #region private fields

            private readonly List<ILogger> _loggerList;
            private readonly Dictionary<LogLevelDc, string> _logLevelAbbreviations = new Dictionary<LogLevelDc, string>()
            {
                { LogLevelDc.Debug,   "DBG" },
                { LogLevelDc.Info,    "INF" },
                { LogLevelDc.Warning, "WRN" },
                { LogLevelDc.Error ,  "ERR" },
                { LogLevelDc.Fatal,   "FTL" }

            };
            private readonly ProgramInformationDc _programInformation;

            #endregion

            #region construction

            public Logger(ProgramInformationDc programInformation)
            {
                _loggerList = new List<ILogger>();
                _programInformation = programInformation;
            }

            #endregion

            #region logging

            public void LogDebug(string message)
            {
                if (_programInformation != null && _programInformation.LogLevel > LogLevelDc.Debug)
                    return;
                Log($"[{_logLevelAbbreviations[LogLevelDc.Debug]}] {message}", LogLevelDc.Debug);
            }

            public void LogInfo(string message)
            {
                if (_programInformation != null && _programInformation.LogLevel > LogLevelDc.Info)
                    return;
                Log($"[{_logLevelAbbreviations[LogLevelDc.Info]}] {message}", LogLevelDc.Info);
            }

            public void LogWarning(string message)
            {
                if (_programInformation != null && _programInformation.LogLevel > LogLevelDc.Warning)
                    return;
                Log($"[{_logLevelAbbreviations[LogLevelDc.Warning]}] {message}", LogLevelDc.Warning);
            }

            public void LogError(string message)
            {
                if (_programInformation != null && _programInformation.LogLevel > LogLevelDc.Error)
                    return;
                Log($"[{_logLevelAbbreviations[LogLevelDc.Error]}] {message}", LogLevelDc.Error);
            }

            public void LogFatal(string message)
            {
                if (_programInformation != null && _programInformation.LogLevel > LogLevelDc.Fatal)
                    return;
                Log($"[{_logLevelAbbreviations[LogLevelDc.Fatal]}] {message}", LogLevelDc.Fatal);
            }

            #endregion

            public void AddLogger(ILogger logger)
            {
                _loggerList.Add(logger);
            }

            public void Log(string message, LogLevelDc logLevel)
            {
                foreach (var logger in _loggerList)
                {
                    logger.Log($"{DateTime.Now.ToShortTimeString()} {message}", logLevel);
                }
            }
        }

        public interface ILogger
        {
            void Log(string message, LogLevelDc logLevel);
        }
    }
}
