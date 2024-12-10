using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class ScreenshotCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public ScreenshotCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    var text = message.Text?.Split(' ') ?? Array.Empty<string>();

    var monitor = 1; // Default monitor
    if (text.Length > 1 && int.TryParse(text[1], out var parsedMonitor)) monitor = parsedMonitor;

    try {
      var screenshotPath = SystemUtils.CaptureScreenshot(monitor);
      await _botClient.SendPhoto(chatId,
        new InputFileStream(new FileStream(screenshotPath, FileMode.Open, FileAccess.Read)));
      // Optionally delete the file after sending
      File.Delete(screenshotPath);
    }
    catch (Exception ex) {
      await _botClient.SendMessage(chatId, $"Error capturing screenshot: {ex.Message}");
    }
  }
}