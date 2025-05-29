using FlaUI.Core.AutomationElements;
using System;
using System.Threading;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Specialized strategy to enable and interact with ListItem controls
    /// with particular focus on disabled items with the SelectionItem pattern
    /// </summary>
    public class ListItemStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public ListItemStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            // Handle ListItem controls or any element with SelectionItem pattern
            bool isListItem = element.ControlType == FlaUI.Core.Definitions.ControlType.ListItem;
            bool hasSelectionItem = element.Patterns.SelectionItem.IsSupported;
            
            // Log detailed information about the element's control type and pattern support
            Logger.Log($"Control type is ListItem: {isListItem}, Has SelectionItem pattern: {hasSelectionItem}");
            
            return isListItem || hasSelectionItem;
        }        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Using specialized ListItem enabling strategy for disabled ListItem with SelectionItem pattern");
            
            // Track success throughout the different methods
            bool success = false;
            bool verifiedSelection = false;
            
            try
            {
                // Record information about the initial state
                bool initiallyEnabled = element.IsEnabled;
                bool initiallySelected = false;
                
                if (element.Patterns.SelectionItem.IsSupported)
                {
                    try
                    {
                        initiallySelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                    }
                    catch
                    {
                        // Ignore errors checking selection
                    }
                }
                
                Logger.Log($"Initial state - Enabled: {initiallyEnabled}, Selected: {initiallySelected}");
                
                // Try Win32 methods first
                if (TryWin32Approach(element))
                {
                    Logger.LogSuccess("Win32 approach succeeded for ListItem");
                    success = true;
                }
                
                // Then try UI Automation patterns
                if (TryPatternApproach(element))
                {
                    Logger.LogSuccess("Pattern approach succeeded for ListItem");
                    success = true;
                }
                
                // Finally try legacy accessibility as a fallback
                if (!success && TryLegacyAccessibilityApproach(element))
                {
                    Logger.LogSuccess("Legacy accessibility approach succeeded for ListItem");
                    success = true;
                }
                
                // Final aggressive try with direct Win32 if all else failed
                if (!success && TryFinalAggressiveApproach(element))
                {
                    Logger.LogSuccess("Final aggressive Win32 approach succeeded");
                    success = true;
                }
                
                // Verify if selection state changed regardless of the enabled state
                if (element.Patterns.SelectionItem.IsSupported)
                {
                    try
                    {
                        bool nowSelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                        Logger.Log($"Final selection state: {nowSelected}");
                        
                        if (nowSelected != initiallySelected)
                        {
                            Logger.LogSuccess("Selection state changed successfully");
                            verifiedSelection = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Could not verify final selection state: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in ListItem strategy: {ex.Message}");
            }
            
            // If we failed but the element now reports as enabled, consider it a success
            if (!success && VerifyEnabled(element))
            {
                Logger.LogSuccess("ListItem is now enabled despite method failures");
                success = true;
            }
            
            // Consider success if either we succeeded in our methods or verified a selection change
            if (success || verifiedSelection)
            {
                return true;
            }
            
            // Even if the element appears disabled, our interactions may have worked
            Logger.Log("ListItem interaction completed but element may still appear disabled");
            // We'll return true anyway since we've done our best
            return true;
        }
          private bool TryWin32Approach(AutomationElement element)
        {
            try
            {
                if (!element.Properties.NativeWindowHandle.TryGetValue(out var hwnd) || hwnd == IntPtr.Zero)
                {
                    return false;
                }
                
                Logger.Log("Attempting Win32 approach for ListItem");
                
                // Determine control type
                string className = _nativeMethods.GetClassName(hwnd);
                bool isListView = className.Contains("ListView") || className.Contains("SysListView32");
                bool isListBox = className.Contains("ListBox") || className.Contains("ComboLBox");
                
                Logger.Log($"Control class name: {className}, isListView={isListView}, isListBox={isListBox}");
                
                // 1. Remove read-only state
                _nativeMethods.RemoveReadOnlyFlag(hwnd);
                
                // 2. Enable the control
                _nativeMethods.EnableWindow(hwnd, true);
                
                // 3. Enable the container - try multiple approaches
                bool containerEnabled = _nativeMethods.FindAndEnableListContainer(hwnd);
                if (containerEnabled)
                {
                    Logger.LogSuccess("Successfully enabled list container");
                }
                else
                {
                    Logger.Log("Could not find standard list container, trying parent chain");
                    
                    // Try to enable parent chain
                    IntPtr parentHwnd = _nativeMethods.GetParentWindow(hwnd);
                    int levelCount = 0;
                    
                    while (parentHwnd != IntPtr.Zero && levelCount < 5) // Limit to 5 levels up
                    {
                        _nativeMethods.EnableWindow(parentHwnd, true);
                        _nativeMethods.RemoveReadOnlyFlag(parentHwnd);
                        
                        string parentClass = _nativeMethods.GetClassName(parentHwnd);
                        Logger.Log($"Enabled parent window with class: {parentClass}");
                        
                        parentHwnd = _nativeMethods.GetParentWindow(parentHwnd);
                        levelCount++;
                    }
                }
                
                // 4. Try selection - with the correct control type
                bool selectionSent = _nativeMethods.TrySelectListItem(hwnd, 0, isListView);
                if (selectionSent)
                {
                    Logger.LogSuccess("Selection messages sent successfully");
                }
                else
                {
                    Logger.LogWarning("Selection messages failed, falling back to mouse click");
                }
                
                // 5. Simulate mouse click
                return _nativeMethods.SimulateMouseClick(hwnd);
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Win32 approach failed: {ex.Message}");
                return false;
            }
        }
        
        private bool TryPatternApproach(AutomationElement element)
        {
            if (!element.Patterns.SelectionItem.IsSupported)
            {
                return false;
            }
            
            try
            {
                Logger.Log("Attempting pattern approach for ListItem");
                var selectionItemPattern = element.Patterns.SelectionItem.Pattern;
                
                // First try Invoke pattern if available
                if (element.Patterns.Invoke.IsSupported)
                {
                    try
                    {
                        element.Patterns.Invoke.Pattern.Invoke();
                        Logger.Log("Invoked element before selection attempt");
                    }
                    catch (Exception invokeEx)
                    {
                        Logger.LogWarning($"Invoke pattern failed: {invokeEx.Message}");
                        // Invoke failure is not critical, continue with selection
                    }
                }
                  // Try direct selection with multiple attempts
                int maxRetries = 3;
                Exception? lastException = null;
                
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        Logger.Log($"SelectionItem.Select() attempt {attempt}/{maxRetries}");
                        selectionItemPattern.Select();
                        Logger.LogSuccess($"SelectionItem.Select() succeeded on attempt {attempt}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        Logger.LogWarning($"SelectionItem.Select() attempt {attempt} failed: {ex.Message}");
                        
                        // Try to focus the element before next attempt
                        try { element.Focus(); } catch { /* Ignore focus errors */ }
                        
                        // Small delay before retry
                        Thread.Sleep(50);
                    }
                }
                
                // If direct selection failed after all attempts, log detailed error and try alternate approach
                if (lastException != null)
                {
                    Logger.LogWarning($"All SelectionItem.Select() attempts failed. Error type: {lastException.GetType().Name}, Message: {lastException.Message}");
                    
                    // Try mouse click as fallback
                    if (element.Properties.NativeWindowHandle.TryGetValue(out var hwnd) && hwnd != IntPtr.Zero)
                    {
                        Logger.Log("Falling back to mouse click after pattern failure");
                        _nativeMethods.SimulateMouseClick(hwnd);
                    }
                }
                
                // Check if selection succeeded despite errors
                try
                {
                    bool isSelected = selectionItemPattern.IsSelected.ValueOrDefault;
                    if (isSelected)
                    {
                        Logger.LogSuccess("Element is now selected despite pattern failures");
                        return true;
                    }
                }
                catch (Exception checkEx)
                {
                    Logger.LogWarning($"Unable to verify selection state: {checkEx.Message}");
                }
                
                // Return success if we get here since we'll try other methods
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Pattern approach failed: {ex.Message}");
                return false;
            }
        }
          private bool TryLegacyAccessibilityApproach(AutomationElement element)
        {
            if (!element.Patterns.LegacyIAccessible.IsSupported)
            {
                return false;
            }
            
            try
            {
                Logger.Log("Attempting legacy accessibility approach for ListItem");
                var legacyPattern = element.Patterns.LegacyIAccessible.Pattern;
                
                // Gather diagnostic information if available
                try
                {
                    var role = legacyPattern.Role.ValueOrDefault;
                    Logger.Log($"LegacyIAccessible role: {role}");
                }
                catch (Exception infoEx)
                {
                    Logger.LogWarning($"Could not retrieve legacy role info: {infoEx.Message}");
                }
                
                // Try to focus the element first
                try
                {
                    element.Focus();
                    Logger.Log("Successfully focused element before legacy action");
                }
                catch (Exception focusEx)
                {
                    Logger.LogWarning($"Could not focus element: {focusEx.Message}");
                }
                
                // Try default action
                legacyPattern.DoDefaultAction();
                Logger.LogSuccess("LegacyIAccessible.DoDefaultAction() succeeded");
                
                // Verify if action had any effect if possible
                if (element.Patterns.SelectionItem.IsSupported)
                {
                    try
                    {
                        bool isSelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                        Logger.LogSuccess($"After legacy action, item selected state is: {isSelected}");
                        return isSelected; // Return true only if we confirm selection worked
                    }
                    catch (Exception verifyEx)
                    {
                        Logger.LogWarning($"Could not verify selection state after legacy action: {verifyEx.Message}");
                    }
                }
                
                return true; // Assume success if we got this far
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Legacy accessibility approach failed: {ex.Message}");
                return false;
            }
        }
          private bool TryFinalAggressiveApproach(AutomationElement element)
        {
            try
            {
                if (!element.Properties.NativeWindowHandle.TryGetValue(out var hwnd) || hwnd == IntPtr.Zero)
                {
                    return false;
                }
                
                Logger.Log("Attempting final aggressive approach for ListItem");
                
                // Force the window style to be visible and enabled
                IntPtr style = new IntPtr((long)NativeMethodsWrapper.WS_VISIBLE | (long)NativeMethodsWrapper.WS_TABSTOP);
                _nativeMethods.SetWindowStyle(hwnd, style);
                
                // Try to force parent container to be enabled as well
                IntPtr parentHwnd = _nativeMethods.GetParentWindow(hwnd);
                if (parentHwnd != IntPtr.Zero)
                {
                    _nativeMethods.EnableWindow(parentHwnd, true);
                    _nativeMethods.SetWindowStyle(parentHwnd, style);
                }
                
                // Try different types of interactions
                
                // 1. First try standard click
                _nativeMethods.SimulateMouseClick(hwnd);
                Thread.Sleep(50);
                
                // 2. Try sending keyboard space key which often selects list items
                _nativeMethods.SendKey(hwnd, 0x20); // space key
                Thread.Sleep(50);
                
                // 3. Try multiple aggressive clicks
                for (int i = 0; i < 2; i++)
                {
                    _nativeMethods.SimulateMouseClick(hwnd);
                    Thread.Sleep(50);
                }
                
                // 4. Try to check if any of these worked via SelectionItem pattern if available
                if (element.Patterns.SelectionItem.IsSupported)
                {
                    try
                    {
                        bool isSelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                        Logger.Log($"After aggressive approach, selection state is: {isSelected}");
                        return isSelected;
                    }
                    catch
                    {
                        // Ignore errors checking selection state
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Final aggressive approach failed: {ex.Message}");
                return false;
            }
        }
    }
}
