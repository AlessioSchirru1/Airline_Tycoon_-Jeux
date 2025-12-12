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
        private List<Airplane> airplanes;


        public AirplanesView( List<Airplane> airplaneList )
        {
            InitializeComponent();
            airplanes = airplaneList;

            // Ajouter 2 avions de base si la liste est vide
            if(airplanes.Count == 0)
            {
                airplanes.Add(new Airplane());
                airplanes.Add(new Airplane());
            }

            GenerateAirplaneViews();

            var main = Application.Current.MainWindow as MainWindow;
            main!.CapitalChanged += UpdateButtonsState;

            UpdateButtonsState();
        }

        private void GenerateAirplaneViews()
        {
            ListContainer.Children.Clear();

            int index = 1;
            foreach(var plane in airplanes)
            {
                ListContainer.Children.Add(new AirplaneItem(plane, index));
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

            int nextAirplaneIndex = airplanes.Count + 1;
            BigInteger price = GetAirplanePrice(nextAirplaneIndex);

            // Déduire le prix
            main.Capital -= price;

            // Ajouter le nouvel avion
            airplanes.Add(new Airplane());

            // Régénérer l’affichage
            GenerateAirplaneViews();
            UpdateButtonsState();
        }

        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;

            foreach(var plane in airplanes)
            {
                // Exemple : seats
                plane.CanUpgradeSeats = main.Capital >= plane.SeatsPrice;

                // speed
                plane.CanUpgradeSpeed = main.Capital >= plane.SpeedPrice;

                // tickets
                plane.CanUpgradeTickets = main.Capital >= plane.TicketPrice;
            }

            // Raffraîchit l’écran
            foreach(var child in ListContainer.Children.OfType<AirplaneItem>())
            {
                child.RefreshState();
            }

            // --- NOUVEAU : met à jour le bouton Buy Airplane ---
            int nextIndex = airplanes.Count + 1;
            BigInteger price = GetAirplanePrice(nextIndex);
            AddAirplaneButton.IsEnabled = main.Capital >= price;
            AddAirplaneButton.Opacity = AddAirplaneButton.IsEnabled ? 1.0 : 0.5;

            // Met à jour le texte à l'intérieur du bouton
            if(AddAirplaneButton.Content is StackPanel stack)
            {
                foreach(var child in stack.Children.OfType<TextBlock>())
                {
                    child.Foreground = AddAirplaneButton.IsEnabled ? Brushes.White : Brushes.Black;
                }
            }

            // Met à jour le prix affiché
            NextAirplanePriceText.Text = $"${NumberFormatter.Format(price)}";


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

    }
}
