using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class Airplane
    {
        public int Seats { get; set; } = 10;
        public int SeatsPrice { get; set; } = 200;

        public int Ticket { get; set; } = 18;
        public int TicketPrice { get; set; } = 240;

        public int Speed { get; set; } = 100;
        public int SpeedPrice { get; set; } = 100;

        public bool CanUpgradeSeats { get; set; }
        public bool CanUpgradeSpeed { get; set; }
        public bool CanUpgradeTickets { get; set; }
    }

}
