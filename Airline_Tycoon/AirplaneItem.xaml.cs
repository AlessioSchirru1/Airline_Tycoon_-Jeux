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

        public AirplaneItem( Airplane airplaneModel , int index)
        {

            InitializeComponent();

            // On enregistre l'objet avion
            airplane = airplaneModel;

            // On met le titre correct
            TitleText.Text = $"Airplane {index}";

            // On charge les valeurs dans l'affichage
            LoadAirplaneData();
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
            // Plus tard, tu remplaceras ça par ta fonction
            airplane.Seats++;
            airplane.SeatsPrice += 20;
            LoadAirplaneData();
        }

        private void TicketButton_Click( object sender, RoutedEventArgs e )
        {
            // Plus tard, tu remplaceras ça par ta fonction
            airplane.Ticket++;
            airplane.TicketPrice += 20;
            LoadAirplaneData();
        }

        private void SpeedButton_Click( object sender, RoutedEventArgs e )
        {
            // Plus tard, tu remplaceras ça par ta fonction
            airplane.Speed++;
            airplane.SpeedPrice += 20;
            LoadAirplaneData();
        }

        public void SetTitle( string title )
        {
            TitleText.Text = title;
        }
    }
}
