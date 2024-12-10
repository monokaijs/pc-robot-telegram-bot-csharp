# PC Robot Telegram Bot

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/monokaijs/pc-robot-telegram-bot-csharp/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/monokaijs/pc-robot-telegram-bot-csharp/actions)

A Windows application that lets you control your PC remotely using Telegram bot commands. It provides functionality to schedule tasks, manage system resources, and interact with your computer through a Telegram interface.

## Features

- **Access Control**: 
  The bot requires admin approval for new users, ensuring only authorized access.
  
- **Media Control**: 
  Play, pause, mute, or skip media tracks remotely.

- **Screenshot**: 
  Capture the current screen and receive the screenshot in Telegram.

- **System Management**:
  - Schedule shutdowns.
  - Cancel shutdowns.
  - Kill specific processes.
  - Fetch system stats like CPU, memory usage, and top-consuming processes.

- **File Upload**: 
  Upload files from your PC to Telegram.

- **Custom Bot Token Configuration**:
  Configure your bot token via a user-friendly Windows GUI.

- **Start with Windows**:
  The bot can be set to start automatically with your operating system.

---

## Requirements

- **Operating System**: Windows 10 or later
- **.NET Runtime**: .NET 6.0 or later
- **Telegram Bot Token**: Obtain a bot token from [BotFather](https://t.me/botfather).

---

## Installation

### 1. Download the Application
- Visit the [Releases](https://github.com/monokaijs/pc-robot-telegram-bot-csharp/releases) section of this repository and download the latest `.exe` file.

### 2. Configure the Bot
1. Run the application.
2. Enter your bot token in the GUI.
3. Save the configuration.

### 3. Start the Bot
Click **Start Bot** in the GUI to launch the bot. Optionally, enable **Start with Windows** to ensure the bot launches automatically when the system boots.

---

## Usage

1. Open the application and configure your bot token.
2. Start the bot.
3. Use Telegram commands to interact with your PC.

### Available Commands
| Command               | Description                                   |
|-----------------------|-----------------------------------------------|
| `/request_access`     | Request access to use the bot.               |
| `/control`            | Show media controls and screenshot options.  |
| `/screenshot`         | Capture and send a screenshot.               |
| `/upload <path>`      | Upload a specific file from your PC.          |
| `/cancel_shutdown`    | Cancel any scheduled shutdown.               |
| `/stats`              | View system stats (CPU, RAM, processes).     |
| `/getip`              | Retrieve the current public IP address.      |

---

## Development

### Clone the Repository

```bash
git clone https://github.com/monokaijs/pc-robot-telegram-bot-csharp.git
cd pc-robot-telegram-bot-csharp
```

### Build and Run
1. Open the project in [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/).
2. Build the solution.
3. Run the application.

### Publish as a Single `.exe`
To create a single executable file:
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```
The `.exe` file will be located in:
```
bin\Release\net6.0-windows\win-x64\publish
```

---

## GitHub Actions

The project includes a GitHub Actions workflow for automatic builds and releases. On every tagged commit (e.g., `v1.0.0`), the workflow:
1. Builds the application using `dotnet publish`.
2. Uploads the `.exe` to the [Releases](https://github.com/monokaijs/pc-robot-telegram-bot-csharp/releases) section.

---

## Contribution

Contributions are welcome! To contribute:
1. Fork this repository.
2. Create a feature branch (`git checkout -b feature-name`).
3. Commit your changes (`git commit -m "Add feature"`).
4. Push the branch (`git push origin feature-name`).
5. Open a Pull Request.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Credits

Created by [MonokaiJs](https://github.com/monokaijs). Inspired by the need for remote PC control with ease and flexibility.