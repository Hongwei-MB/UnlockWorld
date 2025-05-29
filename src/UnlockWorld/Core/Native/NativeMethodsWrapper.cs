using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Wrapper for Windows API calls
    /// </summary>
    public class NativeMethodsWrapper
    {        // Win32 API constants
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_ENABLE = 0x000A;
        public const uint BST_CHECKED = 0x0001;
        public const uint BST_UNCHECKED = 0x0000;
        public const uint BM_GETCHECK = 0x00F0;
        public const uint BM_SETCHECK = 0x00F1;
        public const uint BM_CLICK = 0x00F5;
        public const uint BM_SETSTYLE = 0x00F4;
        public const uint WM_SETTEXT = 0x000C;
        public const uint WM_UPDATEUISTATE = 0x0128;
        public const uint WM_PAINT = 0x000F;
        public const uint BS_CHECKBOX = 0x0002;
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint SC_CLOSE = 0xF060;
        public const uint WM_CONTEXTMENU = 0x007B;
        public const uint WM_SETFOCUS = 0x0007;
        public const uint WM_KILLFOCUS = 0x0008;
        public const uint WM_ERASEBKGND = 0x0014;
        public const uint WM_CTLCOLORSTATIC = 0x0138;
        
        // List control messages
        public const uint LVM_FIRST = 0x1000;
        public const uint LVM_GETITEMSTATE = LVM_FIRST + 44;
        public const uint LVM_SETITEMSTATE = LVM_FIRST + 43;
        public const uint LVIS_SELECTED = 0x0002;
        public const uint LVIS_FOCUSED = 0x0001;
        
        // ListBox messages
        public const uint LB_SETCURSEL = 0x0186;
        public const uint LB_GETCURSEL = 0x0188;
        public const uint LB_GETSEL = 0x0187;
        public const uint LB_SETSEL = 0x0185;
        
        // Additional ListView messages
        public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const uint LVM_ENSUREVISIBLE = LVM_FIRST + 19;
        public const uint LVM_GETCOUNTPERPAGE = LVM_FIRST + 40;
        public const uint LVM_GETSELECTEDCOUNT = LVM_FIRST + 50;
        
        // Additional ListBox constants
        public const uint LB_GETCOUNT = 0x018B;
        
        // Mouse input
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        
        // Style constants
        public const long GWL_STYLE = -16;
        public const long GWL_EXSTYLE = -20;
        public const uint WS_DISABLED = 0x08000000;
        public const uint WS_READONLY = 0x00200000;
        public const uint WS_TABSTOP = 0x00010000;
        public const uint ES_READONLY = 0x0800;
        public const uint EM_SETREADONLY = 0x00CF;
        public const uint BS_AUTOCHECKBOX = 0x0003;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_EX_TRANSPARENT = 0x00000020;
        public const uint WS_EX_LAYERED = 0x00080000;
        public const uint WS_EX_COMPOSITED = 0x02000000;
        public const int GA_ROOT = 2;
        public const int GA_PARENT = 1;
        
        private readonly LogHelper _logger;
        
        public NativeMethodsWrapper(LogHelper logger)
        {
            _logger = logger;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern IntPtr SendMessageWin32(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", EntryPoint = "EnableWindow")]
        private static extern bool EnableWindowWin32(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "GetClassName")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
          [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetAncestor")]
        private static extern IntPtr GetAncestor(IntPtr hwnd, int gaFlags);
        
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        
        public bool EnableWindow(IntPtr hWnd, bool enable)
        {
            try
            {
                _logger.Log($"Calling EnableWindow on handle {hWnd}, enable={enable}");
                bool result = EnableWindowWin32(hWnd, enable);
                _logger.Log($"EnableWindow result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in EnableWindow: {ex.Message}");
                return false;
            }
        }
        
        public IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                _logger.Log($"Sending window message {msg} to handle {hWnd}");
                return SendMessageWin32(hWnd, msg, wParam, lParam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending window message: {ex.Message}");
                return IntPtr.Zero;
            }
        }
        
        public bool ToggleCheckboxStateNative(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Attempting to toggle checkbox using Win32 APIs");
                
                // Get current check state
                IntPtr currentCheck = SendMessage(hWnd, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero);
                
                // Set to opposite state
                IntPtr newCheckState = (currentCheck.ToInt32() == (int)BST_CHECKED) ? 
                    new IntPtr((int)BST_UNCHECKED) : new IntPtr((int)BST_CHECKED);
                    
                SendMessage(hWnd, BM_SETCHECK, newCheckState, IntPtr.Zero);
                _logger.Log("Checkbox state toggled via Win32 API");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error toggling checkbox with Win32 API: {ex.Message}");
                return false;
            }
        }
        
        public void SendAdvancedWindowMessages(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Sending advanced window messages to enable control");
                
                // Send sequence of messages that might enable controls in various frameworks
                SendMessage(hWnd, WM_ENABLE, new IntPtr(1), IntPtr.Zero);
                
                // Simulate a mouse click which can sometimes enable controls
                SendMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                
                // For checkboxes, try BM_CLICK which is a more standard way to click them
                SendMessage(hWnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending advanced window messages: {ex.Message}");
            }
        }
        
        public string GetClassName(IntPtr hWnd)
        {
            try
            {
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);
                return className.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting class name: {ex.Message}");
                return string.Empty;
            }
        }
          public IntPtr GetRootWindow(IntPtr hWnd)
        {
            try
            {
                return GetAncestor(hWnd, GA_ROOT);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting root window: {ex.Message}");
                return IntPtr.Zero;
            }
        }
          /// <summary>
        /// Get the window style flags
        /// </summary>
        public IntPtr GetWindowStyle(IntPtr hWnd)
        {
            try
            {
                // Try the 64-bit version first
                IntPtr style = GetWindowLongPtr(hWnd, (int)GWL_STYLE);
                
                // If it failed and we're on 32-bit, use the 32-bit version
                if (style == IntPtr.Zero && Marshal.GetLastWin32Error() != 0)
                {
                    style = new IntPtr(GetWindowLong(hWnd, (int)GWL_STYLE));
                }
                
                return style;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting window style: {ex.Message}");
                return IntPtr.Zero;
            }
        }
        
        /// <summary>
        /// Set the window style flags
        /// </summary>
        public bool SetWindowStyle(IntPtr hWnd, IntPtr style)
        {
            try
            {
                // Try the 64-bit version first
                IntPtr result = SetWindowLongPtr(hWnd, (int)GWL_STYLE, style);
                
                // If it failed and we're on 32-bit, use the 32-bit version
                if (result == IntPtr.Zero && Marshal.GetLastWin32Error() != 0)
                {
                    SetWindowLong(hWnd, (int)GWL_STYLE, style.ToInt32());
                }
                
                // Force a repaint to apply style changes
                SendMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting window style: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Remove read-only and disabled flags from a control
        /// </summary>
        public bool RemoveReadOnlyFlag(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Attempting to remove read-only flag");
                
                // Get current style
                IntPtr currentStyle = GetWindowStyle(hWnd);
                if (currentStyle == IntPtr.Zero)
                    return false;
                
                // Remove WS_DISABLED and WS_READONLY flags
                IntPtr newStyle = new IntPtr(currentStyle.ToInt64() & ~(long)WS_DISABLED & ~(long)WS_READONLY);
                
                // Set the new style
                SetWindowStyle(hWnd, newStyle);
                
                // For edit controls, explicitly clear the read-only state
                SendMessage(hWnd, EM_SETREADONLY, IntPtr.Zero, IntPtr.Zero);
                
                _logger.Log("Removed read-only flag if present");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing read-only flag: {ex.Message}");
                return false;
            }
        }
          /// <summary>
        /// Performs a comprehensive attempt to force a checkbox to appear enabled
        /// </summary>
        public bool ForceCheckboxVisuallyEnabled(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Attempting to force checkbox to appear visually enabled");
                
                // Use the most comprehensive approach
                if (ResetCheckboxAppearance(hWnd))
                {
                    _logger.Log("Applied comprehensive reset to checkbox appearance");
                }
                
                // First try standard EnableWindow - might not work but worth trying
                EnableWindow(hWnd, true);
                
                // Remove read-only flag which might be causing the checkbox to appear disabled
                RemoveReadOnlyFlag(hWnd);
                
                // Apply proper checkbox style bits
                IntPtr currentStyle = GetWindowStyle(hWnd);
                if (currentStyle != IntPtr.Zero)
                {
                    // Add WS_VISIBLE and WS_TABSTOP, remove WS_DISABLED
                    IntPtr newStyle = new IntPtr(
                        (currentStyle.ToInt64() & ~(long)WS_DISABLED) | 
                        (long)WS_VISIBLE | 
                        (long)WS_TABSTOP
                    );
                    
                    SetWindowStyle(hWnd, newStyle);
                }
                
                // Perform a series of UI refresh operations
                SendMessage(hWnd, WM_UPDATEUISTATE, new IntPtr(0), IntPtr.Zero);
                
                // Try both checkbox styles to see which works better
                SendMessage(hWnd, BM_SETSTYLE, new IntPtr((int)BS_AUTOCHECKBOX), new IntPtr(1));
                
                // Give focus to the control
                SendMessage(hWnd, WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);
                
                // Force a complete repaint
                SendMessage(hWnd, WM_ERASEBKGND, IntPtr.Zero, IntPtr.Zero);
                SendMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                _logger.Log("Applied enhanced visual techniques to checkbox");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error forcing checkbox visual enable: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets the parent window handle
        /// </summary>
        public IntPtr GetParentWindow(IntPtr hWnd)
        {
            try
            {
                return GetAncestor(hWnd, GA_PARENT);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting parent window: {ex.Message}");
                return IntPtr.Zero;
            }
        }
        
        /// <summary>
        /// Detects if a window has the read-only flag set
        /// </summary>
        public bool HasReadOnlyFlag(IntPtr hWnd)
        {
            try
            {
                IntPtr style = GetWindowStyle(hWnd);
                if (style == IntPtr.Zero)
                    return false;
                
                // Check for WS_READONLY or ES_READONLY flags
                bool hasReadOnly = (style.ToInt64() & (long)WS_READONLY) != 0 || 
                                   (style.ToInt64() & (long)ES_READONLY) != 0;
                
                if (hasReadOnly)
                    _logger.Log("Read-only flag detected on control");
                
                return hasReadOnly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error detecting read-only flag: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Advanced checkbox state manipulation for read-only checkboxes
        /// </summary>
        public bool ForceCheckboxInteraction(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Performing advanced checkbox interaction sequence");
                
                // Try to remove any read-only or disabled flags
                RemoveReadOnlyFlag(hWnd);
                
                // Force a visual refresh
                SendMessage(hWnd, WM_ERASEBKGND, IntPtr.Zero, IntPtr.Zero);
                
                // Give focus to ensure the checkbox can receive input
                SendMessage(hWnd, WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);
                
                // Use the most direct method to toggle checkbox state
                IntPtr currentCheck = SendMessage(hWnd, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero);
                IntPtr newCheckState = (currentCheck.ToInt32() == (int)BST_CHECKED) ? 
                    new IntPtr((int)BST_UNCHECKED) : new IntPtr((int)BST_CHECKED);
                SendMessage(hWnd, BM_SETCHECK, newCheckState, IntPtr.Zero);
                
                // Try to give it the appearance of a normal control
                EnableWindow(hWnd, true);
                
                // Try to make the style an auto-checkbox which is usually clickable
                SendMessage(hWnd, BM_SETSTYLE, new IntPtr((int)BS_AUTOCHECKBOX), new IntPtr(1));
                
                // Force a repaint to show changes
                SendMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                // Also modify extended styles if necessary
                ModifyExtendedWindowStyle(hWnd);
                
                _logger.Log("Completed advanced checkbox interaction sequence");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in advanced checkbox interaction: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Modifies the extended window styles to help with checkbox appearance
        /// </summary>
        public bool ModifyExtendedWindowStyle(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Modifying extended window styles");
                
                // Get current extended style
                IntPtr currentExStyle = IntPtr.Zero;
                
                // Try the 64-bit version first
                currentExStyle = GetWindowLongPtr(hWnd, (int)GWL_EXSTYLE);
                
                // If it failed and we're on 32-bit, use the 32-bit version
                if (currentExStyle == IntPtr.Zero && Marshal.GetLastWin32Error() != 0)
                {
                    currentExStyle = new IntPtr(GetWindowLong(hWnd, (int)GWL_EXSTYLE));
                }
                
                if (currentExStyle == IntPtr.Zero)
                    return false;
                
                // Remove styles that might make it appear disabled
                IntPtr newExStyle = new IntPtr(
                    (currentExStyle.ToInt64() & ~(long)WS_EX_TRANSPARENT) | (long)WS_EX_COMPOSITED
                );
                
                // Set the new style
                if (IntPtr.Size == 8)
                {
                    SetWindowLongPtr(hWnd, (int)GWL_EXSTYLE, newExStyle);
                }
                else
                {
                    SetWindowLong(hWnd, (int)GWL_EXSTYLE, newExStyle.ToInt32());
                }
                
                // Force a repaint
                SendMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error modifying extended window style: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Comprehensive approach to reset all styles and flags that might cause disabled appearance
        /// </summary>
        public bool ResetCheckboxAppearance(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Performing comprehensive checkbox appearance reset");
                
                // First remove read-only and disabled flags
                RemoveReadOnlyFlag(hWnd);
                
                // Make sure the control is enabled
                EnableWindow(hWnd, true);
                
                // Get current style
                IntPtr currentStyle = GetWindowStyle(hWnd);
                if (currentStyle == IntPtr.Zero)
                    return false;
                
                // Ensure it has the proper style bits set
                IntPtr newStyle = new IntPtr(
                    (currentStyle.ToInt64() & ~(long)WS_DISABLED) | 
                    (long)WS_VISIBLE | 
                    (long)WS_TABSTOP
                );
                
                // Apply the new style
                SetWindowStyle(hWnd, newStyle);
                
                // Also modify extended window styles
                ModifyExtendedWindowStyle(hWnd);
                
                // Set checkbox style to auto checkbox
                SendMessage(hWnd, BM_SETSTYLE, new IntPtr((int)BS_AUTOCHECKBOX), new IntPtr(1));
                
                // Force a complete repaint
                SendMessage(hWnd, WM_ERASEBKGND, IntPtr.Zero, IntPtr.Zero);
                SendMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                _logger.Log("Completed checkbox appearance reset");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error resetting checkbox appearance: {ex.Message}");
                return false;
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        
        [DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        
        [DllImport("user32.dll", EntryPoint = "ClientToScreen", SetLastError = true)]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
        
        [DllImport("user32.dll", EntryPoint = "SetCursorPos", SetLastError = true)]
        private static extern bool SetCursorPos(int X, int Y);
        
        [DllImport("user32.dll", EntryPoint = "mouse_event", SetLastError = true)]
        private static extern void MouseEvent(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
        
        /// <summary>
        /// Simulates a mouse click on a specific window
        /// </summary>
        public bool SimulateMouseClick(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Simulating mouse click on window");
                
                // Get the window rect
                if (!GetWindowRect(hWnd, out RECT rect))
                {
                    _logger.LogError("Failed to get window rect");
                    return false;
                }
                
                // Calculate center of window
                int centerX = rect.Left + (rect.Right - rect.Left) / 2;
                int centerY = rect.Top + (rect.Bottom - rect.Top) / 2;
                
                // Set cursor position and perform mouse click
                SetCursorPos(centerX, centerY);
                MouseEvent(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                MouseEvent(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                
                _logger.Log($"Clicked at ({centerX}, {centerY})");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error simulating mouse click: {ex.Message}");
                return false;
            }
        }
          /// <summary>
        /// Attempts to select an item in a list control
        /// </summary>
        /// <param name="hWnd">Handle to the list control or list item</param>
        /// <param name="index">Index of the item to select</param>
        /// <param name="isListView">True if it's a ListView control, false for a standard ListBox</param>
        /// <returns>True if selection messages were sent successfully</returns>        /// <summary>
        /// Enhanced version of TrySelectListItem with more robust handling of ListView and ListBox controls
        /// </summary>
        /// <param name="hWnd">Handle to the list control or list item</param>
        /// <param name="index">Index of the item to select</param>
        /// <param name="isListView">True if it's a ListView control, false for a standard ListBox</param>
        /// <returns>True if selection messages were sent successfully</returns>
        public bool TrySelectListItem(IntPtr hWnd, int index = 0, bool isListView = false)
        {
            try
            {
                _logger.Log($"Attempting to select list item at index {index}, isListView={isListView}");
                
                // Make sure the window is enabled first
                EnableWindow(hWnd, true);
                
                // If it's a ListView, we need to use ListView-specific messages
                if (isListView)
                {
                    // First check how many items we have
                    IntPtr itemCount = SendMessage(hWnd, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
                    int count = itemCount.ToInt32();
                    
                    // If the index is too high, default to 0
                    if (count > 0 && index >= count)
                        index = 0;
                    
                    _logger.Log($"ListView contains {count} items, selecting index {index}");
                    
                    if (count > 0)
                    {
                        // Make sure the item is visible
                        SendMessage(hWnd, LVM_ENSUREVISIBLE, new IntPtr(index), new IntPtr(0));
                        
                        // Wait a bit for the UI to update
                        Thread.Sleep(30);
                        
                        // Set the selection state - requires a specific structure, but we'll use the simplified approach
                        // ListView_SetItemState macro equivalent
                        IntPtr lParam = new IntPtr(LVIS_SELECTED | LVIS_FOCUSED);
                        SendMessage(hWnd, LVM_SETITEMSTATE, new IntPtr(index), lParam);
                        
                        _logger.LogSuccess("ListView selection messages sent");
                    }
                    else
                    {
                        _logger.LogWarning("ListView appears to be empty");
                    }
                }
                else
                {
                    // Check list box item count
                    IntPtr itemCount = SendMessage(hWnd, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
                    int count = itemCount.ToInt32();
                    
                    // If the index is too high, default to 0
                    if (count > 0 && index >= count)
                        index = 0;
                        
                    _logger.Log($"ListBox contains {count} items, selecting index {index}");
                    
                    if (count > 0)
                    {
                        // For multi-select listboxes
                        // First try clearing all selections
                        SendMessage(hWnd, LB_SETSEL, IntPtr.Zero, new IntPtr(-1));
                        
                        // Then set the single selection for regular listboxes
                        SendMessage(hWnd, LB_SETCURSEL, new IntPtr(index), IntPtr.Zero);
                        
                        // Also try multi-selection approach - set selection on the specific item
                        SendMessage(hWnd, LB_SETSEL, new IntPtr(1), new IntPtr(index));
                        
                        _logger.LogSuccess("ListBox selection messages sent");
                    }
                    else
                    {
                        _logger.LogWarning("ListBox appears to be empty");
                    }
                }
                
                // Set focus to the control to ensure it's active
                SendMessage(hWnd, WM_SETFOCUS, IntPtr.Zero, IntPtr.Zero);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error selecting list item: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Finds and enables the list container control for a list item
        /// </summary>
        /// <param name="itemHwnd">Handle to the list item</param>
        /// <returns>True if the container was found and enabled</returns>        /// <summary>
        /// Enhanced method to find and enable the container of a list item
        /// Also tries to find non-standard list containers and list controls 
        /// with custom class names
        /// </summary>
        public bool FindAndEnableListContainer(IntPtr itemHwnd)
        {
            try
            {
                if (itemHwnd == IntPtr.Zero)
                {
                    _logger.LogError("Invalid window handle");
                    return false;
                }
                
                _logger.Log("Finding and enabling list container for item");
                
                // Get the parent window which is usually the list container
                IntPtr containerHwnd = GetParentWindow(itemHwnd);
                if (containerHwnd == IntPtr.Zero)
                {
                    _logger.LogWarning("Could not find parent container");
                    return false;
                }
                
                // Get the class name to determine the container type
                string className = GetClassName(containerHwnd);
                _logger.Log($"Found container with class name: {className}");
                
                bool isListView = className.Contains("ListView") || className.Contains("SysListView32");
                bool isListBox = className.Contains("ListBox") || className.Contains("ComboBox") || className.Contains("ComboLBox");
                bool isOtherListContainer = className.Contains("List") || className.EndsWith("View");
                
                // Check for known non-standard list containers used in various frameworks
                bool isPotentialListContainer = isListView || isListBox || isOtherListContainer ||
                                               className.Contains("Tree") || className.Contains("Grid") ||
                                               className.Contains("DataGrid") || className.Contains("Collection");
                
                if (isPotentialListContainer)
                {
                    _logger.Log($"Container appears to be a list container: {className}");
                    
                    // Remove any read-only and disabled flags
                    RemoveReadOnlyFlag(containerHwnd);
                    EnableWindow(containerHwnd, true);
                    
                    // Send specific enable messages
                    SendMessage(containerHwnd, WM_ENABLE, new IntPtr(1), IntPtr.Zero);
                    
                    // For ListView, reset its appearance and force redraw
                    if (isListView)
                    {
                        // Check if ListView has any items before modifying it
                        IntPtr itemCount = SendMessage(containerHwnd, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
                        _logger.Log($"ListView has {itemCount.ToInt32()} items");
                        
                        // Make sure first item is visible
                        if (itemCount.ToInt32() > 0)
                        {
                            SendMessage(containerHwnd, LVM_ENSUREVISIBLE, IntPtr.Zero, IntPtr.Zero);
                        }
                        
                        // Force redraw
                        SendMessage(containerHwnd, WM_ERASEBKGND, IntPtr.Zero, IntPtr.Zero);
                        SendMessage(containerHwnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                    }
                    else if (isListBox)
                    {
                        // For ListBox, try to get item count
                        IntPtr itemCount = SendMessage(containerHwnd, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
                        _logger.Log($"ListBox has {itemCount.ToInt32()} items");
                    }
                    
                    // Try alternative style settings to make list more accessible
                    ModifyWindowStyles(containerHwnd);
                    
                    return true;
                }
                
                _logger.LogWarning("Container does not appear to be a standard list control");
                
                // Try one level up for container
                IntPtr grandparentHwnd = GetParentWindow(containerHwnd);
                if (grandparentHwnd != IntPtr.Zero)
                {
                    string gpClassName = GetClassName(grandparentHwnd);
                    _logger.Log($"Checking grandparent container: {gpClassName}");
                    
                    if (gpClassName.Contains("List") || gpClassName.Contains("View") || 
                        gpClassName.Contains("Grid") || gpClassName.Contains("Tree"))
                    {
                        _logger.Log("Grandparent appears to be a list container, enabling it");
                        RemoveReadOnlyFlag(grandparentHwnd);
                        EnableWindow(grandparentHwnd, true);
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enabling list container: {ex.Message}");
                return false;
            }
        }
        
        // Keyboard input messages
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_CHAR = 0x0102;
        
        /// <summary>
        /// Sends a keyboard key to the specified window
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="vkCode">Virtual key code</param>
        /// <returns>True if the key was sent successfully</returns>
        public bool SendKey(IntPtr hWnd, int vkCode)
        {
            try
            {
                _logger.Log($"Sending key {vkCode} to window {hWnd}");
                
                // Send key down
                SendMessage(hWnd, WM_KEYDOWN, new IntPtr(vkCode), IntPtr.Zero);
                
                // Send char message for printable keys
                if (vkCode >= 32 && vkCode <= 126)
                {
                    SendMessage(hWnd, WM_CHAR, new IntPtr(vkCode), IntPtr.Zero);
                }
                
                // Send key up
                SendMessage(hWnd, WM_KEYUP, new IntPtr(vkCode), IntPtr.Zero);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending key: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Helper method to modify window styles to make controls more accessible
        /// </summary>
        private void ModifyWindowStyles(IntPtr hwnd)
        {
            try
            {
                // Get current style
                IntPtr style = GetWindowStyle(hwnd);
                if (style == IntPtr.Zero)
                    return;
                
                // Add tabstop and remove disabled flags
                long newStyle = style.ToInt64();
                newStyle |= (long)WS_TABSTOP | (long)WS_VISIBLE;
                newStyle &= ~(long)WS_DISABLED;
                
                // Apply the new style
                SetWindowStyle(hwnd, new IntPtr(newStyle));
                
                _logger.Log("Modified window styles for better accessibility");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error modifying window styles: {ex.Message}");
            }
        }
    }
}
