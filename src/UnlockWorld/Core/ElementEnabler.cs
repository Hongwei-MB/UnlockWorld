using System;
using FlaUI.Core.AutomationElements;
using System.Collections.Generic;

namespace UnlockWorld.Core
{    /// <summary>
    /// Handles enabling disabled UI elements using various automation patterns
    /// </summary>
    public class ElementEnabler
    {
        private readonly LogHelper _logger;
        private readonly NativeMethodsWrapper _nativeMethods;
        private readonly List<IEnableStrategy> _enableStrategies;

        public ElementEnabler(LogHelper logger)
        {
            _logger = logger;
            _nativeMethods = new NativeMethodsWrapper(logger);              // Initialize strategies in order of precedence
            _enableStrategies = new List<IEnableStrategy>
            {
                new ReadOnlyRemovalStrategy(logger, _nativeMethods), // Try removing read-only flags first
                new DirectPropertyStrategy(logger, _nativeMethods),
                new ListItemStrategy(logger, _nativeMethods), // Add ListItem strategy early in the chain
                new CheckboxStrategy(logger, _nativeMethods),
                new LegacyAccessibilityStrategy(logger, _nativeMethods),
                new InvokePatternStrategy(logger),
                new SelectionPatternStrategy(logger),
                new ValuePatternStrategy(logger),
                new WindowPatternStrategy(logger),
                new ExpandCollapsePatternStrategy(logger),
                new TogglePatternStrategy(logger, _nativeMethods),
                new ReflectionStrategy(logger, _nativeMethods)
            };
        }        /// <summary>
        /// Attempts to toggle a checkbox state without enabling it
        /// </summary>
        /// <param name="element">The automation element to toggle</param>
        /// <returns>True if the element was successfully toggled, false otherwise</returns>
        public bool ToggleCheckboxState(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot toggle null element");
                return false;
            }

            _logger.Log("Attempting to toggle checkbox state...");
            
            // Use specialized checkbox handler for clean, efficient toggle operation
            var checkboxHandler = new CheckboxHandler(_logger, _nativeMethods);
            return checkboxHandler.ToggleCheckbox(element);
        }        /// <summary>
        /// Attempts to enable a disabled UI element using various automation patterns
        /// </summary>
        /// <param name="element">The automation element to enable</param>
        /// <returns>True if the element was successfully enabled, false otherwise</returns>
        public bool TryEnableElement(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot enable null element");
                return false;
            }

            bool isCheckbox = element.Patterns.Toggle.IsSupported;
            
            // Special handling for checkboxes to avoid redundant operations
            if (isCheckbox)
            {
                _logger.Log("Detected checkbox - using optimized handling approach");
                
                // First try the specialized checkbox strategy directly
                var checkboxStrategy = new CheckboxStrategy(_logger, _nativeMethods);
                if (checkboxStrategy.CanHandle(element) && checkboxStrategy.Execute(element))
                {
                    _logger.LogSuccess("Successfully enabled checkbox using specialized strategy");
                    return true;
                }
                
                // If that fails, try handling as read-only
                if (IsElementReadOnly(element))
                {
                    _logger.Log("Checkbox appears to be in read-only state - applying minimal handling");
                    if (HandleReadOnlyElement(element))
                    {
                        return IsElementEnabled(element) || element.Properties.IsEnabled.ValueOrDefault;
                    }
                }
            }
            else
            {
                // First check for read-only state for non-checkbox elements
                if (IsElementReadOnly(element))
                {
                    _logger.Log("Element appears to be in read-only state - applying specialized handling");
                    HandleReadOnlyElement(element);
                }
            }

            // If the element is already enabled, return true
            if (element.Properties.IsEnabled.ValueOrDefault)
            {
                _logger.Log("Element is already enabled");
                return true;
            }

            _logger.Log("Attempting to enable element using available patterns...");

            try
            {
                // Try each strategy in order until one succeeds
                foreach (var strategy in _enableStrategies)
                {
                    // Skip strategies we've already tried
                    if (isCheckbox && strategy is CheckboxStrategy)
                        continue;
                        
                    if (strategy.CanHandle(element) && strategy.Execute(element))
                    {
                        // Verify the element is actually enabled
                        if (IsElementEnabled(element))
                        {
                            return true;
                        }
                    }
                }

                _logger.LogError("No supported pattern found to enable this element");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enabling element: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies if an element is actually enabled
        /// </summary>
        private bool IsElementEnabled(AutomationElement element)
        {
            try
            {
                return element.Properties.IsEnabled.ValueOrDefault;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Explicitly checks for and removes read-only flags from a UI element
        /// </summary>
        /// <param name="element">The automation element to handle</param>
        /// <returns>True if the element was successfully processed, false otherwise</returns>
        public bool HandleReadOnlyElement(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot process null element");
                return false;
            }

            _logger.Log("Explicitly handling potential read-only element...");
            
            // Use the specialized read-only removal strategy
            ReadOnlyRemovalStrategy readOnlyStrategy = new ReadOnlyRemovalStrategy(_logger, _nativeMethods);
            bool basicResult = readOnlyStrategy.Execute(element);
            
            // For checkboxes, apply additional handling
            bool isCheckbox = element.Patterns.Toggle.IsSupported;
            if (isCheckbox)
            {
                _logger.Log("Element is a checkbox - applying additional read-only handling techniques");
                
                if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
                {
                    // Get the class name to confirm it's a checkbox
                    string className = _nativeMethods.GetClassName(hWnd);
                    bool isCheckboxControl = className.Contains("Button") || 
                                            className.Contains("Check") || 
                                            className.Contains("Radio");
                    
                    if (isCheckboxControl)
                    {
                        _logger.Log($"Found checkbox control of class '{className}' - applying specialized techniques");
                        
                        // Apply all advanced checkbox enhancement techniques
                        _nativeMethods.ForceCheckboxInteraction(hWnd);
                        _nativeMethods.ResetCheckboxAppearance(hWnd);
                        
                        // Handle parent and root windows
                        var parentHwnd = _nativeMethods.GetParentWindow(hWnd);
                        if (parentHwnd != IntPtr.Zero)
                        {
                            _nativeMethods.RemoveReadOnlyFlag(parentHwnd);
                            _nativeMethods.EnableWindow(parentHwnd, true);
                        }
                        
                        var rootHwnd = _nativeMethods.GetRootWindow(hWnd);
                        if (rootHwnd != IntPtr.Zero && rootHwnd != parentHwnd)
                        {
                            _nativeMethods.RemoveReadOnlyFlag(rootHwnd);
                            _nativeMethods.EnableWindow(rootHwnd, true);
                        }
                        
                        return true;
                    }
                }
            }
            
            return basicResult;
        }

        /// <summary>
        /// Determines if an element is in a read-only state
        /// </summary>
        /// <param name="element">The automation element to check</param>
        /// <returns>True if the element is in read-only state, false otherwise</returns>
        public bool IsElementReadOnly(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot check null element for read-only state");
                return false;
            }

            _logger.Log("Checking if element is in read-only state...");
            
            // Check for UI Automation read-only property, if available
            if (element.Patterns.Value.IsSupported)
            {
                if (element.Patterns.Value.Pattern.IsReadOnly.TryGetValue(out var isReadOnly))
                {
                    _logger.Log($"Element has Value pattern with IsReadOnly = {isReadOnly}");
                    if (isReadOnly)
                        return true;
                }
            }
            
            // Check using Win32 API if we have a handle
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                // Check for read-only flag in the window style
                if (_nativeMethods.HasReadOnlyFlag(hWnd))
                {
                    _logger.Log("Element has read-only flag in window style");
                    return true;
                }
            }
            
            _logger.Log("No read-only flags detected on element");
            return false;
        }        /// <summary>
        /// Attempts to select a ListItem control even if it's disabled
        /// </summary>
        /// <param name="element">The ListItem automation element to select</param>
        /// <returns>True if the element was successfully selected, false otherwise</returns>
        public bool SelectListItem(AutomationElement element)
        {
            if (element == null)
            {
                _logger.LogError("Cannot select null element");
                return false;
            }

            _logger.Log("Attempting to select ListItem...");
            
            // First check if it's a valid ListItem control or supports SelectionItem pattern
            bool isListItem = element.ControlType == FlaUI.Core.Definitions.ControlType.ListItem;
            bool hasSelectionItem = element.Patterns.SelectionItem.IsSupported;
            
            if (!isListItem && !hasSelectionItem)
            {
                _logger.LogError("Element is not a ListItem or doesn't support SelectionItem pattern");
                return false;
            }
            
            _logger.Log($"Control type is ListItem: {isListItem}, Has SelectionItem pattern: {hasSelectionItem}");
            
            // Check initial selection state
            bool initiallySelected = false;
            if (hasSelectionItem)
            {
                initiallySelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                _logger.Log($"Initial selection state: {initiallySelected}");
            }
            
            // Try to enable the element first if disabled
            if (!element.Properties.IsEnabled.ValueOrDefault)
            {
                _logger.Log("ListItem is disabled, attempting to enable it first");
                
                // Use the ListItem strategy directly
                var listItemStrategy = new ListItemStrategy(_logger, _nativeMethods);
                listItemStrategy.Execute(element);
            }
            
            bool selectionSucceeded = false;
            
            // APPROACH 1: Try SelectionItem pattern first as it's usually most reliable
            if (hasSelectionItem)
            {
                _logger.Log("Attempting to select ListItem using SelectionItem pattern");
                try
                {
                    var selectionItemPattern = element.Patterns.SelectionItem.Pattern;
                    selectionItemPattern.Select();
                    _logger.Log("SelectionItem.Select() called successfully");
                    
                    // Verify if selection worked by checking the selection state again
                    bool nowSelected = selectionItemPattern.IsSelected.ValueOrDefault;
                    _logger.Log($"Selection state after Select() call: {nowSelected}");
                    
                    if (nowSelected)
                    {
                        _logger.LogSuccess("ListItem successfully selected using pattern");
                        selectionSucceeded = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error selecting ListItem using pattern: {ex.Message}");
                }
            }
            
            // APPROACH 2: Try Win32 APIs if pattern approach failed
            if (!selectionSucceeded && element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                _logger.Log("Attempting native ListItem selection approaches");
                
                // Get container information
                string className = _nativeMethods.GetClassName(hWnd);
                bool isListView = className.Contains("ListView") || className.Contains("SysListView32");
                
                // First enable the container if needed
                _nativeMethods.FindAndEnableListContainer(hWnd);
                
                // Try programmatic selection first
                _logger.Log("Attempting programmatic selection via Win32 messages");
                _nativeMethods.TrySelectListItem(hWnd, 0, isListView);
                
                // Then try mouse click as a backup approach
                _logger.Log("Attempting to select ListItem using mouse simulation");
                if (_nativeMethods.SimulateMouseClick(hWnd))
                {
                    _logger.Log("Mouse click simulated successfully");
                }
                
                // Check if selection worked
                if (hasSelectionItem)
                {
                    bool nowSelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                    if (nowSelected && !initiallySelected)
                    {
                        _logger.LogSuccess("ListItem successfully selected using Win32 methods");
                        selectionSucceeded = true;
                    }
                }
                else
                {
                    // If we can't verify via pattern, assume it worked since we did our best
                    _logger.Log("Cannot verify selection state, assuming Win32 methods succeeded");
                    selectionSucceeded = true;
                }
            }
            
            // APPROACH 3: Try legacy accessibility as a last resort
            if (!selectionSucceeded && element.Patterns.LegacyIAccessible.IsSupported)
            {
                _logger.Log("Attempting to use LegacyIAccessible pattern for ListItem");
                try
                {
                    element.Patterns.LegacyIAccessible.Pattern.DoDefaultAction();
                    _logger.Log("LegacyIAccessible.DoDefaultAction() called");
                    
                    // Check if selection worked
                    if (hasSelectionItem)
                    {
                        bool nowSelected = element.Patterns.SelectionItem.Pattern.IsSelected.ValueOrDefault;
                        if (nowSelected && !initiallySelected)
                        {
                            _logger.LogSuccess("ListItem successfully selected using legacy accessibility");
                            selectionSucceeded = true;
                        }
                    }
                    else
                    {
                        // If we can't verify, assume it worked
                        selectionSucceeded = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error using legacy accessibility: {ex.Message}");
                }
            }
            
            if (selectionSucceeded)
            {
                _logger.LogSuccess("ListItem selection operation completed successfully");
                return true;
            }
            else
            {
                _logger.LogWarning("All ListItem selection attempts failed");
                return false;
            }
        }
    }
}
