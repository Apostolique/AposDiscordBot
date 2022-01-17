using System;
using Discord;
using Discord.WebSocket;

namespace AposDiscordBot {
    public class Program {
        public static Task Main(string[] args) => new Program().MainAsync();

        public Program() {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += HandleMessageAsync;
            _client.UserUpdated += HandleUserUpdateAsync;
        }

        public async Task MainAsync() {
            await _client.LoginAsync(TokenType.Bot, Settings.Token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task ReadyAsync() {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            foreach (var e in _client.Guilds) {
                Console.WriteLine(e.Id);
            }

            return Task.CompletedTask;
        }

        private async Task HandleUserUpdateAsync(SocketUser a, SocketUser b) {
            Console.WriteLine("Test");
        }

        private async Task HandleMessageAsync(SocketMessage message) {
            if (message.Author.Id == _client.CurrentUser.Id || message is not SocketUserMessage) return;

            var textChannel = message.Channel as SocketTextChannel;
            if (textChannel == null || textChannel is SocketThreadChannel) return;

            string nick = message.Author.Username;
            var user = message.Author as IGuildUser;
            if (user != null && user.Nickname != null) nick = user.Nickname;

            var thread = await textChannel.CreateThreadAsync($"{nick} showcase", message: message);

            await thread.SendMessageAsync("pong!");
        }

        private Task Log(LogMessage msg) {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private DiscordSocketClient _client;
    }
}
