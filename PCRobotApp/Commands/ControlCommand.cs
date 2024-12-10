using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;

namespace PCRobotApp.Commands;

public class ControlCommand {
  private readonly AccessControl _accessControl;
  private readonly ITelegramBotClient _botClient;

  public ControlCommand(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
  }

  public async Task ExecuteAsync(Message message) {
    var chatId = message.Chat.Id;

    var inlineKeyboard = new InlineKeyboardMarkup(new[] {
      new[] {
        InlineKeyboardButton.WithCallbackData("Play/Pause", "play_pause"),
        InlineKeyboardButton.WithCallbackData("Next", "next"),
        InlineKeyboardButton.WithCallbackData("Previous", "previous")
      },
      new[] {
        InlineKeyboardButton.WithCallbackData("Volume Up", "volume_up"),
        InlineKeyboardButton.WithCallbackData("Volume Down", "volume_down"),
        InlineKeyboardButton.WithCallbackData("Mute", "mute")
      },
      new[] {
        InlineKeyboardButton.WithCallbackData("Seek Forward 10s", "seek_forward_10"),
        InlineKeyboardButton.WithCallbackData("Seek Backward 10s", "seek_backward_10")
      }
    });

    await _botClient.SendMessage(chatId, "Media Controls:", replyMarkup: inlineKeyboard);
  }
}