// See https://aka.ms/new-console-template for more information
using RiotSharp;
var api = RiotApi.GetDevelopmentInstance("RGAPI-ba015301-ad96-4198-ab56-b2ad78db0718");
var matchStart = 0;
var matchEnd = 0;
Console.WriteLine("Please provide your Summoner Name");
string userName = Console.ReadLine();
Console.WriteLine("how many matches would you like to see");
matchEnd = Convert.ToInt32(Console.ReadLine());

try
{
    var summoner = api.Summoner.GetSummonerByNameAsync(RiotSharp.Misc.Region.Euw,userName).Result;
    var name = summoner.Name;
    var level = summoner.Level;
    var accountId = summoner.AccountId;
    var puuId = summoner.Puuid;
    
    
    //NOT WORKING ON OTHER PC'S FOR SOME REASON
    var matchlist = api.Match.GetMatchListAsync(RiotSharp.Misc.Region.Europe, puuId, matchStart, matchEnd, 420).Result;
    Console.WriteLine("============================================");
    Console.WriteLine("Summoner Info");
    Console.WriteLine("============================================");
    Console.WriteLine("Summoner Name : " + name);
    Console.WriteLine("Account ID    : " + accountId);
    Console.WriteLine("PUUID         : " + puuId);
    Console.WriteLine("Summoner Level: " + level);
    Console.WriteLine("============================================");
    foreach (var match in matchlist)
    {
        var matchdetails = api.Match.GetMatchAsync(RiotSharp.Misc.Region.Europe, match).Result;
        var gamedata = matchdetails.Info.GameDuration;
        var sDate = matchdetails.Info.GameCreation.ToShortDateString();
        //string sDate = mDate;
        var summonerdata = matchdetails.Info.Participants.FirstOrDefault(p => p.Puuid == puuId);
        var sLane = summonerdata.IndividualPosition;
        decimal sKills = summonerdata.Kills;
        decimal sDeaths = summonerdata.Deaths;
        decimal sAssists = summonerdata.Assists;
        var sDuration = summonerdata.timePlayed;
        var sCS = summonerdata.NeutralMinionsKilled + summonerdata.TotalMinionsKilled;
        var sChampion = summonerdata.ChampionName;
        var sResult = "";
        decimal sKDA = 0;
        if (sDeaths == 0)
        {
            sKDA = Math.Round((sKills + sAssists),2);
        } else
        {
            sKDA = Math.Round((sKills + sAssists) / sDeaths,2);
        }

        if (summonerdata.Winner == true)
        {
            sResult = "Victory";
        } else
        {
            sResult = "Defeat";
        }

        Console.WriteLine("============================================");
        Console.WriteLine("Match ID: " + match);
        Console.WriteLine("============================================");
        Console.WriteLine("Date         : " + sDate);
        Console.WriteLine("Result       : " + sResult);
        Console.WriteLine("Game Duration: " + sDuration);
        Console.WriteLine("Lane         : " + sLane);
        Console.WriteLine("Champion     : " + sChampion);
        Console.WriteLine("K/D/A        : " + sKills + "/" + sDeaths + "/" + sAssists);
        Console.WriteLine("KDA          : " + String.Format("{0:0.00}", sKDA));    
        Console.WriteLine("Total CS     : " + sCS);
        Console.WriteLine("============================================");




    }
}
catch (RiotSharpException ex)
{
    Console.WriteLine(ex.Message);
    // Handle the exception however you want.
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex);
}
