using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnlockWorld.Core;
using UnlockWorld.Models;

namespace UnlockWorld
{
    public partial class MainForm : Form
    {
        private bool isSelecting = false;
        private Cursor originalCursor;
        private readonly ILogger<MainForm> _logger;
        private readonly AppSettings _appSettings;

        public MainForm(ILogger<MainForm> logger, AppSettings appSettings)
        {
            InitializeComponent();
            _logger = logger;
            _appSettings = appSettings;

            _logger.LogInformation("Initializing main form...");
            _logger.LogInformation("Application settings: {ExampleSetting}", _appSettings.ExampleSetting);

            // If application name is set in appsettings, use it as form title
            if (!string.IsNullOrEmpty(_appSettings.ApplicationName))
            {
                this.Text = _appSettings.ApplicationName;
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {

        }

        private void btnFinderTool_MouseDown(object sender, MouseEventArgs e)
        {
            isSelecting = true;
            originalCursor = Cursor.Current;
            Cursor = Cursors.Cross;

            // 开启全局鼠标追踪
            Task.Run(() =>
            {
                while (isSelecting)
                {
                    IntPtr hWnd = NativeMethods.WindowFromPoint(Cursor.Position);
                    BeginInvoke(() => HighlightWindow(hWnd)); // 可选高亮
                    Thread.Sleep(100);
                }
            });
        }

        private void HighlightWindow(IntPtr hWnd)
        {
            WindowHighlighter.HighlightWindow(hWnd);
        }

        private void btnFinderTool_MouseUp(object sender, MouseEventArgs e)
        {
            isSelecting = false;
            Cursor = originalCursor;

            IntPtr hWnd = NativeMethods.WindowFromPoint(Cursor.Position);

            var controlOp = ControlOperatorFactory.Create(hWnd);
            controlOp.EnableControl();
            controlOp.SetValue("自动填充内容");
        }
    }
}
