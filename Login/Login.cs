using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace IcaroRPG
{
    public class Login : Script
    {
        public class PrimaryGroup
        {
            public int id { get; set; }
            public string name { get; set; }
            public string formattedName { get; set; }
        }

        public class __invalid_type__3
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        public class Fields
        {
            public __invalid_type__3 __invalid_name__3 { get; set; }
        }

        public class __invalid_type__1
        {
            public string name { get; set; }
            public Fields fields { get; set; }
        }

        public class CustomFields
        {
            public __invalid_type__1 __invalid_name__1 { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string timeZone { get; set; }
            public string formattedName { get; set; }
            public PrimaryGroup primaryGroup { get; set; }
            public List<object> secondaryGroups { get; set; }
            public string email { get; set; }
            public string joined { get; set; }
            public string registrationIpAddress { get; set; }
            public int warningPoints { get; set; }
            public int reputationPoints { get; set; }
            public string photoUrl { get; set; }
            public string profileUrl { get; set; }
            public bool validating { get; set; }
            public int posts { get; set; }
            public string lastActivity { get; set; }
            public string lastVisit { get; set; }
            public string lastPost { get; set; }
            public int profileViews { get; set; }
            public object birthday { get; set; }
            public CustomFields customFields { get; set; }
        }

        public class RootObject
        {
            public int page { get; set; }
            public int perPage { get; set; }
            public int totalResults { get; set; }
            public int totalPages { get; set; }
            public List<Result> results { get; set; }
        }
        public void apiGetMembers(Client sender)
        {
            string URL = "http://nordicrp.net/forums/api/index.php?/core/members&key=6cbeaaea52c8d7df2f9e2aacdff1a324";
            string urlParameters = "";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                Result result2 = new Result();
                JsonConvert.PopulateObject(result, result2);
                API.sendChatMessageToPlayer(sender, result2.name);
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "Error connecting!");
            }
        }
        public void apiLogin(Client sender)
        {
            string URL = "http://nordicrp.net/forums/api/index.php?/core/members/1&key=6cbeaaea52c8d7df2f9e2aacdff1a324";
            string urlParameters = "";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                Result result2 = new Result();
                JsonConvert.PopulateObject(result, result2);
                API.sendChatMessageToPlayer(sender, result2.name);
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "Error connecting!");
            }
        }

        public Login()
        {
            Database.Init();
            API.onClientEventTrigger += OnClientEvent;
        }
        public void OnClientEvent(Client sender, string eventName, params object[] args)
        {
            if (eventName == "PlayerLogin")
            {

                if (!Database.IsPlayerLoggedIn(sender))
                {
                    if (!Database.TryLoginPlayer(sender, args[0].ToString()))
                    {
                        if (Database.DoesAccountExist(sender.socialClubName))
                        {
                            API.triggerClientEvent(sender, "display_subtitle", "~r~ERROR:~w~ Wrong password.", 3000);
                            API.triggerClientEvent(sender, "displaylogin");
                            return;
                        }
                    }
                    else
                    {
                        API.sendNativeToPlayer(sender, Hash.DO_SCREEN_FADE_IN, 4000);
                        Database.LoadPlayerAccount(sender);
                        API.sendChatMessageToPlayer(sender, "~g~Logged in successfully!");
                        API.call("SpawnManager", "SpawnCitizen", sender);
                        int money = API.getEntityData(sender, "Money");
                        API.triggerClientEvent(sender, "update_money_display", money);
                        apiLogin(sender);
                    }
                }
            }
            if (eventName == "PlayerRegister")
            {
                if (Database.IsPlayerLoggedIn(sender))
                {
                    API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
                    return;
                }

                if (Database.DoesAccountExist(sender.socialClubName))
                {
                    API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~An account linked to this Social Club handle already exists!");
                    return;
                }

                Database.CreatePlayerAccount(sender, args[0].ToString());
                Database.LoadPlayerAccount(sender);
                API.sendChatMessageToPlayer(sender, "~g~Logged in successfully!");
                // API.call("SpawnManager", "SpawnCitizen", sender);
                int money = API.getEntityData(sender, "Money");
                API.triggerClientEvent(sender, "update_money_display", money);
                API.call("CharacterCreator", "startCreator", sender);
            }

        }
        public void onResourceStop()
        {
            foreach (var client in API.getAllPlayers())
            {
                if (Database.IsPlayerLoggedIn(client))
                {
                    ConnectionManager.Leave(client);
                    // Database.SavePlayerAccount(client);
                }
                foreach (var data in API.getAllEntityData(client))
                {
                    API.resetEntityData(client, data);
                }
            }
        }
    }
}