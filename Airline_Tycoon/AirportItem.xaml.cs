using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Airline_Tycoon
{
    /// <summary>
    /// Logique d'interaction pour AirplaneItem.xaml
    /// </summary>
    public partial class AirportItem :UserControl
    {
        private AirportData airport;
        private MainWindow main;
        private int niveauCapacite = 1;
        private int niveauRemplissage = 1;
        private int niveauMultiplicateur = 1;
        private double multiplicateurPrix = 1.25;

        public AirportItem( AirportData airportModel )
        {
            InitializeComponent();

            airport = airportModel;
            main = Application.Current.MainWindow as MainWindow;

            TitleText.Text = airport.CityName;

            // Abonnement à l'événement pour mettre à jour les boutons automatiquement
            if(main != null)
                main.CapitalChanged += UpdateButtons;

            LoadAirportData();
            UpdateButtons();
        }

        private void LoadAirportData()
        {
            // Capacité
            SeatsValueText.Text = airport.Capacity.ToString();
            SeatsPriceText.Text = $"${airport.CapacityUpgradePrice}";

            // Vitesse d’arrivée
            TicketValueText.Text = $"{airport.ArrivalSpeed}/s";
            TicketPriceText.Text = $"${airport.SpeedUpgradePrice}";

            // Multiplicateur billet
            SpeedValueText.Text = $"{airport.TicketMultiplier:F1}x";
            SpeedPriceText.Text = $"${airport.MultiplierUpgradePrice}";
        }

        private void UpdateButtons()
        {
            if(main == null) return;

            CapaciteButton.IsEnabled = main.Capital >= airport.CapacityUpgradePrice;
            RemplissageButton.IsEnabled = main.Capital >= airport.SpeedUpgradePrice;
            MultiplicateurButton.IsEnabled = main.Capital >= airport.MultiplierUpgradePrice;

            SeatsValueText.Foreground = CapaciteButton.IsEnabled ? Brushes.White : Brushes.Black;
            SeatsPriceText.Foreground = CapaciteButton.IsEnabled ? Brushes.White : Brushes.Black;

            TicketValueText.Foreground = RemplissageButton.IsEnabled ? Brushes.White : Brushes.Black;
            TicketPriceText.Foreground = RemplissageButton.IsEnabled ? Brushes.White : Brushes.Black;

            SpeedValueText.Foreground = MultiplicateurButton.IsEnabled ? Brushes.White : Brushes.Black;
            SpeedPriceText.Foreground = MultiplicateurButton.IsEnabled ? Brushes.White : Brushes.Black;

            if(!airport.IsOwned)
            {
                CapaciteButton.IsEnabled = false;
                RemplissageButton.IsEnabled = false;
                MultiplicateurButton.IsEnabled = false;
                return;
            }
        }

        private void CapaciteButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airport.CapacityUpgradePrice) return;

            main.Capital -= airport.CapacityUpgradePrice;
            airport.Capacity += 25;
            airport.MaxPassengers = airport.Capacity;
            niveauCapacite++;
            airport.CapacityUpgradePrice = (int)ArrondiDynamique(PrixNiveau(niveauCapacite, 100));
            

            LoadAirportData();
            UpdateButtons();
        }

        private void RemplissageButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airport.SpeedUpgradePrice) return;

            main.Capital -= airport.SpeedUpgradePrice;
            airport.ArrivalSpeed += 1;
            airport.CurrentPassengerSpeed = airport.ArrivalSpeed/2;

            niveauRemplissage++;
            airport.SpeedUpgradePrice = (int)ArrondiDynamique(PrixNiveau(niveauRemplissage, 100));
            

            GeneratePassengers(1f);

            LoadAirportData();
            UpdateButtons();
        }

        private void MultiplicateurButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airport.MultiplierUpgradePrice) return;

            main.Capital -= airport.MultiplierUpgradePrice;
            airport.TicketMultiplier += 0.1f;
            niveauMultiplicateur++;
            airport.MultiplierUpgradePrice = (int)ArrondiDynamique(PrixNiveau(niveauMultiplicateur, 100));

            LoadAirportData();
            UpdateButtons();
        }

        public void RefreshState()
        {
            LoadAirportData();
            UpdateButtons();
            PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        }



        public void UpdatePassengers( float deltaSeconds )
        {
            if(!airport.IsOwned) return;

            // Génération de passagers simple
            int newPassengers = (int)(airport.ArrivalSpeed * deltaSeconds);
            airport.Passengers += newPassengers;

            // Ne pas dépasser le maximum
            if(airport.Passengers > airport.MaxPassengers)
                airport.Passengers = airport.MaxPassengers;

            // Affichage
            PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        }

        //public void UpdatePassengers( float deltaSeconds )
        //{
        //    if(!airport.IsOwned) return;

        //    // Génération de passagers simple
        //    int newPassengers = (int)(airport.ArrivalSpeed * deltaSeconds);
        //    airport.Passengers += newPassengers;

        //    if(airport.Passengers > airport.MaxPassengers)
        //        airport.Passengers = airport.MaxPassengers;

        //    // Affichage
        //    PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        //}

        //public void UpdatePassengers( float deltaSeconds )
        //{
        //    if(!airport.IsOwned) return;

        //    // Génération de passagers total
        //    int newPassengers = (int)(airport.CurrentPassengerSpeed * deltaSeconds);
        //    airport.Passengers += newPassengers;

        //    if(airport.Passengers > airport.MaxPassengers)
        //        airport.Passengers = airport.MaxPassengers;

        //    // Répartition des nouveaux passagers vers les destinations
        //    if(airport.Requests.Count > 0)
        //    {
        //        // Exemple simple : répartir équitablement
        //        int perAirport = newPassengers / airport.Requests.Count;
        //        foreach(var request in airport.Requests)
        //        {
        //            request.PassengerCount += perAirport;
        //        }

        //        // On met le reste sur le premier aéroport
        //        int reste = newPassengers % airport.Requests.Count;
        //        if(reste > 0)
        //            airport.Requests[0].PassengerCount += reste;
        //    }

        //    PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}"; // juste pour info
        //}

        public void GeneratePassengers( float deltaSeconds )
        {
            if(!airport.IsOwned) return;

            // Chaque tick ajoute un nombre entier de passagers égal à la vitesse
            int passengersToAdd = (int)airport.ArrivalSpeed;

            airport.Passengers += passengersToAdd;

            if(airport.Passengers > airport.MaxPassengers)
                airport.Passengers = airport.MaxPassengers;

            PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        }

        //public void GeneratePassengers( float deltaSeconds )
        //{
        //    if(!airport.IsOwned) return;


        //    int newPassengers = (int)(airport.ArrivalSpeed * deltaSeconds);

        //    airport.Passengers += newPassengers;

        //    // Ne dépasse pas le max
        //    if(airport.Passengers > airport.MaxPassengers)
        //        airport.Passengers = airport.MaxPassengers;

        //    // Met à jour le texte
        //    PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        //}

        private double ArrondiDynamique( double nombre )
        {
            if(nombre < 1000)
                return Math.Round(nombre / 10.0) * 10;
            else if(nombre < 10000)
                return Math.Round(nombre / 100.0) * 100;
            else if(nombre < 100000)
                return Math.Round(nombre / 1000.0) * 1000;
            else if(nombre < 1000000)
                return Math.Round(nombre / 10000.0) * 10000;
            else
            {
                double facteur = Math.Pow(10, Math.Floor(Math.Log10(nombre)) - 1);
                return Math.Round(nombre / facteur) * facteur;
            }
        }

        private double PrixNiveau( int niveau, double prixDepart )
        {
            return prixDepart * Math.Pow(multiplicateurPrix, niveau - 1);
        }
    }
}
