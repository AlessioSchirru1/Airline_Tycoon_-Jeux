using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Airline_Tycoon
{
    public class FlightRequest
    {
        public AirportData Destination { get; set; }
        public int PassengerCount { get; set; }
        public BigInteger BaseTicketPrice { get; set; }

        public FlightRequest( AirportData destination, int passengers, BigInteger ticketPrice )
        {
            Destination = destination;
            PassengerCount = passengers;
            BaseTicketPrice = ticketPrice;
        }
    }

    public class AirportData
    {
        public bool IsOwned { get; set; } = false;
        public string CityName { get; set; }
        public Vector2 Position { get; set; }
        public List<FlightRequest> Requests { get; set; } = new List<FlightRequest>();
        public int Capacity { get; set; } = 25;
        public float ArrivalSpeed { get; set; } = 1f;
        public float TicketMultiplier { get; set; } = 1.0f;

        // Ajouts pour AirportItem
        public BigInteger CapacityUpgradePrice { get; set; } = 5000;
        public BigInteger SpeedUpgradePrice { get; set; } = 5000;
        public BigInteger MultiplierUpgradePrice { get; set; } = 5000;

        public AirportData( string name, Vector2 position )
        {
            CityName = name;
            Position = position;
        }

        public int Passengers { get; set; } = 0;
        public int MaxPassengers { get; set; } = 25;

        public float PassengerGenerationRate { get; set; } = 1f;
        // passagers par seconde
        public int Level { get; set; } = 1;

        public void Upgrade()
        {
            Level++;
            PassengerGenerationRate += 0.5f;
            MaxPassengers += 25;
        }
    }

    public class AirplaneData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AirportData CurrentAirport { get; set; }
        public AirportData TargetAirport { get; set; } = null;
        public Vector2 Position { get; set; }
        public string PlaneColor { get; set; } // couleur sous forme de string ou hex
        public int Seats { get; set; } = 10;
        public int Ticket { get; set; } = 20;
        public float Speed { get; set; } = 100f;

        public AirplaneData( int id, string name, AirportData startAirport, string color )
        {
            if(id < 1)
                throw new ArgumentException("Id doit être >= 1 pour que l'image fonctionne", nameof(id));

            Id = id;
            Name = name;
            CurrentAirport = startAirport;
            TargetAirport = startAirport;
            Position = startAirport.Position;
            PlaneColor = color;


        }

        
        public BigInteger PurchasePrice { get; set; } = 0;

        
        public int SeatsPrice { get; set; } = 200;

        
        public int AirportsPrice { get; set; } = 240;

        
        public int SpeedPrice { get; set; } = 100;

        public bool CanUpgradeSeats { get; set; }
        public bool CanUpgradeSpeed { get; set; }
        public bool CanUpgradeAirports { get; set; }
    }
}
