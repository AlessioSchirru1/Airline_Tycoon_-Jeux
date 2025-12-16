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
            LoadManagerData();
            UpdateButtons();
        }
        private void UpdateButtons()
        {
            MultiplierSpeedButton.IsEnabled = main.Capital >= manager.MultiplierSpeedPrice;

            // Met à jour la couleur du texte
            MultiplierSpeedValueText.Foreground = MultiplierSpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
            MultiplierSpeedPriceText.Foreground = MultiplierSpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
        }

        private void MultiplierSpeedButton_Click( object sender, RoutedEventArgs e )
        {
            if(main.Capital >= manager.MultiplierSpeedPrice)
            {
                main.Capital -= manager.MultiplierSpeedPrice;
                manager.MultiplierSpeedValue++; // exemple
                manager.MultiplierSpeedPrice *= 2; // prix augmente à chaque upgrade
                RefreshState();
            }
        }

        public void RefreshState()
        {
            MultiplierSpeedButton.IsEnabled = main.Capital >= manager.MultiplierSpeedPrice;
            MultiplierSpeedValueText.Foreground = MultiplierSpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
            MultiplierSpeedPriceText.Foreground = MultiplierSpeedButton.IsEnabled ? Brushes.White : Brushes.Black;
            MultiplierSpeedValueText.Text = manager.MultiplierSpeedValue.ToString();
            MultiplierSpeedPriceText.Text = $"${NumberFormatter.Format(manager.MultiplierSpeedPrice)}";

        }

        private void LoadManagerData()
        {
            MultiplierSpeedValueText.Text = manager.MultiplierSpeedValue.ToString();
            MultiplierSpeedPriceText.Text = $"${NumberFormatter.Format(manager.MultiplierSpeedPrice)}";
        }
    }
}
