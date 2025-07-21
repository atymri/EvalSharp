# EvalSharp

**EvalSharp** is a Telegram bot written in C# that evaluates and executes C# code snippets sent by users in real time. It uses a sandboxed scripting engine and provides responses directly in the Telegram chat.

---

## Features

* Execute C# scripts safely within Telegram
* Async bot lifecycle management
* Simple, minimal setup
* Extendable for additional command handling or sandbox logic

---

## Requirements

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* A valid [Telegram bot token](https://t.me/BotFather)

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/atymri/EvalSharp.git
cd EvalSharp
```

### 2. Set Up Your Bot Token

Open `Program.cs` and replace the following line with your actual token:

```csharp
string token = "YOUR_BOT_TOKEN_HERE";
```

### 3. Build and Run

```bash
dotnet build
dotnet run
```

You should see:

```
Bot started. Press Enter to exit.
```

---

## Docker Support (Optional)

To run EvalSharp in a container:

### Build the Docker Image

```bash
docker build -t evalsharp .
```

### Run the Bot

```bash
docker run -e TELEGRAM_BOT_TOKEN=your_token_here evalsharp
```

> If you're using Docker, make sure `Program.cs` is modified to read the token from an environment variable:

```csharp
string token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
```

---

## Usage

1. Start a chat with your bot on Telegram.
2. Send a C# code block like:

````
/exec Console.WriteLine("2 + 2 = " + (2 + 2));
````
3. The bot will evaluate the code and reply with the output.
---

## Development

- Main entry point: `Program.cs`
- Bot logic is encapsulated in `EvalSharp.Bot.Bot`
- Extend with custom command parsers, security limits, or logging

To contribute:

1. Fork and clone the repo
2. Create a new branch
3. Commit changes with meaningful messages
4. Open a pull request

---
## License
This project is licensed under the **GNU AGPLv3**. See [LICENSE](LICENSE) for details.
