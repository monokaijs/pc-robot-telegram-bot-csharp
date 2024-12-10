using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PCRobotApp
{
    public partial class Form1 : Form
    {
        private const string RegistryRunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string StateFilePath = "bot_state.txt";

        public Form1()
        {
            InitializeComponent();
            chkStartWithWindows.Checked = IsStartupEnabled();
            RestoreBotState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string token = txtToken.Text.Trim();
            if (string.IsNullOrEmpty(token))
            {
                lblStatus.Text = "Token cannot be empty.";
                return;
            }

            try
            {
                File.WriteAllText("bot_token.txt", token);
                lblStatus.Text = "Token saved successfully.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error saving token: {ex.Message}";
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Testing token functionality is not implemented.";
        }

        private void btnStartBot_Click(object sender, EventArgs e)
        {
            SaveBotState(true);
            lblStatus.Text = "Starting bot functionality is not implemented.";
            btnStartBot.Enabled = false;
            btnStopBot.Enabled = true;
        }

        private void btnStopBot_Click(object sender, EventArgs e)
        {
            SaveBotState(false);
            lblStatus.Text = "Stopping bot functionality is not implemented.";
            btnStartBot.Enabled = true;
            btnStopBot.Enabled = false;
        }

        private void ChkStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartWithWindows.Checked)
            {
                SetStartup(true);
            }
            else
            {
                SetStartup(false);
            }
        }

        private void SetStartup(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryRunPath, true))
                {
                    if (enable)
                    {
                        key.SetValue("PcRobotTelegramBot", Application.ExecutablePath);
                    }
                    else
                    {
                        key.DeleteValue("PcRobotTelegramBot", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating startup setting: {ex.Message}");
            }
        }

        private bool IsStartupEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryRunPath))
            {
                return key?.GetValue("PcRobotTelegramBot") != null;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void MenuRestore_Click(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void RestoreWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void LinkCredit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/monokaijs/pc-robot-telegram-bot-csharp",
                UseShellExecute = true
            });
        }

        private void SaveBotState(bool isRunning)
        {
            try
            {
                File.WriteAllText(StateFilePath, isRunning ? "started" : "stopped");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving bot state: {ex.Message}");
            }
        }

        private void RestoreBotState()
        {
            try
            {
                if (File.Exists(StateFilePath))
                {
                    string state = File.ReadAllText(StateFilePath).Trim().ToLower();
                    if (state == "started")
                    {
                        btnStartBot_Click(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring bot state: {ex.Message}");
            }
        }
    }
}