#r "Newtonsoft.Json"

using System;
using System.Threading.Tasks;
using System.Net.Http;

using System.Linq;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using Newtonsoft.Json;


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
        await context.PostAsync("Let's see what we have...");
        //Call the API from the web to retrieve the events
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("http://uamnet.azurewebsites.net/api/events");
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        List<Event> events = JsonConvert.DeserializeObject<List<Event>>(content);
        EntityRecommendation entityName;
        if (result.TryFindEntity("Nombre", out entityName))
        {
            await sendEvents(context, (from ev in events where ev.day != "" && ev.title.ToLower().Contains(entityName.Entity.ToLower()) select ev));
        }
        else
        {
            await sendEvents(context, (from ev in events where ev.day != "" select ev));
        }
        context.Wait(MessageReceived);
    }

    async Task sendEvents(IDialogContext context, IEnumerable<Event> events)
    {
        foreach (Event e in events)
        {
            await context.PostAsync(e.ToString());
        }
    }

    public class Event
    {
        public string title { get; set; }
        public string by { get; set; }
        public string place { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string time { get; set; }
        public string color { get; set; }
        public string id { get; set; }
        override public String ToString()
        {
            return $"'{title}' by {by} at {place}, {day}-{month}-{year} at {time}";
        }
    }
}