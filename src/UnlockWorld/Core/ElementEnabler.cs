using System;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Handles enabling disabled UI elements using various automation patterns
    /// </summary>
    public class ElementEnabler
    {
        private readonly LogHelper _logger;

        public ElementEnabler(LogHelper logger)
        {
            _logger = logger;
        }

        /// <summary>
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

            // If the element is already enabled, no need to do anything
            if (element.Properties.IsEnabled.ValueOrDefault)
            {
                _logger.Log("Element is already enabled");
                return true;
            }

            _logger.Log("Attempting to enable element using available patterns...");

            try
            {
                // Try different patterns depending on the element type
                if (TryEnableViaTogglePattern(element)) return true;
                if (TryEnableViaInvokePattern(element)) return true;
                if (TryEnableViaSelectionPattern(element)) return true;
                if (TryEnableViaValuePattern(element)) return true;
                if (TryEnableViaWindowPattern(element)) return true;
                if (TryEnableViaExpandCollapsePattern(element)) return true;

                // If no pattern worked, try advanced techniques
                if (TryEnableUsingReflection(element)) return true;

                _logger.LogError("No supported pattern found to enable this element");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error enabling element: {ex.Message}");
                return false;
            }
        }

        private bool TryEnableViaTogglePattern(AutomationElement element)
        {
            if (!element.Patterns.Toggle.IsSupported)
                return false;

            try
            {
                var togglePattern = element.Patterns.Toggle.Pattern;
                var currentState = togglePattern.ToggleState.ValueOrDefault;

                if (currentState == ToggleState.Off)
                {
                    _logger.Log("Enabling element using Toggle pattern");
                    togglePattern.Toggle();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using Toggle pattern: {ex.Message}");
            }

            return false;
        }

        private bool TryEnableViaInvokePattern(AutomationElement element)
        {
            if (!element.Patterns.Invoke.IsSupported)
                return false;

            try
            {
                _logger.Log("Enabling element using Invoke pattern");
                element.Patterns.Invoke.Pattern.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using Invoke pattern: {ex.Message}");
                return false;
            }
        }

        private bool TryEnableViaSelectionPattern(AutomationElement element)
        {
            if (!element.Patterns.SelectionItem.IsSupported)
                return false;

            try
            {
                _logger.Log("Enabling element using Selection pattern");
                element.Patterns.SelectionItem.Pattern.Select();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using Selection pattern: {ex.Message}");
                return false;
            }
        }

        private bool TryEnableViaValuePattern(AutomationElement element)
        {
            if (!element.Patterns.Value.IsSupported)
                return false;

            try
            {
                var valuePattern = element.Patterns.Value.Pattern;
                if (!valuePattern.IsReadOnly.ValueOrDefault)
                {
                    _logger.Log("Enabling element using Value pattern");
                    // For demonstration, we set a space character if empty
                    string currentValue = valuePattern.Value.ValueOrDefault ?? string.Empty;
                    if (string.IsNullOrEmpty(currentValue))
                    {
                        valuePattern.SetValue(" ");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using Value pattern: {ex.Message}");
            }

            return false;
        }
        
        private bool TryEnableViaWindowPattern(AutomationElement element)
        {
            if (!element.Patterns.Window.IsSupported)
                return false;
                
            try
            {
                var windowPattern = element.Patterns.Window.Pattern;
                var canMaximize = windowPattern.CanMaximize.ValueOrDefault;
                
                if (canMaximize)
                {
                    _logger.Log("Enabling element using Window pattern");
                    windowPattern.SetWindowVisualState(WindowVisualState.Normal);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using Window pattern: {ex.Message}");
            }
            
            return false;
        }
        
        private bool TryEnableViaExpandCollapsePattern(AutomationElement element)
        {
            if (!element.Patterns.ExpandCollapse.IsSupported)
                return false;
                
            try
            {
                var expandPattern = element.Patterns.ExpandCollapse.Pattern;
                var expandState = expandPattern.ExpandCollapseState.ValueOrDefault;
                
                if (expandState == ExpandCollapseState.Collapsed)
                {
                    _logger.Log("Enabling element using ExpandCollapse pattern");
                    expandPattern.Expand();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using ExpandCollapse pattern: {ex.Message}");
            }
            
            return false;
        }

        private bool TryEnableUsingReflection(AutomationElement element)
        {
            try
            {
                _logger.Log("Attempting to enable element using advanced techniques...");
                
                // This is a placeholder for more advanced techniques
                // In a real implementation, you might use reflection or P/Invoke here
                // to access internal properties or APIs not exposed through the standard patterns
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error using advanced techniques: {ex.Message}");
                return false;
            }
        }
    }
}
