using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;

namespace PCRobotApp.Utils;

public static class SystemUtils {
  private const int KEYEVENTF_KEYUP = 0x0002;

  // Windows-specific implementation using keybd_event
  public static void SendMediaKey(string key) {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      // Define virtual key codes
      ushort VK_MEDIA_PLAY_PAUSE = 0xB3;
      ushort VK_MEDIA_NEXT_TRACK = 0xB0;
      ushort VK_MEDIA_PREV_TRACK = 0xB1;
      ushort VK_VOLUME_UP = 0xAF;
      ushort VK_VOLUME_DOWN = 0xAE;
      ushort VK_VOLUME_MUTE = 0xAD;

      ushort vk = key switch {
        "play_pause" => VK_MEDIA_PLAY_PAUSE,
        "next" => VK_MEDIA_NEXT_TRACK,
        "previous" => VK_MEDIA_PREV_TRACK,
        "volume_up" => VK_VOLUME_UP,
        "volume_down" => VK_VOLUME_DOWN,
        "mute" => VK_VOLUME_MUTE,
        _ => 0
      };

      if (vk != 0) {
        keybd_event((byte)vk, 0, 0, 0);
        keybd_event((byte)vk, 0, KEYEVENTF_KEYUP, 0);
      }
    }
    // Implement for other OSes if needed
  }

  // P/Invoke for keybd_event
  [DllImport("user32.dll")]
  private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

  public static string CaptureScreenshot(int monitor = 1) {
    try {
      // Adjust for multiple monitors if necessary
      var screens = Screen.AllScreens;
      if (monitor < 1 || monitor > screens.Length) monitor = 1; // Default to first monitor

      var bounds = screens[monitor - 1].Bounds;
      using (var bitmap = new Bitmap(bounds.Width, bounds.Height)) {
        using (var g = Graphics.FromImage(bitmap)) {
          g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        }

        var filePath = $"screenshot_{monitor}_{DateTime.Now.Ticks}.png";
        bitmap.Save(filePath, ImageFormat.Png);
        return filePath;
      }
    }
    catch (Exception ex) {
      // Handle exceptions as needed
      throw new Exception("Failed to capture screenshot.", ex);
    }
  }

  public static string GetPublicIP() {
    using (var client = new WebClient()) {
      return client.DownloadString("https://api.ipify.org").Trim();
    }
  }

  public static string GetSystemStats() {
    var cpuUsage = GetCpuUsage();
    var ramUsage = GetRamUsage();
    var topProcesses = GetTopProcesses();

    return $"**CPU Usage:** {cpuUsage}%\n**RAM Usage:** {ramUsage}%\n**Top Processes:**\n{topProcesses}";
  }

  private static float GetCpuUsage() {
    // Simplified CPU usage calculation
    var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    cpuCounter.NextValue();
    Thread.Sleep(1000);
    return cpuCounter.NextValue();
  }

  private static float GetRamUsage() {
    var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
    var availableMemory = ramCounter.NextValue();
    var totalMemory = GetTotalMemoryInMBytes();
    return (totalMemory - availableMemory) / totalMemory * 100;
  }

  private static float GetTotalMemoryInMBytes() {
    var computerInfo = new ComputerInfo();
    return computerInfo.TotalPhysicalMemory / (1024 * 1024);
  }

  private static string GetTopProcesses() {
    var startInfo = new ProcessStartInfo {
      FileName = "cmd.exe",
      Arguments = "/c tasklist /FI \"STATUS eq running\" /FO LIST",
      RedirectStandardOutput = true,
      UseShellExecute = false,
      CreateNoWindow = true
    };

    var process = Process.Start(startInfo);
    if (process == null) return string.Empty;
    var output = process.StandardOutput.ReadToEnd();
    process.WaitForExit();

    // Parse and format output as needed
    return output;
  }

  public static void ScheduleShutdown(int minutes, bool force = false) {
    var command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
      ? $"shutdown /s /t {minutes * 60}" + (force ? " /f" : "")
      : $"sudo shutdown -h +{minutes}";

    Process.Start("cmd.exe", $"/c {command}");
  }

  public static void CancelShutdown() {
    var command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
      ? "shutdown /a"
      : "sudo shutdown -c";

    Process.Start("cmd.exe", $"/c {command}");
  }
}