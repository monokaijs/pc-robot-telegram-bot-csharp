using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class RequestAccessCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public RequestAccessCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;
    if (message.From == null) return;
    var userId = message.From.Id.ToString();

    if (_accessControl.IsAllowedUser(userId)) {
      await _botClient.SendMessage(chatId, "You already have access.");
      return;
    }

    var adminId = _accessControl.AdminId;
    if (string.IsNullOrEmpty(adminId)) {
      await _botClient.SendMessage(chatId, "No admin is set yet. Please try again later.");
      return;
    }

    var requestText = $"User ID {userId} is requesting access. Approve?";
    var inlineKeyboard =
      new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Approve", $"approve_access_{userId}"),
        InlineKeyboardButton.WithCallbackData("Reject", $"reject_access_{userId}"));

    await _botClient.SendMessage(long.Parse(adminId), requestText, replyMarkup: inlineKeyboard);
    await _botClient.SendMessage(chatId,
      "Your request has been sent to the admin. Please wait for approval.");
  }
}