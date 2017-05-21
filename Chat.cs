using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;
using System.Timers;
using System.IO;

namespace IcaroRPG
{
    public class Chat : Script
    {

        public Chat()
        {
            API.onChatMessage += OnPlayerChat;
        }
        
        public void sendCloseMessage(Client player, float radius, string sender, string msg)
        {
            List<Client> nearPlayers = API.getPlayersInRadiusOfPlayer(radius, player);
           nearPlayers.Remove(player);
            foreach (Client target in nearPlayers)
            {
                API.sendChatMessageToPlayer(player, sender, msg);
            }
        }
        public void OnPlayerChat(Client player, string message, CancelEventArgs e)
        {
            sendCloseMessage(player, 15.0f, "~#ffffff~",player.name + " says: " + message);
            e.Cancel = true;
            return;
        }
        [Command("coords")]
        public void coords(Client player, string coordName)
        {
            Vector3 playerPosGet = API.getEntityPosition(player);
            var pPosX = (playerPosGet.X.ToString().Replace(',', '.') + ", ");
            var pPosY = (playerPosGet.Y.ToString().Replace(',', '.') + ", ");
            var pPosZ = (playerPosGet.Z.ToString().Replace(',', '.'));
            Vector3 playerRotGet = API.getEntityRotation(player);
            var pRotX = (playerRotGet.X.ToString().Replace(',', '.') + ", ");
            var pRotY = (playerRotGet.Y.ToString().Replace(',', '.') + ", ");
            var pRotZ = (playerRotGet.Z.ToString().Replace(',', '.'));

            API.sendChatMessageToPlayer(player, "Your position is: ~y~" + playerPosGet, "~w~Your rotation is: ~y~" + playerRotGet);
            StreamWriter coordsFile;
            if (!File.Exists("SavedCoords.txt"))
            {
                coordsFile = new StreamWriter("SavedCoords.txt");
            }
            else
            {
                coordsFile = File.AppendText("SavedCoords.txt");
            }
            API.sendChatMessageToPlayer(player, "~r~Coordinates have been saved!");
            coordsFile.WriteLine("| " + coordName + " | " + "Saved Coordenates: " + pPosX + pPosY + pPosZ + " Saved Rotation: " + pRotX + pRotY + pRotZ);
            coordsFile.Close();
        }
        [Command("savedcoords")]
        public void savedCoords(Client player)
        {
            API.sendChatMessageToPlayer(player, "~r~Current Saved Coordenates:");
            int counter = 0;
            string coordsLine;
            System.IO.StreamReader file = new System.IO.StreamReader("SavedCoords.txt");
            while ((coordsLine = file.ReadLine()) != null)
            {
                API.sendChatMessageToPlayer(player, coordsLine);
                counter++;
            }
            file.Close();
        }

        [Command("me", GreedyArg = true)]
        public void Command_me(Client sender, string message)
        {
            TextLabel txt = API.createTextLabel(sender.name + " " + message + ".", sender.position, 15, 0.7f);
            API.setTextLabelColor(txt, 220, 220, 220, 200);
            API.attachEntityToEntity(txt, sender, "SKEL_HEAD", new Vector3(0, 0, 0.7), new Vector3(0, 0, 0));
            API.delay(6000, true, () => { txt.delete(); });
        }
        [Command("do", GreedyArg = true)]
        public void Command_do(Client sender, string message)
        {
            TextLabel txt = API.createTextLabel("( " + message + ". ) ", sender.position, 15, 0.7f);
            API.setTextLabelColor(txt, 220, 220, 220, 200);
            API.attachEntityToEntity(txt, sender, "SKEL_HEAD", new Vector3(0, 0, 0.7), new Vector3(0, 0, 0));
            API.setEntityData(txt, "Showtxt", true);
            API.delay(6000, true, () => { txt.delete(); });
        }

        [Command("o", GreedyArg = true)]
        public void Command_o(Client sender, string message)
        {
            API.sendChatMessageToAll(sender.name, sender.name + ": ((" + message + "))");
        }


    }

}
