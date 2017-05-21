using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;

namespace IcaroRPG
{
    public class Login : Script
    {
        public Login()
        {
            Database.Init();
            API.onClientEventTrigger += OnClientEvent;
        }
        public void OnClientEvent(Client sender, string eventName, params object[] args)
        {
            if (eventName == "PlayerLogin")
            {
                API.consoleOutput("Password:" + args[0].ToString());

                if (!Database.IsPlayerLoggedIn(sender))
                {
                    if (!Database.TryLoginPlayer(sender, args[0].ToString()))
                    {
                        if (Database.DoesAccountExist(sender.socialClubName))
                        {
                            API.triggerClientEvent(sender, "showLogin");
                            API.triggerClientEvent(sender, "display_subtitle", "~r~ERROR:~w~ Wrong password.", 3000);
                            API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ Wrong password.");
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
                    }
                }
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