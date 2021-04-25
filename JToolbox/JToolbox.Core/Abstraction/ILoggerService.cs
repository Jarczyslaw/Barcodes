using System;

namespace JToolbox.Core.Abstraction
{
    public interface ILoggerService
    {
        string LogFilePath { get; }

        void Debug(string message, params object[] args);

        void Error(Exception exception);

        void Error(Exception exception, string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(Exception exception);

        void Fatal(Exception exception, string message, params object[] args);

        void Fatal(string message, params object[] args);

        void Info(string message, params object[] args);

        void Trace(string message, params object[] args);

        void Warn(string message, params object[] args);
    }
}