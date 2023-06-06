using System;
using System.Reflection;
using System.Text;
using DesignedUtils;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using Serilog.Core;
using Serilog.Context;

namespace OutlineBot
{
    public class Program
    {
        public static bool UpdateCommands = false;
        public static Configuration Configuration { get; set; }
        public static DiscordSocketClient Client;
        public static InteractionService InteractionService;
        public static IServiceProvider Services;
        private static ILogger _logger;

        public static async Task Main(string[] args)
        {
            if (args.Any())
            {
                if (args[0] == "register") UpdateCommands = true;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[DESIGNED {Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}.{Method} {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Debug()
                .CreateLogger();

            _logger = Log.Logger.ForContext<Program>();

            EmbedUtils.Instance.SetPrimaryColor(Color.Purple);
            EmbedUtils.Instance.SetFooter($"OutlineBot - v{Assembly.GetEntryAssembly().GetName().Version}");

            _logger.Information("Registering Exception handler");
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                var exception = (Exception) eventArgs.ExceptionObject;
                _logger.Error("{0}", exception.StackTrace);
            };
            
            string confPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            _logger.Information("Loading configuration {0}", confPath);

            Configuration = JsonConvert.DeserializeObject<Configuration>(await File.ReadAllTextAsync(confPath));

            Client = new DiscordSocketClient(
                new DiscordSocketConfig() {});
            var eventHandler = new DiscordEventHandler();

            Client.Log += eventHandler.Log;
            Client.Ready += eventHandler.Ready;

            _logger.Information("Logging in");
            await Client.LoginAsync(TokenType.Bot, Configuration.Token);

            InteractionService = new InteractionService(Client);

            OutlineManager outlineManager = new OutlineManager(Configuration.OutlineManagementUrl);

            Services = new ServiceCollection().AddSingleton(Client).AddSingleton(InteractionService).AddSingleton(outlineManager).BuildServiceProvider();

            await InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), Services);

            _logger.Information("Loaded modules: {0} commands: {1}", InteractionService.Modules.Count, InteractionService.SlashCommands.Count);

            var inter = new InteractionHandler();
            await inter.InitializeAsync();

            await Client.StartAsync();
            await Task.Delay(-1);
        }
    }
}