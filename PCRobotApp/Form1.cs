using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json.Linq;
using PCRobotApp.Utils;
using PCRobotApp.Middleware;
using PCRobotApp.Commands;
using PCRobotApp.Handlers;
using DotNetEnv;
using NLog;
using Microsoft.Win32;

namespace PCRobotApp {
  public partial class Form1 : Form {
    private readonly string envFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PCRobotApp.env");
    private ITelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;
    private Task _botTask;
    private readonly AccessControl _accessControl;
    private readonly FirstUserAdminMiddleware _firstUserAdminMiddleware;
    private readonly AccessMiddleware _accessMiddleware;
    private CallbackHandler _callbackHandler;
    private ControlCommand _controlCommand;
    private RequestAccessCommand _requestAccessCommand;
    private ScreenshotCommand _screenshotCommand;
    private UploadCommand _uploadCommand;
    private CancelShutdownCommand _cancelShutdownCommand;
    private StatsCommand _statsCommand;
    private GetIpCommand _getIpCommand;
    private SecretCommand _secretCommand;
    private ApproveAccessHandler _approveAccessHandler;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private const string RegistryRunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string StateFilePath = "bot_state.txt";

    public Form1() {
      InitializeComponent();
      _accessControl = new AccessControl();
      _firstUserAdminMiddleware = new FirstUserAdminMiddleware(_accessControl);
      _accessMiddleware = new AccessMiddleware(_accessControl);
      _approveAccessHandler = new ApproveAccessHandler(_botClient, _accessControl);
      _callbackHandler = new CallbackHandler(_botClient, _accessControl);
      _controlCommand = new ControlCommand(_botClient, _accessControl);
      _requestAccessCommand = new RequestAccessCommand(_botClient, _accessControl);
      _screenshotCommand = new ScreenshotCommand(_botClient, _accessControl);
      _uploadCommand = new UploadCommand(_botClient, _accessControl);
      _cancelShutdownCommand = new CancelShutdownCommand(_botClient, _accessControl);
      _statsCommand = new StatsCommand(_botClient, _accessControl);
      _getIpCommand = new GetIpCommand(_botClient, _accessControl);
      _secretCommand = new SecretCommand(_botClient, _accessControl);

      LoadToken();
      RestoreBotState();
      chkStartWithWindows.Checked = IsStartupEnabled();
    }

    private void LoadToken() {
      if (System.IO.File.Exists(envFilePath)) {
        var lines = System.IO.File.ReadAllLines(envFilePath);
        foreach (var line in lines) {
          if (line.StartsWith("TELEGRAM_TOKEN=")) {
            txtToken.Text = line.Substring("TELEGRAM_TOKEN=".Length);
            break;
          }
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e) {
      var token = txtToken.Text.Trim();

      if (string.IsNullOrEmpty(token)) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = "Token cannot be empty.";
        return;
      }

      try {
        string content = $"TELEGRAM_TOKEN={token}";
        System.IO.File.WriteAllText(envFilePath, content);
        lblStatus.ForeColor = System.Drawing.Color.Green;
        lblStatus.Text = "Token saved successfully!";
      }
      catch (Exception ex) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = $"Error saving token: {ex.Message}";
        Logger.Error(ex, "Error saving token.");
      }
    }

    private async void btnTest_Click(object sender, EventArgs e) {
      var token = txtToken.Text.Trim();

      if (string.IsNullOrEmpty(token)) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = "Token cannot be empty.";
        return;
      }

      try {
        using (HttpClient client = new HttpClient()) {
          string url = $"https://api.telegram.org/bot{token}/getMe";
          var response = await client.GetStringAsync(url);
          var json = JObject.Parse(response);

          if (json["ok"].Value<bool>()) {
            var username = json["result"]["username"].Value<string>();
            lblStatus.ForeColor = System.Drawing.Color.Green;
            lblStatus.Text = $"Token is valid! Bot Username: @{username}";
          }
          else {
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.Text = "Invalid Token.";
          }
        }
      }
      catch (Exception ex) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = $"Error testing token: {ex.Message}";
        Logger.Error(ex, "Error testing token.");
      }
    }

    private async void btnStartBot_Click(object sender, EventArgs e) {
      var token = txtToken.Text.Trim();

      if (string.IsNullOrEmpty(token)) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = "Token cannot be empty.";
        return;
      }

      try {
        Env.Load(envFilePath);
        _botClient = new TelegramBotClient(token);

        // Reinitialize commands and handlers with the actual bot client
        _controlCommand = new ControlCommand(_botClient, _accessControl);
        _requestAccessCommand = new RequestAccessCommand(_botClient, _accessControl);
        _screenshotCommand = new ScreenshotCommand(_botClient, _accessControl);
        _uploadCommand = new UploadCommand(_botClient, _accessControl);
        _cancelShutdownCommand = new CancelShutdownCommand(_botClient, _accessControl);
        _statsCommand = new StatsCommand(_botClient, _accessControl);
        _getIpCommand = new GetIpCommand(_botClient, _accessControl);
        _secretCommand = new SecretCommand(_botClient, _accessControl);
        _callbackHandler = new CallbackHandler(_botClient, _accessControl);
        _approveAccessHandler = new ApproveAccessHandler(_botClient, _accessControl);

        _cancellationTokenSource = new CancellationTokenSource();

        _botTask = Task.Run(() => StartBotAsync(_cancellationTokenSource.Token));

        btnStartBot.Enabled = false;
        btnStopBot.Enabled = true;
        lblStatus.ForeColor = System.Drawing.Color.Green;
        lblStatus.Text = "Bot is running.";
        SaveBotState(btnStopBot.Enabled);
      }
      catch (Exception ex) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = $"Error starting bot: {ex.Message}";
        Logger.Error(ex, "Error starting bot.");
      }
    }

    private async Task StartBotAsync(CancellationToken cancellationToken) {
      try {
        await _botClient.SetMyCommands(new[] {
          new Telegram.Bot.Types.BotCommand { Command = "request_access", Description = "Request access to the bot" },
          new Telegram.Bot.Types.BotCommand
            { Command = "control", Description = "Control media playback and take screenshots" },
          new Telegram.Bot.Types.BotCommand { Command = "screenshot", Description = "Capture a screenshot" },
          new Telegram.Bot.Types.BotCommand
            { Command = "upload", Description = "Upload a file from the server: /upload <path>" },
          new Telegram.Bot.Types.BotCommand
            { Command = "cancel_shutdown", Description = "Cancel a scheduled shutdown" },
          new Telegram.Bot.Types.BotCommand { Command = "stats", Description = "Show system stats and top processes" },
          new Telegram.Bot.Types.BotCommand { Command = "getip", Description = "Get the server’s public IP" },
          new Telegram.Bot.Types.BotCommand
            { Command = "secret", Description = "A protected command for allowed users and admin" }
        });

        _botClient.StartReceiving(
          HandleUpdateAsync,
          HandleErrorAsync,
          new Telegram.Bot.Polling.ReceiverOptions {
            AllowedUpdates = Array.Empty<UpdateType>() // Receive all update types
          },
          cancellationToken
        );

        Logger.Info("Bot started.");
      }
      catch (Exception ex) {
        Logger.Error(ex, "Error during bot startup.");
      }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
      CancellationToken cancellationToken) {
      try {
        // Middleware: First user becomes admin
        await _firstUserAdminMiddleware.InvokeAsync(update, botClient);

        // Middleware: Access control
        await _accessMiddleware.InvokeAsync(update, botClient);

        // Handle different update types
        if (update.Type == UpdateType.Message && update.Message != null && update.Message.Type == MessageType.Text) {
          var message = update.Message;
          if (message.Text == null) return;
          var command = message.Text.Split(' ')[0].ToLower();

          switch (command) {
            case "/control":
              await _controlCommand.ExecuteAsync(message);
              break;
            case "/request_access":
              await _requestAccessCommand.ExecuteAsync(message);
              break;
            case "/screenshot":
              await _screenshotCommand.ExecuteAsync(message);
              break;
            case "/upload":
              await _uploadCommand.ExecuteAsync(message);
              break;
            case "/cancel_shutdown":
              await _cancelShutdownCommand.ExecuteAsync(message);
              break;
            case "/stats":
              await _statsCommand.ExecuteAsync(message);
              break;
            case "/getip":
              await _getIpCommand.ExecuteAsync(message);
              break;
            case "/secret":
              await _secretCommand.ExecuteAsync(message);
              break;
            default:
              await botClient.SendMessage(message.Chat.Id, "Unknown command.");
              break;
          }
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null) {
          await _callbackHandler.HandleCallbackAsync(update.CallbackQuery);
        }
      }
      catch (Exception ex) {
        Logger.Error(ex, "Error handling update.");
      }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
      CancellationToken cancellationToken) {
      Logger.Error(exception, $"Telegram Bot Error: {exception.Message}");
      return Task.CompletedTask;
    }

    private void btnStopBot_Click(object sender, EventArgs e) {
      try {
        _cancellationTokenSource.Cancel();
        _botTask.Wait();
        btnStartBot.Enabled = true;
        btnStopBot.Enabled = false;
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = "Bot has been stopped.";
        Logger.Info("Bot has been stopped.");
      }
      catch (Exception ex) {
        lblStatus.ForeColor = System.Drawing.Color.Red;
        lblStatus.Text = $"Error stopping bot: {ex.Message}";
        Logger.Error(ex, "Error stopping bot.");
      }
    }

    private void ChkStartWithWindows_CheckedChanged(object sender, EventArgs e) {
      if (chkStartWithWindows.Checked) {
        SetStartup(true);
      }
      else {
        SetStartup(false);
      }
    }

    private void SetStartup(bool enable) {
      try {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryRunPath, true)) {
          if (enable) {
            key.SetValue("PcRobotTelegramBot", Application.ExecutablePath);
          }
          else {
            key.DeleteValue("PcRobotTelegramBot", false);
          }
        }
      }
      catch (Exception ex) {
        MessageBox.Show($"Error updating startup setting: {ex.Message}");
      }
    }

    private bool IsStartupEnabled() {
      using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryRunPath)) {
        return key?.GetValue("PcRobotTelegramBot") != null;
      }
    }

    private void Form1_Resize(object sender, EventArgs e) {
      if (WindowState == FormWindowState.Minimized) {
        Hide();
        notifyIcon.Visible = true;
      }
    }

    private void NotifyIcon_DoubleClick(object sender, EventArgs e) {
      RestoreWindow();
    }

    private void MenuRestore_Click(object sender, EventArgs e) {
      RestoreWindow();
    }

    private void MenuExit_Click(object sender, EventArgs e) {
      notifyIcon.Visible = false;
      Application.Exit();
    }

    private void RestoreWindow() {
      Show();
      WindowState = FormWindowState.Normal;
      notifyIcon.Visible = false;
    }

    private void LinkCredit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
        FileName = "https://github.com/monokaijs/pc-robot-telegram-bot-csharp",
        UseShellExecute = true
      });
    }

    private void SaveBotState(bool isRunning) {
      try {
        System.IO.File.WriteAllText(StateFilePath, isRunning ? "started" : "stopped");
      }
      catch (Exception ex) {
        MessageBox.Show($"Error saving bot state: {ex.Message}");
      }
    }

    private void RestoreBotState() {
      try {
        if (System.IO.File.Exists(StateFilePath)) {
          string state = System.IO.File.ReadAllText(StateFilePath).Trim().ToLower();
          if (state == "started") {
            btnStartBot_Click(null, EventArgs.Empty);
          }
        }
      }
      catch (Exception ex) {
        MessageBox.Show($"Error restoring bot state: {ex.Message}");
      }
    }
  }
}