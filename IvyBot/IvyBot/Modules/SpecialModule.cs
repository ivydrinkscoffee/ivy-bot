using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text.Json;
using IvyBot.Services;

namespace IvyBot.Modules
{
    [Name("Special")]
    public class SpecialModule : ModuleBase<SocketCommandContext>
    {
        [Command("longelmo")]
        [Summary("This elmo is long")]
        public async Task LongElmoAsync()
        {
            var filestream = WebRequest.Create("https://cdn.discordapp.com/attachments/762004355888185365/767022764916736020/video0.mp4");
            Stream stream = filestream.GetResponse().GetResponseStream();
            await Context.Channel.SendFileAsync(stream, "superstrong.mp4", "https://twitter.com/intent/tweet?hashtags=LongElmo2020%2CMakeAmericaLongAgain");
        }

        public class AsmCommandContext
        {
            public Hex hex { get; set; }
            public int counter { get; set; }
        }
        public class Hex
        {
            public string arm64 { get; set; }
        }

        public class DisasmCommandContext
        {
            public Asm asm { get; set; }
            public int counter { get; set; }
        }
        public class Asm
        {
            public string arm64 { get; set; }
        }

        [Command("asm")]
        [Summary("Converts ARM64 assembly code to hex code")]
        public Task AssembleAsync([Remainder] string assembly)
        {  
            try
            {
                var client = new WebClient();
                
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string json = @"{""asm"":""" + $"{assembly}" + @"""," + @"""offset"":""0x100"",""arch"":""arm64""}";
                
                string result = client.UploadString("https://armconverter.com/api/convert", "POST", json);
                var resultSubString = result.Substring(18, 7);
                var finalResult = result.Replace(resultSubString, " ").Replace("]", " ");
                
                string hex = JsonSerializer.Deserialize<AsmCommandContext>(finalResult).hex.arm64.Replace("### ", " ");
                
                return ReplyAsync(hex);
            }
            catch (Exception)
            {
                return ReplyAsync("Invalid assembly code");
            }
        }

        [Command("disasm")]
        [Summary("Converts ARM64 hex code to assembly code")]
        public Task DisassembleAsync([Remainder] string hex)
        {
            try
            {
                var client = new WebClient();
                
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string json = @"{""hex"":""" + $"{hex}" + @"""," + @"""offset"":""0x100"",""arch"":""arm64""}";
                
                string result = client.UploadString("https://armconverter.com/api/convert", "POST", json);
                var resultSubString = result.Substring(18, 7);
                var finalResult = result.Replace(resultSubString, " ").Replace("]", " ");

                string asm = JsonSerializer.Deserialize<DisasmCommandContext>(finalResult).asm.arm64.Replace("### ", " ");
                
                return ReplyAsync(asm);
            }
            catch (Exception)
            {
                return ReplyAsync("Invalid hex code");
            }
        }
        
        [Command("pchtxt")]
        [Summary("Sends the specified pchtxt for Splatoon 2")]
        public async Task SendPatchesAsync([Remainder] string version)
        {
            IEnumerable<string> versionList = new List<string>() { "5.0.0", "5.0.1", "5.1.0", "5.2.0", "5.2.1", "5.2.2", "5.3.0", "5.3.1" };

            if (StringService.EqualsAny(version, versionList) == false)
            {
                await ReplyAsync("<:xmark:314349398824058880> Game version not supported");
            }
            else
            {
                var filestream = WebRequest.Create($"https://splatoon-hackers.github.io/{version}public.pchtxt");
                Stream stream = filestream.GetResponse().GetResponseStream();
                await Context.Channel.SendFileAsync(stream, $"{version}public.pchtxt");
            }
        }

        [Command("starlion")]
        [Summary("Sends the latest public Starlion for Splatoon 2")]
        public async Task SendStarlionAsync()
        {
            var filestream = WebRequest.Create("https://splatoon-hackers.github.io/starlion_public.rar");
            Stream stream = filestream.GetResponse().GetResponseStream();
            await Context.Channel.SendFileAsync(stream, "starlion_public.rar");
        }
    }
}
