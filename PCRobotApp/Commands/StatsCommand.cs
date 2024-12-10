using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class StatsCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public StatsCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    try {
      var stats = SystemUtils.GetSystemStats();
      await _botClient.SendMessage(chatId, stats, ParseMode.Markdown);
    }
    catch (Exception ex) {
      await _botClient.SendMessage(chatId, $"Error fetching stats: {ex.Message}");
    }
  }
}