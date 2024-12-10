using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PCRobotApp.Middleware;

public class AccessMiddleware {
  private readonly AccessControl _accessControl;

  public AccessMiddleware(AccessControl accessControl) {
    _accessControl = accessControl;
  }

  public async Task InvokeAsync(Update update, ITelegramBotClient botClient) {
    if (update.Message == null) return;
    if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text) {
      if (update.Message.From == null || update.Message.Text == null) return;
      var userId = update.Message.From.Id.ToString();
      var command = update.Message.Text.Split(' ')[0].ToLower();

      // Allow admins and allowed users to execute any command except /request_access
      if (!_accessControl.IsAllowedUser(userId) && command != "/request_access")
        await botClient.SendMessage(update.Message.Chat.Id,
          "Access denied. Please use /request_access to request permission.");
    }
  }
}