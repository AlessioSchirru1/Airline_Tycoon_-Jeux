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
        private int niveauSeats = 1;
        private int niveauTicket = 1;
        private int niveauSpeed = 1;
        private double multiplicateurPrix = 1.25;

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
            airplane.Seats+=2;
            niveauSeats++;
            airplane.SeatsPrice = (int)ArrondiDynamique(PrixNiveau(niveauSeats, 100));
            

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        private void TicketButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airplane.AirportsPrice)
                return;

            main.Capital -= airplane.AirportsPrice;
            airplane.Ticket+=2;
            niveauTicket++;
            airplane.AirportsPrice = (int)ArrondiDynamique(PrixNiveau(niveauTicket, 100));
            

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        private void SpeedButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital < airplane.SpeedPrice)
                return;

            main.Capital -= airplane.SpeedPrice;
            airplane.Speed+=10;
            niveauSpeed++;
            airplane.SpeedPrice = (int)ArrondiDynamique(PrixNiveau(niveauSpeed, 100));
            

            LoadAirplaneData();
            UpdateButtons();

            main.UpdateCapitalDisplay();
        }

        public void SetTitle( string title )
        {
            TitleText.Text = title;
        }

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
