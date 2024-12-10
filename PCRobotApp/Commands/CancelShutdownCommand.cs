using PCRobotApp.Utils;
using Telegram.Bot;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class CancelShutdownCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public CancelShutdownCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    try {
      SystemUtils.CancelShutdown();
      await _botClient.SendMessage(chatId, "Successfully canceled the scheduled shutdown.");
    }
    catch (Exception ex) {
      await _botClient.SendMessage(chatId, $"Failed to cancel shutdown: {ex.Message}");
    }
  }
}