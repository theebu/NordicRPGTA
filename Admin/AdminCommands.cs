using System.Collections.Generic;
using GTANetworkServer;
using GTANetworkShared;
using IcaroRPG.Cops;

namespace IcaroRPG.Admin
{
    public class AdminCommands : Script
    {

        [Command("setplate", Group = "Admin Commands")]
        public void setCarPlate(Client sender, string plate)
        {
            var vehicle = API.getPlayerVehicle(sender);
            var newPlate = plate;
            API.setVehicleNumberPlate(vehicle, newPlate);
            Vehicles.VehicleSpawner.saveOwnedVehicles();
        }
    }
}