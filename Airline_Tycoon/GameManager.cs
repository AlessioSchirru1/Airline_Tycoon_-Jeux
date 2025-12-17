using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    public class GameManager
    {
        public List<Manager> Managers { get; private set; } = new List<Manager>();
        public List<AirportData> Airports { get; private set; } = new List<AirportData>();
        public List<AirplaneData> Airplanes { get; private set; } = new List<AirplaneData>();
        public BigInteger Capital { get; set; } = 100000;

        private List<string> airportNames = new List<string>
            { "London", "New York", "Rome", "Alger", "Sydney", "Moscow", "Tokyo" };

        public GameManager()
        {
            InitializeAirports();
            InitializeAirplanes();
            InitializeFlightRequests();
        }



    private void InitializeAirports()
    {
        Vector2[] positions = new Vector2[]
        {
            new Vector2(350,420),
            new Vector2(500,150),
            new Vector2(300,300),
            new Vector2(600,400),
            new Vector2(800,200),
            new Vector2(700,500),
            new Vector2(900,100)
        };

        for(int i = 0 ; i < airportNames.Count ; i++)
        {
                var airport = new AirportData(airportNames[i], positions[i]);
                airport.IsOwned = i < 3; // Les 3 premiers aéroports sont déjà achetés
                Airports.Add(airport);
        }
    }

        private void InitializeAirplanes()
        {
            // Le joueur commence avec 2 avions à London
            Airplanes.Add(new AirplaneData(1, "Airplane 1", Airports[0], "#FF0000"));
            Airplanes.Add(new AirplaneData(2, "Airplane 2", Airports[0], "#0000FF"));
        }

        private void InitializeFlightRequests()
        {
            // Exemple : chaque aéroport a des demandes vers les autres
            Random rnd = new Random();
            foreach(var airport in Airports)
            {
                foreach(var dest in Airports)
                {
                    if(dest == airport) continue;
                    int passengers = rnd.Next(50, 200);
                    int ticketPrice = rnd.Next(10,50);
                    airport.Requests.Add(new FlightRequest(dest, passengers, ticketPrice));
                }
            }
        }

        // Calcule le revenu d'un trajet
        public BigInteger CalculateRevenue( FlightRequest request, AirplaneData airplane )
        {
            return (BigInteger)( request.PassengerCount * airplane.Ticket * request.Destination.TicketMultiplier );
        }

        // Assignation d'un avion à un trajet
        public void AssignAirplane( AirplaneData airplane, FlightRequest request )
        {
            airplane.TargetAirport = request.Destination;
            // Pour test console, on simule l'arrivée immédiate
            airplane.CurrentAirport = request.Destination;
            airplane.Position = request.Destination.Position;

            BigInteger revenue = CalculateRevenue(request, airplane);
            Capital += revenue;

            Console.WriteLine($"{airplane.Name} est allé à {request.Destination.CityName} et a gagné ${revenue} !");

            // On retire les passagers de la demande
            request.PassengerCount = 0;
        }

        // Retourne le trajet le plus rentable pour un avion
        public FlightRequest GetBestFlight( AirplaneData airplane )
        {
            return airplane.CurrentAirport.Requests.Where(r => r.PassengerCount > 0).OrderByDescending(r => CalculateRevenue(r, airplane)).FirstOrDefault();
        }

        // Affiche l'état du jeu pour test console
        public void PrintGameState()
        {
            Console.WriteLine($"Capital: ${Capital}");
            Console.WriteLine("Avions:");
            foreach(var plane in Airplanes)
            {
                Console.WriteLine($"- {plane.Name} à {plane.CurrentAirport.CityName}");
            }
        }
    }
}
