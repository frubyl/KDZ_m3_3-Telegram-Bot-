# 📦 Telegram CSV/JSON Parser Bot

**A C# Telegram bot that lets users upload CSV/JSON files, filter and sort their contents, and download the processed result — all via chat.**

---

## ✨ Features

- 📁 Upload CSV or JSON files directly in Telegram
- 🔍 Filter data by:
  - `District`
  - `Owner`
  - `AdmArea` and `Owner`
- 📊 Sort data by `TestDate` (ascending or descending)
- 📥 Download the filtered or sorted result
- 🤖 Friendly Telegram menu with keyboard buttons
- 🧠 Automatically manages a user list (up to 50 users)

---

## 🛠 Technologies Used

- **C# / .NET** — core logic and architecture
- **Telegram.Bot** — Telegram API integration
- **Microsoft.Extensions.Logging** — structured logging
- **Custom Libraries** — for user handling and file processing

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/telegram-csv-json-bot.git
   ```

2. Add your Telegram Bot Token in the `Main` method:
   ```csharp
   string token = "YOUR_BOT_TOKEN";
   ```

3. Run the project:
   ```bash
   dotnet run
   ```

4. Start interacting with your bot in Telegram by sending `/start`.

---

## 📂 Project Structure

- `Program.cs` — main logic for receiving updates and handling users
- `UserPart` — custom user and session management
- `AdditionalLibrary` — file processing, filtering, sorting, exporting
- `sticker/` — optional folder for sending welcome stickers

---

## 📄 License

MIT License — feel free to use, modify, and share!


