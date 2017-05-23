using GTANetworkServer;
using GTANetworkShared;

namespace IcaroRPG.Global
{
    class KeyManager : Script
    {
        public KeyManager()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnClientEventTrigger(Client player, string eventName, object[] args)
        {
            if(eventName == "onKeyDown")
            {
                if((int)args[0] == 3)
                {
                    if (player.isInVehicle)
                    {
                        var vehicle = API.getPlayerVehicle(player);
                        if (API.getEntityData(vehicle, "rental") == false)
                        {
                            if (!API.getVehicleEngineStatus(vehicle))
                            {
                                API.setVehicleEngineStatus(vehicle, true);
                            }
                            else
                            {
                                API.setVehicleEngineStatus(vehicle, false);
                            }   
                        }
                    }
                }
            }
        }
    }
}
