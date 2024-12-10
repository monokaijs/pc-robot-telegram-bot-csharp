using PCRobotApp.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PCRobotApp.Handlers;

public class CallbackHandler {
  private readonly AccessControl _accessControl;
  private readonly ApproveAccessHandler _approveAccessHandler;
  private readonly ITelegramBotClient _botClient;

  public CallbackHandler(ITelegramBotClient botClient, AccessControl accessControl) {
    _botClient = botClient;
    _accessControl = accessControl;
    _approveAccessHandler = new ApproveAccessHandler(_botClient, _accessControl);
  }

  public async Task HandleCallbackAsync(CallbackQuery callbackQuery) {
    var userId = callbackQuery.From.Id.ToString();

    var data = callbackQuery.Data ?? "";

    if ((data.StartsWith("approve_") || data.StartsWith("reject_"))) {
      if (_accessControl.IsAdmin(userId)) {
        if (data.StartsWith("approve_access_")) {
          var userIdToApprove = data.Replace("approve_access_", "");
          await _approveAccessHandler.HandleApproveAsync(callbackQuery, userIdToApprove);
        }
        else if (data.StartsWith("reject_access_")) {
          var userIdToReject = data.Replace("reject_access_", "");
          await _approveAccessHandler.HandleRejectAsync(callbackQuery, userIdToReject);
        }
        else {
          await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Unknown action.");
        }
      }
      else {
        await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Access denied.");
      }
    }

    switch (data) {
      case "play_pause":
      case "next":
      case "previous":
      case "volume_up":
      case "volume_down":
      case "mute":
        SystemUtils.SendMediaKey(data);
        await _botClient.AnswerCallbackQuery(callbackQuery.Id, $"Executed {data.Replace('_', ' ')}.");
        break;
      case var s when s.StartsWith("seek_forward_"):
        // TODO: Implement seek forward logic based on media player shortcuts
        await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Seeked forward 10 seconds.");
        break;
      case var s when s.StartsWith("seek_backward_"):
        // TODO: Implement seek backward logic
        await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Seeked backward 10 seconds.");
        break;
      default:
        await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Unknown command.");
        break;
    }
  }
}