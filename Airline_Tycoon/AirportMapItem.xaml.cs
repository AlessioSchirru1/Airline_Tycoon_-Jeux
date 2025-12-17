using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour AirportMapItem.xaml
    /// </summary>
    public partial class AirportMapItem :UserControl
    {
        private AirportData airport;

        public AirportMapItem( AirportData airport )
        {
            InitializeComponent();
            this.airport = airport;
            Refresh();
        }

        public void Refresh()
        {
            if(!airport.IsOwned)
            {
                Icon.Source = null; // pas d’image pour les aéroports non achetés
                return;
            }

            Icon.Source = new BitmapImage(new Uri("pack://application:,,,/img/cercle.png"));
            Icon.Opacity = 1.0;
        }
    }
}
