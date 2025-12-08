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
    /// Logique d'interaction pour ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow :Window
    {
        public bool IsConfirmed { get; private set; } = false;

        public ConfirmationWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click( object sender, RoutedEventArgs e )
        {
            IsConfirmed = true;
            this.Close(); // ferme la fenêtre
        }

        private void NoButton_Click( object sender, RoutedEventArgs e )
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
