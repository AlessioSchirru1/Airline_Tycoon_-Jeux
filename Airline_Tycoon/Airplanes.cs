using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class Airplane
    {

        public string Name { get; set; } = "Airplane";
        public BigInteger PurchasePrice { get; set; } = 0;

        public int Seats { get; set; } = 10;
        public int SeatsPrice { get; set; } = 200;

        public int Ticket { get; set; } = 18;
        public int AirportsPrice { get; set; } = 240;

        public int Speed { get; set; } = 100;
        public int SpeedPrice { get; set; } = 100;

        public bool CanUpgradeSeats { get; set; }
        public bool CanUpgradeSpeed { get; set; }
        public bool CanUpgradeAirports { get; set; }
    }

}
