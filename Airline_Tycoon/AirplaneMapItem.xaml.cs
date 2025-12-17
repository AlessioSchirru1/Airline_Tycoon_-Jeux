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
    /// Logique d'interaction pour AirplaneMapItem.xaml
    /// </summary>
    public partial class AirplaneMapItem :UserControl
    {
        public AirplaneMapItem( int airplaneId )
        {
            InitializeComponent();

            // Assigne l'image selon l'ID
            int imgIndex = (airplaneId - 1) % 6 + 1;
            string imgPath = $"pack://application:,,,/img/avion{imgIndex}.png";
            AirplaneImage.Source = new BitmapImage(new Uri(imgPath));
        }
    }
}
