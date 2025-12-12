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
    /// <summary>
    /// Logique d'interaction pour AirportsView.xaml
    /// </summary>
    public partial class AirportsView :UserControl
    {
        private List<Airport> airports;
        private List<string> Cities = new List<string> { "Paris", "New York", "Tokyo", "London", "Sydney" }; // etc.

        public AirportsView( List<Airport> airportList )
        {
            InitializeComponent();
            airports = airportList;

            

            // Ajouter 3 aéroports de base si vide
            if(airports.Count == 0)
            {
                airports.Add(new Airport { CityName = Cities[0], PurchasePrice = 0 });
                airports.Add(new Airport { CityName = Cities[1], PurchasePrice = 5000 });
                airports.Add(new Airport { CityName = Cities[2], PurchasePrice = 15000 });
            }

            GenerateAirportViews();
            UpdateButtonsState();

            var main = Application.Current.MainWindow as MainWindow;
            main.CapitalChanged += UpdateButtonsState;
        }

        private void GenerateAirportViews()
        {
            ListContainer.Children.Clear();

            foreach(var airport in airports)
            {
                var tb = new TextBlock
                {
                    Text = $"{airport.CityName} - ${NumberFormatter.Format(airport.PurchasePrice)}",
                    FontSize = 20,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0,0,0,10)
                };
                ListContainer.Children.Add(tb);
            }

            // Ajouter le bouton Buy Airport en bas
            ListContainer.Children.Add(AddAirportButton);
        }

        private BigInteger GetNextAirportPrice( int index )
        {
            // Exemple : prix qui augmente exponentiellement
            return 5000 * BigInteger.Pow(3, index - 3);
        }

        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;

            int nextIndex = airports.Count;
            BigInteger price = GetNextAirportPrice(nextIndex);

            bool canBuy = main.Capital >= price;

            // Texte du bouton et prix
            AddAirportText.Foreground = canBuy ? Brushes.White : Brushes.Black;
            NextAirportPriceText.Foreground = canBuy ? Brushes.White : Brushes.Black;
            NextAirportPriceText.Text = $"${NumberFormatter.Format(price)}";

            // Optionnel : changer le fond
            AddAirportButton.Background = canBuy ? new SolidColorBrush(Color.FromRgb(68, 68, 68))
                                                 : new SolidColorBrush(Color.FromRgb(30, 30, 30));
        }

        private void AddAirportButton_Click( object sender, RoutedEventArgs e )
        {
            var main = Application.Current.MainWindow as MainWindow;
            int nextIndex = airports.Count;
            BigInteger price = GetNextAirportPrice(nextIndex);

            if(main.Capital < price) return; // pas assez → ne fait rien

            main.Capital -= price;

            airports.Add(new Airport
            {
                CityName = Cities[nextIndex % Cities.Count],
                PurchasePrice = price
            });

            GenerateAirportViews();
            UpdateButtonsState();
        }
    }

}
