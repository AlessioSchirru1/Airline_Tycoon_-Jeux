using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using static Airline_Tycoon.Airport;

namespace Airline_Tycoon
{
    public partial class AirportsView :UserControl
    {
        private List<AirportData> airports;
        private List<string> Cities = new List<string> { "London", "New York", "Rome", "Algé", "Sydney", "Moscou", "Tokyo" };

        public AirportsView( List<AirportData> airportList )
        {
            InitializeComponent();
            airports = airportList;

            if(airports.Count == 0)
            {
                airports.Add(new AirportData(Cities[0], new Vector2(0, 0)) { IsOwned = false } );
                airports.Add(new AirportData(Cities[1], new Vector2(100, 0)) { IsOwned = false } );
                airports.Add(new AirportData(Cities[2], new Vector2(200, 0)) { IsOwned = false } );
            }

            GenerateAirportViews();
            UpdateButtonsState();

            var main = Application.Current.MainWindow as MainWindow;
            if(main != null)
                main.CapitalChanged += UpdateButtonsState;
        }

        private void GenerateAirportViews()
        {
            ListContainer.Children.Clear();

            foreach(var airport in airports)
            {
                if(airport.IsOwned)
                {
                    ListContainer.Children.Add(new AirportItem(airport));
                }
            }

            ListContainer.Children.Add(AddAirportButton);
        }


        private BigInteger GetNextAirportPrice( int index )
        {
            return 5000 * BigInteger.Pow(3, index - 3);
        }

        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;
            if(main == null) return;

            var nextAirport = airports.FirstOrDefault(a => !a.IsOwned);
            bool canBuy = nextAirport != null && main.Capital >= GetNextAirportPrice(airports.IndexOf(nextAirport));

            AddAirportText.Foreground = canBuy ? Brushes.White : Brushes.Black;
            NextAirportPriceText.Foreground = canBuy ? Brushes.White : Brushes.Gray;
            NextAirportPriceText.Text = canBuy
                ? $"${NumberFormatter.Format(GetNextAirportPrice(airports.IndexOf(nextAirport)))}"
                : "MAX";

            AddAirportButton.Background = canBuy ? new SolidColorBrush(Color.FromRgb(68, 68, 68))
                                                 : new SolidColorBrush(Color.FromRgb(30, 30, 30));
            AddAirportButton.IsEnabled = canBuy;

            foreach(var item in ListContainer.Children.OfType<AirportItem>())
            {
                item.RefreshState();
            }
        }

        private void AddAirportButton_Click( object sender, RoutedEventArgs e )
        {
            var main = Application.Current.MainWindow as MainWindow;
            if(main == null) return;

            // Trouver le premier aéroport non acheté
            var airportToBuy = airports.FirstOrDefault(a => !a.IsOwned);
            if(airportToBuy == null) return; // tous achetés

            int index = airports.IndexOf(airportToBuy);
            BigInteger price = GetNextAirportPrice(index);

            if(main.Capital < price) return;

            // Déduire le capital
            main.Capital -= price;
            airportToBuy.IsOwned = true;

            // Ajouter le cercle sur la map
            if(main != null)
            {
                Image circle = new Image
                {
                    Source = new BitmapImage(new Uri("/img/cercle-rouge.png", UriKind.Relative)),
                    Width = 25,
                    Height = 25,
                    Tag = airportToBuy
                };
                main.MapCanvas.Children.Add(circle);
                Canvas.SetLeft(circle, airportToBuy.Position.X);
                Canvas.SetTop(circle, airportToBuy.Position.Y);
            }

            // Mettre à jour l’affichage
            GenerateAirportViews();
            UpdateButtonsState();
        }

        public void RefreshAll()
        {
            float deltaSeconds = 0.5f; // intervalle du DispatcherTimer

            foreach(var item in ListContainer.Children.OfType<AirportItem>())
            {
                item.UpdatePassengers(deltaSeconds);
                item.RefreshState(); // pour mettre à jour boutons et textes
            }
        }
    }

}
