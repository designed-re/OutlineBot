using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignedUtils;
using Discord;
using Discord.Interactions;
using Serilog;

namespace OutlineBot.Modules
{
    public class OutlineModule : InteractionModuleBase
    {
        private readonly OutlineManager _manager;
        private readonly ILogger _logger;
        public OutlineModule(OutlineManager manager)
        {
            _manager = manager;
            _logger = this.ContextLogger();
        }

        [SlashCommand("client", "Outline 다운로드")]
        public async Task OutlineClient()
        {
            try
            {
                await DeferAsync();

                var embed = EmbedUtils.Instance.GetDefaultBuilder("Outline Client");
                embed.AddField("Android",
                    "[Android](https://play.google.com/store/apps/details?id=org.outline.android.client)", true);
                embed.AddField("Windows",
                    "[Windows](https://s3.amazonaws.com/outline-releases/client/windows/stable/Outline-Client.exe)", //change this to github url if censored
                    true);
                embed.AddField("MacOS",
                    "[MacOS](https://itunes.apple.com/us/app/outline-app/id1356178125)", true);
                embed.AddField("Linux",
                    "[Linux](https://s3.amazonaws.com/outline-releases/client/linux/stable/Outline-Client.AppImage)",
                    true);
                await ModifyOriginalResponseAsync(x=> x.Embed= embed.Build());
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());   
            }
        }

        [SlashCommand("getkey", "Outline Key 확인")]
        public async Task GetKey([Summary("server"), Autocomplete(typeof(ServerAutocompleteHandler))] string serverSelection)
        {
            await DeferAsync(true);

            var embed = EmbedUtils.Instance.GetDefaultBuilder("Outline Key");
            embed.ContextLogger().Information("test");


            await ModifyOriginalResponseAsync(x => x.Embed = embed.Build());

        }







        public class ServerAutocompleteHandler : AutocompleteHandler
        {
            public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
            {
                IEnumerable<AutocompleteResult> results = new[]
                {
                    new AutocompleteResult("Korea", "korea"),
                };

                // max - 25 suggestions at a time (API limit)
                return AutocompletionResult.FromSuccess(results.Take(25));
            }
        }
    }
}
