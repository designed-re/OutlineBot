using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Serilog;

namespace OutlineBot
{
    public class DiscordEventHandler
    {
        private ILogger _logger;

        public DiscordEventHandler()
        {
            _logger = Serilog.Log.Logger.ForContext<DiscordEventHandler>();
        }

        public Task Log(LogMessage arg)
        {
            _logger.Information("{0} {1} {2}", arg.Source, arg.Severity, arg.Message);
            return Task.CompletedTask;
        }

        public async Task Ready()
        {
            try
            {
                if (Program.Configuration.Debug)
                {
                    foreach (var guild in Program.Configuration.DebugGuilds)
                    {
                        _logger.Information("Registering commands to {0}", guild);
                        await Program.InteractionService.AddModulesToGuildAsync(guild, true);
                        await Program.InteractionService.RegisterCommandsToGuildAsync(guild);
                    }
                }
                else if (Program.UpdateCommands)
                {
                    _logger.Information("Registering commands globally");
                    await Program.InteractionService.AddCommandsGloballyAsync(true);
                    await Program.InteractionService.RegisterCommandsGloballyAsync();
                }

                _logger.Information("Ready");
            }
            catch (Exception e)
            {
                _logger.Fatal(e.ToString());
            }
        }
    }
}
