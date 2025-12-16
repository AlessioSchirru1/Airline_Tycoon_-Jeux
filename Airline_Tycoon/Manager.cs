using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class Manager
    {

        public string Name { get; set; } = "Manager";

        // Prix d'amélioration
        public BigInteger MultiplierPrice { get; set; } = 100;
        public BigInteger MultiplierSpeedPrice { get; set; } = 200;
        public int MultiplierSpeedValue { get; set; } = 0;
        

        // Statut pour savoir si le joueur peut acheter/upgrade
        public bool CanUpgradeMultiplier { get; set; } = false;
        public bool CanUpgradeMultiplierSpeed { get; set; } = false;
    }

}
