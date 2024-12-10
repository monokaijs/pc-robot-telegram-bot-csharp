using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PCRobotApp.Handlers;

public class ApproveAccessHandler {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public ApproveAccessHandler(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task HandleApproveAsync(CallbackQuery callbackQuery, string userId) {
    _accessControl.AddAllowedUser(userId);
    if (callbackQuery.Message == null) return;
    await _botClient.AnswerCallbackQuery(callbackQuery.Id, "User approved!", false);
    await _botClient.EditMessageText(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId,
      $"User ID {userId} has been granted access!");
    await _botClient.SendMessage(long.Parse(userId), "Your access request has been approved!");
  }

  public async Task HandleRejectAsync(CallbackQuery callbackQuery, string userId) {
    if (callbackQuery.Message == null) return;
    await _botClient.AnswerCallbackQuery(callbackQuery.Id, "User rejected.", false);
    await _botClient.EditMessageText(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId,
      $"User ID {userId} has been rejected.");
    await _botClient.SendMessage(long.Parse(userId), "Your access request has been rejected.");
  }
}