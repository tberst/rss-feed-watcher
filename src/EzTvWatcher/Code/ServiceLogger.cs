using System;
using System.Collections.Generic;
using System.Text;

namespace EzTvWatcher.Code
{
    public class ServiceLogger
    {
        StringBuilder _debugs = new StringBuilder();
        StringBuilder _errors = new StringBuilder();

        public void Debug(string message)
        {
            this._debugs.AppendLine(message);
        }
        public void Error(string message)
        {
            this._errors.AppendLine(message);
        }

        public string Flush()
        {
            return $"DEBUG : {_debugs.ToString()}\r\nERRORS : {_errors.ToString()}";
            

        }
    }
}
