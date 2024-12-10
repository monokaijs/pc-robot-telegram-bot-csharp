using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PCRobotApp.Middleware;

public class FirstUserAdminMiddleware {
  private readonly AccessControl _accessControl;

  public FirstUserAdminMiddleware(AccessControl accessControl) {
    _accessControl = accessControl;
  }

  public async Task InvokeAsync(Update update, ITelegramBotClient botClient) {
    if (
      update.Type == UpdateType.Message &&
      update.Message != null &&
      update.Message.Type == MessageType.Text &&
      update.Message.From != null
    ) {
      var userId = update.Message.From.Id.ToString();
      if (string.IsNullOrEmpty(_accessControl.AdminId)) {
        _accessControl.SetAdmin(userId);
        await botClient.SendMessage(update.Message.Chat.Id, "You are now the admin!");
      }
    }

    // Proceed to the next middleware or handler
    await Task.CompletedTask;
  }
}