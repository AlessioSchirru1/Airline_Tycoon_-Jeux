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
    public partial class AirplaneItem :UserControl
    {
        private AirplaneData airplane;
        private MainWindow main;

        public AirplaneItem( AirplaneData airplaneModel , int index)
        {

            InitializeComponent();

            // On enregistre l'objet avion
            airplane = airplaneModel;

            main = Application.Current.MainWindow as MainWindow;
            if(main == null)
                throw new InvalidOperationException("MainWindow n'est pas encore initialisée.");

            // On met le titre correct
            TitleText.Text = $"Airplane {index}";

            // On charge les valeurs dans l'affichage
            LoadAirplaneData();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            SeatsButton.IsEnabled = main.Capital >= airplane.SeatsPrice;
            TicketButton.IsEnabled = main.Capital >= airplane.AirportsPrice;
            SpeedButton.IsEnabled = main.Capital >= airplane.SpeedPrice;

            // Met à jour la couleur du texte en fonction de l'état du bouton
            SeatsValueText.Foreground = SeatsButton.IsEnabled ? Brushes.White : Brushes.Black;
            SeatsPriceText.Foreground = SeatsButton.IsEnabled ? Brushes.White : Brushes.Black;

            TicketValueText.Foreground = TicketButton.IsEnabled ? Brushes.White : Brushes.Black;
            TicketPriceText.Foreground = TicketButton.IsEnabled ? Brushes.White : Brushes.Black;

            SpeedValueText.Foreground = SpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
            SpeedPriceText.Foreground = SpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
        }

        public void RefreshState()
        {
            LoadAirplaneData();
            UpdateButtons();
        }

        private void LoadAirplaneData()
        {
            SeatsValueText.Text = airplane.Seats.ToString();
            SeatsPriceText.Text = $"${airplane.SeatsPrice}";

            TicketValueText.Text = airplane.Ticket.ToString();
            TicketPriceText.Text = $"${airplane.AirportsPrice}";

            SpeedValueText.Text = airplane.Speed.ToString();
            SpeedPriceText.Text = $"${airplane.SpeedPrice}";

            // --- Assignation de l'image selon l'ID ---
            int imgIndex = (airplane.Id - 1) % 6 + 1; // boucle sur 6 images
            string imgPath = $"pack://application:,,,/img/avion{imgIndex}.png";
            AirplaneIcon.Source = new BitmapImage(new Uri(imgPath));
        }

        private void SeatsButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airplane.SeatsPrice)
                return;

            main.Capital -= airplane.SeatsPrice;
            airplane.Seats++;
            airplane.SeatsPrice += 20;

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        private void TicketButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airplane.AirportsPrice)
                return;

            main.Capital -= airplane.AirportsPrice;
            airplane.Ticket++;
            airplane.AirportsPrice += 20;

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        private void SpeedButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airplane.SpeedPrice)
                return;

            main.Capital -= airplane.SpeedPrice;
            airplane.Speed++;
            airplane.SpeedPrice += 20;

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        public void SetTitle( string title )
        {
            TitleText.Text = title;
        }
    }
}
