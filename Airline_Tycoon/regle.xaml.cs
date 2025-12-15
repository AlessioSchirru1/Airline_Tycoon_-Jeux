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
using System.Windows.Shapes;

namespace Airline_Tycoon
{
    /// <summary>
    /// Logique d'interaction pour regle.xaml
    /// </summary>
    public partial class regle : Window
    {
        public bool IsConfirmed { get; private set; } = false;
        public regle()
        {
            InitializeComponent();
        }

        private void fermer_regle_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
