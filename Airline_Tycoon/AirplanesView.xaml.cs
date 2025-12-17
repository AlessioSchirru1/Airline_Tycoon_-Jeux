using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Airline_Tycoon
{
    /// <summary>
    /// Logique d'interaction pour AirplanesView.xaml
    /// </summary>
    public partial class AirplanesView :UserControl
    {
        private List<AirplaneData> airplanes;
        private List<AirportData> airports;


        public AirplanesView( List<AirplaneData> airplaneList, List<AirportData> airportList )
        {
            InitializeComponent();
            airplanes = airplaneList;

            airports = airportList;



            GenerateAirplaneViews();

            var main = Application.Current.MainWindow as MainWindow;
            main!.CapitalChanged += UpdateButtonsState;

            UpdateButtonsState();
        }

        private void GenerateAirplaneViews()
        {
            ListContainer.Children.Clear();

            int index = 1;
            foreach(var planeData in airplanes)
            {
                ListContainer.Children.Add(new AirplaneItem(planeData, index));
                index++;
            }

            // Bouton "Buy New Airplane" toujours en bas
            ListContainer.Children.Add(AddAirplaneButton);

            int nextIndex = airplanes.Count + 1;
            NextAirplanePriceText.Text = $"${NumberFormatter.Format(GetAirplanePrice(nextIndex))}";

        }

        private int airplaneCount = 0;

        

        private void AddAirplaneButton_Click( object sender, RoutedEventArgs e )
        {
            var main = Application.Current.MainWindow as MainWindow;
            if(main == null) return;

            int maxAirplanes = 12;
            if(airplanes.Count >= maxAirplanes) return; // limite atteinte

            int nextIndex = airplanes.Count;
            BigInteger price = GetNextAirplanePrice(nextIndex);

            if(main.Capital < price) return; // pas assez d'argent
            main.Capital -= price;

            // Choisir un aéroport de départ (ici le premier disponible)
            AirportData startingAirport = airports.FirstOrDefault();
            if(startingAirport == null) return; // pas d'aéroport dispo

            // Créer un nouvel avion avec le constructeur obligatoire
            int newId = airplanes.Count + 1;
            string defaultColor = "White"; // ou une couleur par défaut
            AirplaneData newPlane = new AirplaneData(newId, $"Airplane {newId}", startingAirport, defaultColor);

            airplanes.Add(newPlane);



            GenerateAirplaneViews();
            UpdateButtonsState();
        }


        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;
            if(main == null) return;

            int maxAirplanes = 12;
            int nextIndex = airplanes.Count;
            bool reachedMax = airplanes.Count >= maxAirplanes;

            BigInteger price = GetNextAirplanePrice(nextIndex); // tu dois avoir une méthode similaire à GetNextAirportPrice

            bool canBuy = !reachedMax && main.Capital >= price;

            // Texte et couleur du bouton
            AddAirplaneText.Foreground = canBuy ? Brushes.White : Brushes.Black;
            NextAirplanePriceText.Foreground = reachedMax ? Brushes.Gray : ( canBuy ? Brushes.White : Brushes.Black );
            NextAirplanePriceText.Text = reachedMax ? "MAX" : $"${NumberFormatter.Format(price)}";

            AddAirplaneButton.Background = canBuy ? new SolidColorBrush(Color.FromRgb(68, 68, 68))
                                                  : new SolidColorBrush(Color.FromRgb(30, 30, 30));
            AddAirplaneButton.IsEnabled = canBuy;

            foreach(var item in ListContainer.Children.OfType<AirplaneItem>())
            {
                item.RefreshState();
            }
        }

        private BigInteger GetAirplanePrice( int airplaneIndex )
        {
            // Exemple de progression exponentielle ou personnalisée
            // airplaneIndex = numéro du prochain avion (1 pour le premier, 2 pour le deuxième, etc.)
            switch(airplaneIndex)
            {
                case 1: return 0;        // déjà offert ou premier gratuit
                case 2: return 0;        // deuxième offert ?
                case 3: return 5000;
                case 4: return 15000;
                case 5: return 50000;
                default:
                    // Pour les suivants, prix = 3x prix du précédent (ou n’importe quelle formule)
                    return 3 * GetAirplanePrice(airplaneIndex - 1);
            }
        }

        private BigInteger GetNextAirplanePrice( int nextIndex )
        {
            return GetAirplanePrice(nextIndex + 1);
        }

    }
}
