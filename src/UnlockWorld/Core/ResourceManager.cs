using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Handles loading and managing application resources
    /// </summary>
    public static class ResourceManager
    {        /// <summary>
        /// Gets the main application icon
        /// </summary>
        public static Icon GetApplicationIcon()
        {
            // Just return the default application icon
            return SystemIcons.Application;
        }

        /// <summary>
        /// Gets the cursor for the finder tool
        /// </summary>
        public static Cursor GetFinderCursor()
        {
            try
            {
                string cursorPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
                    "Resources",
                    "finder.cur");

                if (File.Exists(cursorPath))
                {
                    return new Cursor(cursorPath);
                }
                
                // Fallback to default cross cursor
                return Cursors.Cross;
            }
            catch (Exception)
            {
                return Cursors.Cross;
            }
        }
        
        /// <summary>
        /// Gets the application's current version
        /// </summary>
        public static string GetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
        }
    }
}
