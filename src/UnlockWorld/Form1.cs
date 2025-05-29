using System;
using System.Drawing;
using System.Windows.Forms;
using FlaUI.Core.AutomationElements;
using UnlockWorld.Core;

namespace UnlockWorld
{
    public partial class Form1 : Form
    {
        private ElementFinder? _elementFinder;
        private ElementEnabler? _elementEnabler;
        private LogHelper? _logHelper;
        private bool _isDragging;
        private AutomationElement? _currentElement;

        public Form1()
        {
            InitializeComponent();

            // Hook up event handlers
            Load += Form1_Load;
            FormClosed += Form1_FormClosed;
            btnFinderTool.MouseDown += BtnFinderTool_MouseDown;
            btnFinderTool.MouseMove += BtnFinderTool_MouseMove;
            btnFinderTool.MouseUp += BtnFinderTool_MouseUp;
            btnClearLogs.Click += BtnClearLogs_Click;
            
            // Set tooltip for the toggle checkbox
            toolTip.SetToolTip(chkToggleOnly, "启用此选项将只切换复选框的状态，而不会启用禁用的控件");
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            // Initialize logging
            _logHelper = new LogHelper(logTextBox);
            
            // Initialize the element finder and enabler
            _elementFinder = new ElementFinder(_logHelper);
            _elementEnabler = new ElementEnabler(_logHelper);

            // Set the application icon
            Icon = ResourceManager.GetApplicationIcon();

            // Set tooltips
            toolTip.SetToolTip(btnFinderTool, "点击并拖动此按钮到其他应用程序中被禁用的控件上");
            toolTip.SetToolTip(btnClearLogs, "清除日志显示");
            
            // Set version in status bar
            toolStripStatusLabel.Text = $"UnlockWorld v{ResourceManager.GetApplicationVersion()}";

            _logHelper.Log("应用程序已启动，请将查找控件按钮拖放到需要启用的控件上");
        }

        private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
        {
            // Clean up resources
            _elementFinder?.Dispose();
        }

        private void BtnFinderTool_MouseDown(object? sender, MouseEventArgs e)
        {
            // Start dragging when the left mouse button is pressed
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _logHelper?.Log("开始拖动查找工具...");
                
                // Change the cursor to a crosshair to indicate selection mode
                btnFinderTool.Cursor = ResourceManager.GetFinderCursor();
            }
        }

        private void BtnFinderTool_MouseMove(object? sender, MouseEventArgs e)
        {
            // Only process mouse move if we're currently dragging
            if (!_isDragging) return;

            // Continue showing the special cursor while dragging
            btnFinderTool.Cursor = ResourceManager.GetFinderCursor();
        }

        private void BtnFinderTool_MouseUp(object? sender, MouseEventArgs e)
        {
            // Process when the mouse button is released
            if (_isDragging && e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                btnFinderTool.Cursor = Cursors.Default;

                // Get the current screen position
                Point screenPoint = Control.MousePosition;
                
                // Find the element at the current position
                FindAndAnalyzeElement(screenPoint);
            }
        }

        private void FindAndAnalyzeElement(Point screenPoint)
        {
            if (_elementFinder == null || _elementEnabler == null || _logHelper == null)
            {
                MessageBox.Show("应用程序初始化失败，请重启应用", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _logHelper.Log($"正在查找坐标 X={screenPoint.X}, Y={screenPoint.Y} 处的UI元素...");
            UpdateStatus("正在分析元素...", Color.Blue);

            try
            {
                // Find the element at the position
                _currentElement = _elementFinder.FindElementFromPoint(screenPoint);

                if (_currentElement == null)
                {
                    _logHelper.LogError("未找到UI元素");
                    txtElementDetails.Text = "未找到UI元素，请重试";
                    UpdateStatus("未找到元素", Color.Red);
                    return;
                }

                // Get and display element details
                string details = _elementFinder.GetElementDetails(_currentElement);
                txtElementDetails.Text = details;
                _logHelper.Log("成功找到UI元素");

                // Check if it's a checkbox and we only want to toggle its state
                bool isCheckbox = _currentElement.Patterns.Toggle.IsSupported;
                bool toggleOnlyMode = chkToggleOnly.Checked;                if (isCheckbox && toggleOnlyMode)
                {
                    _logHelper.Log("检测到复选框，正在切换状态而非启用...");
                    
                    bool toggleSuccess = _elementEnabler.ToggleCheckboxState(_currentElement);
                    if (toggleSuccess)
                    {
                        _logHelper.LogSuccess("成功切换复选框状态！");
                        UpdateStatus("成功切换复选框状态", Color.Green);
                    }
                    else
                    {
                        _logHelper.LogError("无法切换复选框状态");
                        UpdateStatus("无法切换复选框状态", Color.Red);
                    }
                    
                    return;
                }

                // Check if the element is already enabled
                bool isEnabled = _currentElement.Properties.IsEnabled.ValueOrDefault;
                
                if (isEnabled)
                {
                    _logHelper.LogWarning("此元素已经处于启用状态，无需操作");
                    UpdateStatus("元素已启用", Color.Orange);
                    return;
                }

                // Try to enable the element
                bool success = _elementEnabler.TryEnableElement(_currentElement);
                
                if (success)
                {
                    _logHelper.LogSuccess("成功启用UI元素！");
                    UpdateStatus("成功启用元素", Color.Green);
                }
                else
                {
                    _logHelper.LogError("无法启用此UI元素，可能不支持启用操作");
                    UpdateStatus("无法启用元素", Color.Red);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogError($"操作过程中发生错误: {ex.Message}");
                UpdateStatus("发生错误", Color.Red);
            }
        }

        private void BtnClearLogs_Click(object? sender, EventArgs e)
        {
            _logHelper?.Clear();
            _logHelper?.Log("日志已清除");
        }
        
        private void UpdateStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            toolStripStatusLabel.Text = $"{message} - UnlockWorld v{ResourceManager.GetApplicationVersion()}";
            toolStripStatusLabel.ForeColor = color;
        }
    }
}
