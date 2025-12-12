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
        private Airplane airplane;
        private MainWindow main = new MainWindow();

        public AirplaneItem( Airplane airplaneModel , int index)
        {

            InitializeComponent();

            // On enregistre l'objet avion
            airplane = airplaneModel;

            main = (MainWindow)Application.Current.MainWindow as MainWindow;

            // On met le titre correct
            TitleText.Text = $"Airplane {index}";

            // On charge les valeurs dans l'affichage
            LoadAirplaneData();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            SeatsButton.IsEnabled = main.Capital >= airplane.SeatsPrice;
            TicketButton.IsEnabled = main.Capital >= airplane.TicketPrice;
            SpeedButton.IsEnabled = main.Capital >= airplane.SpeedPrice;
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
            TicketPriceText.Text = $"${airplane.TicketPrice}";

            SpeedValueText.Text = airplane.Speed.ToString();
            SpeedPriceText.Text = $"${airplane.SpeedPrice}";
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
            if(main.Capital < airplane.TicketPrice)
                return;

            main.Capital -= airplane.TicketPrice;
            airplane.Ticket++;
            airplane.TicketPrice += 20;

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
