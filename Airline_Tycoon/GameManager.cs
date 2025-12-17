using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

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

        private DispatcherTimer airportTimer;

        public GameManager()
        {
            InitializeAirports();
            InitializeAirplanes();
            //InitializeFlightRequests();
            airportTimer = new DispatcherTimer();
            airportTimer.Interval = TimeSpan.FromSeconds(1);
            airportTimer.Start();
        }



        private void InitializeAirports()
        {
            Vector2[] positions = new Vector2[]
            {
                new Vector2(490,430),
                new Vector2(280,450),
                new Vector2(520,460),
                new Vector2(500,530),
                new Vector2(870,660),
                new Vector2(660,420),
                new Vector2(885,490)
            };

            for(int i = 0 ; i < airportNames.Count ; i++)
            {
                    var airport = new AirportData(airportNames[i], positions[i]);
                    airport.IsOwned = i < 3; // Les 3 premiers aéroports sont déjà achetés
                    Airports.Add(airport);
            }



            //foreach(var airport in Airports)
            //{
            //    airport.Requests.Clear();

            //    foreach(var dest in Airports)
            //    {
            //        if(dest == airport) continue;
            //        airport.Requests.Add(new FlightRequest(dest, 0, 10)); // 0 passagers au départ
            //    }
            //}

            //foreach(var airport in Airports)
            //{
            //    airport.Passengers = 0;
            //    airport.Requests.Clear();

            //    foreach(var dest in Airports)
            //    {
            //        if(dest == airport) continue;
            //        airport.Requests.Add(new FlightRequest(dest, 0, 10)); // 0 passagers au départ
            //    }
            //}


        }
        //private void DistributePassengers( AirportData airport )
        //{
        //    if(airport.Passengers <= 0) return;

        //    var ownedDestinations = airport.Requests.Where(r => r.Destination.IsOwned).ToList();
        //    int totalRequests = ownedDestinations.Count;
        //    if(totalRequests == 0) return;

        //    // Réinitialiser à zéro la répartition précédente
        //    foreach(var request in ownedDestinations)
        //        request.PassengerCount = 0;

        //    // Répartir équitablement les passagers
        //    int passengersPerRequest = airport.Passengers / totalRequests;

        //    foreach(var request in ownedDestinations)
        //        request.PassengerCount = passengersPerRequest;

        //    // Reste non réparti
        //    airport.Passengers = airport.Passengers % totalRequests;
        //}






        private void InitializeAirplanes()
        {
            // Le joueur commence avec 2 avions à London
            Airplanes.Add(new AirplaneData(1, "Airplane 1", Airports[0], "#FF0000"));
            Airplanes.Add(new AirplaneData(2, "Airplane 2", Airports[0], "#0000FF"));
        }

        //private void InitializeFlightRequests()
        //{
        //    // Exemple : chaque aéroport a des demandes vers les autres
        //    Random rnd = new Random();
        //    foreach(var airport in Airports)
        //    {
        //        foreach(var dest in Airports)
        //        {
        //            if(dest == airport) continue;
        //            int passengers = rnd.Next(50, 200);
        //            int ticketPrice = rnd.Next(10,50);
        //            airport.Requests.Add(new FlightRequest(dest, passengers, ticketPrice));
        //        }
        //    }
        //}

        // Calcule le revenu d'un trajet
        public BigInteger CalculateRevenue( FlightRequest request, AirplaneData airplane )
        {
            return (BigInteger)( request.PassengerCount * airplane.Ticket * request.Destination.TicketMultiplier );
        }

        // Assignation d'un avion à un trajet
        public void AssignAirplane( AirplaneData airplane, FlightRequest request )
        {
            int usedPassengers = Math.Min( airplane.Seats, airplane.CurrentAirport.Passengers );
            if(usedPassengers == 0)
                return; // pas de passagers → pas de vol

            airplane.CurrentAirport.Passengers -= usedPassengers;

            airplane.TargetAirport = request.Destination;
            airplane.CurrentAirport = request.Destination;
            airplane.Position = request.Destination.Position;

            BigInteger revenue = usedPassengers * airplane.Ticket;
            Capital += revenue;

            Console.WriteLine($"{airplane.Name} est allé à {request.Destination.CityName} et a gagné ${revenue} !");

            // On retire les passagers de la demande
            request.PassengerCount = 0;
        }

        // Retourne le trajet le plus rentable pour un avion
        //public FlightRequest GetBestFlight( AirplaneData airplane )
        //{
        //    return airplane.CurrentAirport.Requests.Where(r => r.PassengerCount > 0).OrderByDescending(r => CalculateRevenue(r, airplane)).FirstOrDefault();
        //}

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

        //private void UpdateAirports( object sender, EventArgs e )
        //{
        //    foreach(var airport in Airports)
        //    {
        //        if(!airport.IsOwned) continue;

        //        // Ajoute les passagers selon la vitesse
        //        airport.Passengers += (int)airport.ArrivalSpeed;

        //        // Ne pas dépasser le maximum
        //        if(airport.Passengers > airport.MaxPassengers)
        //            airport.Passengers = airport.MaxPassengers;

        //        // -- Mettre en commentaire la répartition automatique --
        //        // DistributePassengers(airport);
        //    }
        //}

        //private void UpdateAirports( object sender, EventArgs e )
        //{
        //    foreach(var airport in Airports)
        //    {
        //        if(!airport.IsOwned) continue;

        //        // Ajoute les passagers selon la vitesse
        //        airport.Passengers += (int)airport.ArrivalSpeed;

        //        // Ne pas dépasser le maximum
        //        if(airport.Passengers > airport.MaxPassengers)
        //            airport.Passengers = airport.MaxPassengers;
        //    }
        //}

        //private void UpdateAirports( object sender, EventArgs e )
        //{
        //    foreach(var airport in Airports)
        //    {
        //        if(!airport.IsOwned) continue;

        //        // Génération de passagers selon la vitesse de l'aéroport
        //        airport.Passengers += (int)airport.ArrivalSpeed;

        //        if(airport.Passengers > airport.MaxPassengers)
        //            airport.Passengers = airport.MaxPassengers;

        //        // Répartition automatique uniquement vers les aéroports possédés
        //        DistributePassengers(airport);

        //        // S'assurer que le total des passagers répartis + restant n'excède pas MaxPassengers
        //        int totalDistributed = airport.Requests.Sum(r => r.PassengerCount);
        //        if(totalDistributed > airport.MaxPassengers)
        //        {
        //            // réduire proportionnellement
        //            float ratio = (float)airport.MaxPassengers / totalDistributed;
        //            foreach(var r in airport.Requests)
        //                r.PassengerCount = (int)( r.PassengerCount * ratio );
        //        }

        //        // Le reste non réparti
        //        airport.Passengers = airport.MaxPassengers - airport.Requests.Sum(r => r.PassengerCount);
        //    }
        //}
    }
}
