using PCRobotApp.Utils;
using Telegram.Bot;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class SecretCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public SecretCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    if (message.From == null) return;
    var userId = message.From.Id.ToString();

    if (!_accessControl.IsAllowedUser(userId)) {
      await _botClient.SendMessage(chatId, "You do not have access to this command.");
      return;
    }

    // Implement your secret command logic here
    await _botClient.SendMessage(chatId, "This is a secret command only for authorized users!");
  }
}