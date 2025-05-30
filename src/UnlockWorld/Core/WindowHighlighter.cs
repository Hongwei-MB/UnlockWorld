using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UnlockWorld.Core;

public static class WindowHighlighter
{
    private static Form? currentOverlay;
    private static System.Windows.Forms.Timer? currentTimer;

    public static void HighlightWindow(IntPtr hWnd, int durationMs = 1000)
    {
        if (hWnd == IntPtr.Zero)
            return;

        // 清理上一个高亮框和计时器
        CleanupExistingOverlay();

        if (!NativeMethods.GetWindowRect(hWnd, out NativeMethods.RECT rect))
            return;

        Rectangle bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        var overlay = new HighlightOverlay(bounds);

        currentOverlay = overlay;
        overlay.Show();

        currentTimer = new System.Windows.Forms.Timer();
        currentTimer.Interval = durationMs;
        currentTimer.Tick += (s, e) =>
        {
            CleanupExistingOverlay();
        };
        currentTimer.Start();
    }

    private static void CleanupExistingOverlay()
    {
        // 停止旧计时器
        if (currentTimer != null)
        {
            currentTimer.Stop();
            currentTimer.Dispose();
            currentTimer = null;
        }

        // 关闭旧 overlay
        if (currentOverlay != null && !currentOverlay.IsDisposed)
        {
            try
            {
                currentOverlay.Invoke(new Action(() =>
                {
                    currentOverlay.Close();
                    currentOverlay.Dispose();
                }));
            }
            catch
            {
                // UI 线程可能已被关闭，忽略
            }

            currentOverlay = null;
        }
    }

    private class HighlightOverlay : Form
    {
        public HighlightOverlay(Rectangle bounds)
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Red;
            Opacity = 0.4;
            Bounds = bounds;
            TopMost = true;

            Region = CreateBorderRegion(bounds.Size, 4);
        }

        private Region CreateBorderRegion(Size size, int thickness)
        {
            Rectangle outer = new Rectangle(Point.Empty, size);
            Rectangle inner = new Rectangle(thickness, thickness, size.Width - 2 * thickness, size.Height - 2 * thickness);
            Region region = new Region(outer);
            region.Exclude(inner);
            return region;
        }
    }
}
