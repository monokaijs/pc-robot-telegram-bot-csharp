using PCRobotApp.Utils;
using Telegram.Bot;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class GetIpCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public GetIpCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    try {
      var ip = SystemUtils.GetPublicIP();
      await _botClient.SendMessage(chatId, $"Your server's public IP is: {ip}");
    }
    catch (Exception ex) {
      await _botClient.SendMessage(chatId, $"Failed to retrieve public IP: {ex.Message}");
    }
  }
}