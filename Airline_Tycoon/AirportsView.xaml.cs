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
        private List<string> Cities = new List<string> { "London", "New York", "Rome", "Algé", "Sydney", "Moscou", "Tokyo" };

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
                ListContainer.Children.Add(new AirportItem(airport));
            }

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
            if(main == null) return;

            int nextIndex = airports.Count;
            int maxAirports = Cities.Count; // 7
            bool reachedMax = airports.Count >= maxAirports;

            BigInteger price = GetNextAirportPrice(nextIndex);

            // On ne peut acheter que si on n'a pas atteint la limite ET si on a assez d'argent
            bool canBuy = !reachedMax && main.Capital >= price;

            // Texte du bouton et prix
            AddAirportText.Foreground = canBuy ? Brushes.White : Brushes.Black;
            NextAirportPriceText.Foreground = reachedMax ? Brushes.Gray : ( canBuy ? Brushes.White : Brushes.Black );
            NextAirportPriceText.Text = reachedMax ? "MAX" : $"${NumberFormatter.Format(price)}";

            // Fond du bouton
            AddAirportButton.Background = canBuy ? new SolidColorBrush(Color.FromRgb(68, 68, 68))
                                                 : new SolidColorBrush(Color.FromRgb(30, 30, 30));
            AddAirportButton.IsEnabled = canBuy;

            foreach(var item in ListContainer.Children.OfType<AirportItem>())
            {
                item.RefreshState();
            }
        }

        //public void UpdateButtonsState()
        //{
        //    var main = Application.Current.MainWindow as MainWindow;

        //    int nextIndex = airports.Count;
        //    BigInteger price = GetNextAirportPrice(nextIndex);

        //    bool canBuy = main.Capital >= price;

        //    // Texte du bouton et prix
        //    AddAirportText.Foreground = canBuy ? Brushes.White : Brushes.Black;
        //    NextAirportPriceText.Foreground = canBuy ? Brushes.White : Brushes.Black;
        //    NextAirportPriceText.Text = $"${NumberFormatter.Format(price)}";

        //    // Optionnel : changer le fond
        //    AddAirportButton.Background = canBuy ? new SolidColorBrush(Color.FromRgb(68, 68, 68))
        //                                         : new SolidColorBrush(Color.FromRgb(30, 30, 30));

        //    foreach(var item in ListContainer.Children.OfType<AirportItem>())
        //    {
        //        item.RefreshState();
        //    }
        //}

        private void AddAirportButton_Click( object sender, RoutedEventArgs e )
        {
            var main = Application.Current.MainWindow as MainWindow;

            // Limite des aéroports
            if(airports.Count >= Cities.Count)
                return; // on ne peut plus acheter

            int nextIndex = airports.Count;
            BigInteger price = GetNextAirportPrice(nextIndex);

            if(main.Capital < price) return; // pas assez → ne fait rien

            main.Capital -= price;

            airports.Add(new Airport
            {
                CityName = Cities[nextIndex],
                PurchasePrice = price
            });

            GenerateAirportViews();
            UpdateButtonsState();
        }

        //private void AddAirportButton_Click( object sender, RoutedEventArgs e )
        //{
        //    var main = Application.Current.MainWindow as MainWindow;
        //    int nextIndex = airports.Count;
        //    BigInteger price = GetNextAirportPrice(nextIndex);

        //    if(main.Capital < price) return; // pas assez → ne fait rien

        //    main.Capital -= price;

        //    airports.Add(new Airport
        //    {
        //        CityName = Cities[nextIndex % Cities.Count],
        //        PurchasePrice = price
        //    });

        //    GenerateAirportViews();
        //    UpdateButtonsState();
        //}
    }

}
