using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord.Utils;

namespace OutlineBot.Modules
{
    class TestModule : InteractionModuleBase
    {
        private OutlineManager _manager;

        public TestModule(OutlineManager manager)
        {
            _manager = manager;
        }

        [SlashCommand("test", "ㅇㅇㅇㅇ")]
        public async Task Test()
        {
            await DeferAsync();
            await ModifyOriginalResponseAsync(x => x.Content = "ㅎㅇ");
        }

        [SlashCommand("haskey", "Check if user already has key")]
        public async Task TestHasKey()
        {
            await DeferAsync();
            var key = _manager.IsKeyExists(Context.User.Id.ToString());
            await ModifyOriginalResponseAsync(x => x.Content = key.ToString());
        }

        [SlashCommand("getkey", "Get the private key")]
        public async Task TestGetKey()
        {
            await DeferAsync();
            var key = _manager.GetOrNewKey(Context.User.Id.ToString());
            await ModifyOriginalResponseAsync(x => x.Content = $"```{key.AccessUrl}```");
        }

        // [SlashCommand("reset", "Reset the data limit")]
        public async Task TestReset()
        {
            await DeferAsync();
            _manager.ResetDataLimit(Context.User.Id.ToString());
            await ModifyOriginalResponseAsync(x => x.Content = "ok");
        }

        [SlashCommand("revoke", "Revoke the private key")]
        public async Task TestRevoke()
        {
            await DeferAsync();
            _manager.RevokeKey(Context.User.Id.ToString());
            await ModifyOriginalResponseAsync(x => x.Content = "ok");
        }

        [SlashCommand("stats", "Get the stats of key")]
        public async Task TestStats()
        {
            await DeferAsync();
            var key = _manager.GetUsedBytes(Context.User.Id.ToString());
            await ModifyOriginalResponseAsync(x => x.Content = $"{(key == null ? 0 : key.UsedBytes)} bytes used.");
        }
    }
}
