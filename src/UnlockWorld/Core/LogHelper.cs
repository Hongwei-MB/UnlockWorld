using System;
using System.Drawing;
using System.Windows.Forms;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Helper class for logging messages to both a RichTextBox and application logs
    /// </summary>
    public class LogHelper
    {
        private readonly RichTextBox _logTextBox;
        private static readonly object _lockObject = new object();

        public LogHelper(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;
        }

        /// <summary>
        /// Logs a standard message
        /// </summary>
        public void Log(string message)
        {
            AppendTextWithTimestamp(message, _logTextBox.ForeColor);
            // In a production app, you might also log to a file here
        }

        /// <summary>
        /// Logs an error message in red
        /// </summary>
        public void LogError(string message)
        {
            AppendTextWithTimestamp("ERROR: " + message, Color.Red);
            // In a production app, you might also log to a file here
        }

        /// <summary>
        /// Logs a success message in green
        /// </summary>
        public void LogSuccess(string message)
        {
            AppendTextWithTimestamp("SUCCESS: " + message, Color.Green);
            // In a production app, you might also log to a file here
        }

        /// <summary>
        /// Logs a warning message in orange
        /// </summary>
        public void LogWarning(string message)
        {
            AppendTextWithTimestamp("WARNING: " + message, Color.Orange);
            // In a production app, you might also log to a file here
        }

        private void AppendTextWithTimestamp(string message, Color color)
        {
            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.Invoke(new Action(() => PerformAppend(message, color)));
            }
            else
            {
                PerformAppend(message, color);
            }
        }

        private void PerformAppend(string message, Color color)
        {
            lock (_lockObject)
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                _logTextBox.SelectionStart = _logTextBox.TextLength;
                _logTextBox.SelectionLength = 0;

                _logTextBox.SelectionColor = color;
                _logTextBox.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
                _logTextBox.ScrollToCaret();
            }
        }

        /// <summary>
        /// Clears the log text box
        /// </summary>
        public void Clear()
        {
            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.Invoke(new Action(() => _logTextBox.Clear()));
            }
            else
            {
                _logTextBox.Clear();
            }
        }
    }
}
