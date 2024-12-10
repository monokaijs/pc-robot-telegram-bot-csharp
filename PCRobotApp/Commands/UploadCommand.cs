using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class UploadCommand {
  private const long MaxFileSize = 2L * 1024 * 1024 * 1024; // 2 GB
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public UploadCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    var text = message.Text?.Split(' ') ?? Array.Empty<string>();

    if (text.Length < 2) {
      await _botClient.SendMessage(chatId, "Please specify a file path. Usage: /upload <file_path>");
      return;
    }

    var filePath = string.Join(' ', text[1..]);

    if (!File.Exists(filePath)) {
      await _botClient.SendMessage(chatId, "File not found. Please provide a valid file path.");
      return;
    }

    var fileInfo = new FileInfo(filePath);
    if (fileInfo.Length > MaxFileSize) {
      await _botClient.SendMessage(chatId, "File is too large to send via Telegram (limit is 2GB).");
      return;
    }

    try {
      await _botClient.SendDocument(chatId,
        new InputFileStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)));
    }
    catch (Exception ex) {
      await _botClient.SendMessage(chatId, $"Error uploading file: {ex.Message}");
    }
  }
}