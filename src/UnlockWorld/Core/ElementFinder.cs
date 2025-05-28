using System;
using System.Drawing;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Handles finding and identifying UI elements on the screen
    /// </summary>
    public class ElementFinder : IDisposable
    {
        private readonly UIA3Automation _automation;
        private readonly LogHelper _logger;

        public ElementFinder(LogHelper logger)
        {
            _automation = new UIA3Automation();
            _logger = logger;
        }

        /// <summary>
        /// Finds the automation element at the specified screen coordinates
        /// </summary>
        /// <param name="screenPoint">Screen coordinates</param>
        /// <returns>The automation element at the specified point or null if not found</returns>
        public AutomationElement FindElementFromPoint(Point screenPoint)
        {
            try
            {                _logger.Log($"Searching for element at screen coordinates X={screenPoint.X}, Y={screenPoint.Y}");
                var element = _automation.FromPoint(screenPoint);
                return element; // FlaUI handles null checks internally
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error finding element: {ex.Message}");
                return null!; // Explicitly marked as non-null for compiler
            }
        }

        /// <summary>
        /// Gets detailed information about an automation element
        /// </summary>
        /// <param name="element">The automation element</param>
        /// <returns>String containing element details</returns>
        public string GetElementDetails(AutomationElement element)
        {
            if (element == null)
                return "No element found";

            try
            {                string name = element.Properties.Name.ValueOrDefault ?? "[No Name]";
                string controlType = element.Properties.ControlType.ValueOrDefault.ToString() ?? "[Unknown Type]";
                string automationId = element.Properties.AutomationId.ValueOrDefault ?? "[No ID]";
                bool isEnabled = element.Properties.IsEnabled.ValueOrDefault;

                return $"Name: {name}\n" +
                       $"Type: {controlType}\n" +
                       $"AutomationID: {automationId}\n" +
                       $"Enabled: {isEnabled}\n" +
                       $"Supported Patterns: {GetSupportedPatterns(element)}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting element details: {ex.Message}");
                return "Error retrieving element details";
            }
        }

        /// <summary>
        /// Get a comma-separated list of patterns supported by an element
        /// </summary>
        private string GetSupportedPatterns(AutomationElement element)
        {
            var patterns = new List<string>();

            if (element.Patterns.Invoke.IsSupported) patterns.Add("Invoke");
            if (element.Patterns.Toggle.IsSupported) patterns.Add("Toggle");
            if (element.Patterns.Value.IsSupported) patterns.Add("Value");
            if (element.Patterns.SelectionItem.IsSupported) patterns.Add("SelectionItem");
            if (element.Patterns.ExpandCollapse.IsSupported) patterns.Add("ExpandCollapse");
            if (element.Patterns.Window.IsSupported) patterns.Add("Window");

            return patterns.Count > 0 ? string.Join(", ", patterns) : "None";
        }

        public void Dispose()
        {
            _automation?.Dispose();
        }
    }
}
