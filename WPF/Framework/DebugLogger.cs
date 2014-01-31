using System;
using System.Diagnostics;
using Caliburn.Micro;

namespace AnotherCM.WPF.Framework {
    public class DebugLogger : ILog {
        private const string ErrorLevel = "ERR";
        private const string InfoLevel = "INF";
        private const string WarnLevel = "WRN";
        private readonly string typeName;

        public DebugLogger (Type type) {
            this.typeName = type.FullName;
        }

        #region ILog

        public void Error (Exception exception) {
            this.Log(ErrorLevel, "{0}", exception);
        }

        public void Info (string format, params object[] args) {
            this.Log(InfoLevel, format, args);
        }

        public void Warn (string format, params object[] args) {
            this.Log(WarnLevel, format, args);
        }

        #endregion ILog

        private void Log (string level, string format, params object[] args) {
            string msg = format;
            if (args != null && args.Length != 0) {
                msg = String.Format(format, args);
            }
            Debug.WriteLine("[{0}] <{2}>: {1}", level, msg, this.typeName);
        }
    }
}