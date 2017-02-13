using System;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;


// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"I haven't been programmed to understand that, sorry."); //
        context.Wait(MessageReceived);
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    static List<String> chitChat = new List<String>(){
        "I'm not good at small talk: I'm a bot, y'know?",
        "If you want to talk to real humans you might want to join our [Telegram group](https://t.me/joinchat/AAAAAD67JwNoT7jDq_xfZg)"
    };
    [LuisIntent("Chitchat")]
    public async Task Chitchat(IDialogContext context, LuisResult result)
    {
        Random random = new Random();
        await context.PostAsync(chitChat[random.Next(chitChat.Count)]); //
        context.Wait(MessageReceived);
    }
    [LuisIntent("Unirse al club")]
    public async Task JoinClub(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"If you want to join the club, just fill out [this form](https://onedrive.live.com/survey?resid=C54C5685052E8FDD!234&authkey=!ACoCcPb0M17eQTQ)");
        await context.PostAsync($"You will be added to a mailing list so you can keep up with everything that happens!");
        await context.PostAsync($"Once you do it, you should also consider joining our [Telegram group](https://t.me/joinchat/AAAAAD67JwNoT7jDq_xfZg) and following us on [Twitter](https://twitter.com/uamnethttps://twitter.com/uamnet).");
        context.Wait(MessageReceived);
    }
    [LuisIntent("Go to Github")]
    public async Task GoGithub(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"All our code demos are on our Github account.");
        await context.PostAsync("[You can find them here](https://github.com/UamNet)");
        context.Wait(MessageReceived);
    }
    [LuisIntent("Join Telegram")]
    public async Task GoTelegram(IDialogContext context, LuisResult result)
    {
        await context.PostAsync("We have a Telegram group to talk about stuff!");
        await context.PostAsync("[Click here to join](https://t.me/joinchat/AAAAAD67JwNoT7jDq_xfZg)");
        context.Wait(MessageReceived);
    } 
    [LuisIntent("Join Dreamspark")]
    public async Task JoinDreamspark(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"If you are studying in the EPS, you can ask for a Dreamspark account, that allows you to get free software such as Windows, Visual Studio...");
        await context.PostAsync($"Just fill out [this form](https://onedrive.live.com/survey?resid=C54C5685052E8FDD!236&authkey=!ABZw3EPirQI2iaw) and wait a few days, your credentials will arrive to your mail.");
        await context.PostAsync("Then, just go to [http://dreamspark.uam.es/](http://dreamspark.uam.es/) and download whatever you need!");
        context.Wait(MessageReceived);
    }
    [LuisIntent("Encontrar eventos")]
    public async Task FindEvents(IDialogContext context, LuisResult result)
    {
        await context.PostAsync("wip");
        EntityRecommendation entityName;
        if(result.TryFindEntity("Nombre", out entityName)){
            await context.PostAsync(entityName.Entity);
        }else{
            await context.PostAsync(" no name");
        }
        context.Wait(MessageReceived);
    }
}