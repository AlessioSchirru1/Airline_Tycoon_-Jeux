using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour ManagerItem.xaml
    /// </summary>
    public partial class ManagerItem : UserControl
    {
        private Manager manager;
        private MainWindow main;
        public ManagerItem(Manager managerModel, int index)
        {
            InitializeComponent();
            // On enregistre l'objet avion
            manager = managerModel;

            main = Application.Current.MainWindow as MainWindow;
            if (main == null)
                throw new InvalidOperationException("MainWindow n'est pas encore initialisée.");

            // On met le titre correct
            TitleText.Text = $"Manager {index}";

            // On charge les valeurs dans l'affichage
            LoadAirplaneData();
            UpdateButtons();
        }
        private void UpdateButtons()
        {
            MultiplierSpeedButton.IsEnabled = main.Capital >= manager.MultiplierSpeedPrice;
            MultiplierButton.IsEnabled = main.Capital >= manager.MultiplierPrice;

            // Met à jour la couleur du texte en fonction de l'état du bouton
            MutliplierSpeedValueText.Foreground = SeatsButton.IsEnabled ? Brushes.White : Brushes.Black;
            SeatsPriceText.Foreground = SeatsButton.IsEnabled ? Brushes.White : Brushes.Black;

            TicketValueText.Foreground = TicketButton.IsEnabled ? Brushes.White : Brushes.Black;
            TicketPriceText.Foreground = TicketButton.IsEnabled ? Brushes.White : Brushes.Black;

            SpeedValueText.Foreground = SpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
            SpeedPriceText.Foreground = SpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
        }
    }
}
