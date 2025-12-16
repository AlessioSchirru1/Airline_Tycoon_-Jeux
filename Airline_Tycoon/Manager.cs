using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class Manager
    {
        public double MultiplierSpeed { get; set; } = 1.00;
        public int MultiplierSpeedPrice { get; set; } = 200;

        public int  Multiplier  { get; set; } = 18;
        public int MultiplierPrice { get; set; } = 240;

        public bool CanUpgradeMultiplierSpeed { get; set; }
        public bool CanUpgradeMultiplier { get; set; }
    }

}
