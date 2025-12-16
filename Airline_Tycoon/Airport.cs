using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class Airport
    {
        public string CityName { get; set; } = "Unknown";
        public bool CanBuy { get; set; }

        

        // Nouvelle logique pour les boutons
        public int Capacity { get; set; } = 100;
        public int CapacityUpgradePrice { get; set; } = 5000;

        public int ArrivalSpeed { get; set; } = 5; // passagers par seconde
        public int SpeedUpgradePrice { get; set; } = 5000;

        public double TicketMultiplier { get; set; } = 1.0;
        public int MultiplierUpgradePrice { get; set; } = 5000;

        public BigInteger PurchasePrice { get; set; } = 0; // prix d'achat initial de l'aéroport
    }
}
