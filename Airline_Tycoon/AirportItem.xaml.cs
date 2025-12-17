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
            airport.CapacityUpgradePrice += 5000;

            LoadAirportData();
            UpdateButtons();
        }

        private void RemplissageButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airport.SpeedUpgradePrice) return;

            main.Capital -= airport.SpeedUpgradePrice;
            airport.ArrivalSpeed += 1;
            airport.SpeedUpgradePrice += 5000;

            LoadAirportData();
            UpdateButtons();
        }

        private void MultiplicateurButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airport.MultiplierUpgradePrice) return;

            main.Capital -= airport.MultiplierUpgradePrice;
            airport.TicketMultiplier += 0.1f;
            airport.MultiplierUpgradePrice += 5000;

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

            airport.Passengers += (int)( airport.PassengerGenerationRate * deltaSeconds );
            if(airport.Passengers > airport.MaxPassengers)
                airport.Passengers = airport.MaxPassengers;

            // Mettre à jour le texte
            PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        }

        public void GeneratePassengers( float deltaSeconds )
        {
            if(!airport.IsOwned) return;

            // Multiplie la vitesse d’arrivée par le delta de temps pour gérer le tick
            int newPassengers = (int)(airport.ArrivalSpeed * deltaSeconds);

            airport.Passengers += newPassengers;

            // Ne dépasse pas le max
            if(airport.Passengers > airport.MaxPassengers)
                airport.Passengers = airport.MaxPassengers;

            // Met à jour le texte
            PassengersText.Text = $"{airport.Passengers}/{airport.MaxPassengers}";
        }
    }
}
