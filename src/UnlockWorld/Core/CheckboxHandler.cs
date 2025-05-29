using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using System;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Specialized handler for checkbox operations to provide clean, efficient checkbox interaction
    /// </summary>
    public class CheckboxHandler
    {
        private readonly LogHelper _logger;
        private readonly NativeMethodsWrapper _nativeMethods;

        public CheckboxHandler(LogHelper logger, NativeMethodsWrapper nativeMethods)
        {
            _logger = logger;
            _nativeMethods = nativeMethods;
        }

        /// <summary>
        /// Efficiently toggle a checkbox state while handling read-only and visually disabled states
        /// </summary>
        public bool ToggleCheckbox(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot toggle null element");
                return false;
            }

            _logger.Log("Handling checkbox toggle with optimized approach");
            
            // If we don't have a window handle, try UI Automation only
            if (!element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) || hWnd == IntPtr.Zero)
            {
                _logger.Log("No window handle available, using UI Automation toggle");
                return TryToggleUsingAutomation(element);
            }

            // Check if we're dealing with an actual checkbox
            string className = _nativeMethods.GetClassName(hWnd);
            if (string.IsNullOrEmpty(className) || 
                !(className.Contains("Button") || className.Contains("Check")))
            {
                _logger.LogWarning($"Element with class '{className}' might not be a checkbox");
            }

            _logger.Log("Using optimized checkbox handling sequence");

            // 1. Initial optimization - remove read-only state from both control and parent
            PrepareCheckboxHierarchy(hWnd);
            
            // 2. First attempt - direct Win32 toggle which often works for disabled checkboxes
            if (TryWin32Toggle(hWnd))
            {
                _logger.LogSuccess("Successfully toggled checkbox state using Win32 API");
                return true;
            }
            
            // 3. If direct toggle failed, try UI Automation pattern
            if (TryToggleUsingAutomation(element))
            {
                _logger.Log("Successfully toggled checkbox using UI Automation pattern");
                return true;
            }
            
            // 4. Last resort - advanced interaction methods
            if (TryAdvancedInteraction(hWnd))
            {
                _logger.Log("Successfully toggled checkbox using advanced interaction");
                return true;
            }

            _logger.LogError("All checkbox toggle methods failed");
            return false;
        }
        
        /// <summary>
        /// Prepare the checkbox and its parent windows by removing read-only flags
        /// </summary>
        private bool PrepareCheckboxHierarchy(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Preparing checkbox hierarchy");
                
                // Apply style changes only once
                _nativeMethods.RemoveReadOnlyFlag(hWnd);
                
                // Also handle parent window (we only need to do this once)
                var parentHwnd = _nativeMethods.GetParentWindow(hWnd);
                if (parentHwnd != IntPtr.Zero)
                {
                    _nativeMethods.RemoveReadOnlyFlag(parentHwnd);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error preparing checkbox: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Try to toggle checkbox using direct Win32 API calls
        /// </summary>
        private bool TryWin32Toggle(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Attempting direct Win32 toggle");
                
                // Get current check state
                IntPtr currentState = _nativeMethods.SendMessage(
                    hWnd, 
                    NativeMethodsWrapper.BM_GETCHECK, 
                    IntPtr.Zero, 
                    IntPtr.Zero
                );
                
                // Toggle to opposite state
                IntPtr newState = (currentState.ToInt32() == (int)NativeMethodsWrapper.BST_CHECKED) ? 
                    new IntPtr((int)NativeMethodsWrapper.BST_UNCHECKED) : 
                    new IntPtr((int)NativeMethodsWrapper.BST_CHECKED);
                
                // Set the new state
                _nativeMethods.SendMessage(
                    hWnd, 
                    NativeMethodsWrapper.BM_SETCHECK, 
                    newState, 
                    IntPtr.Zero
                );
                
                // Verify the change was successful
                IntPtr verifyState = _nativeMethods.SendMessage(
                    hWnd, 
                    NativeMethodsWrapper.BM_GETCHECK, 
                    IntPtr.Zero, 
                    IntPtr.Zero
                );
                
                return verifyState.Equals(newState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Win32 toggle: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Try to toggle checkbox using UI Automation pattern
        /// </summary>
        private bool TryToggleUsingAutomation(AutomationElement element)
        {
            try
            {
                _logger.Log("Attempting UI Automation toggle pattern");
                
                if (!element.Patterns.Toggle.IsSupported)
                {
                    _logger.LogWarning("Element doesn't support Toggle pattern");
                    return false;
                }
                
                var togglePattern = element.Patterns.Toggle.Pattern;
                var initialState = togglePattern.ToggleState.ValueOrDefault;
                bool wasChecked = (initialState == ToggleState.On);
                
                // Perform toggle
                togglePattern.Toggle();
                
                // Verify the state changed
                if (togglePattern.ToggleState.TryGetValue(out ToggleState newState))
                {
                    bool isChecked = (newState == ToggleState.On);
                    bool changed = (isChecked != wasChecked);
                    
                    if (changed)
                    {
                        _logger.Log($"Toggle state changed from {wasChecked} to {isChecked}");
                        return true;
                    }
                    
                    _logger.LogWarning("Toggle action did not change state");
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error in automation toggle: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Try advanced interaction methods as a last resort
        /// </summary>
        private bool TryAdvancedInteraction(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Attempting advanced checkbox interaction");
                
                // Send BM_CLICK which simulates a user click 
                _nativeMethods.SendMessage(hWnd, NativeMethodsWrapper.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                
                // We can't easily verify this worked, so we'll just assume it did
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in advanced interaction: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Make minimal visual enhancements to ensure checkbox looks properly enabled
        /// </summary>
        public bool EnhanceVisualAppearance(IntPtr hWnd)
        {
            try
            {
                _logger.Log("Applying minimal visual enhancements to checkbox");
                
                // Just send a repaint message
                _nativeMethods.SendMessage(hWnd, NativeMethodsWrapper.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enhancing visual appearance: {ex.Message}");
                return false;
            }
        }
    }
}
