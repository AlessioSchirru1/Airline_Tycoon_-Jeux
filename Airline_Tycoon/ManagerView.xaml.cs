using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Logique d'interaction pour ManagerView.xaml
    /// </summary>
    public partial class ManagerView : UserControl
    {
        private List<Manager> manager;

        public ManagerView(List<Manager> managerList)
        {
            InitializeComponent();
            manager = managerList;

            //managerView = new ManagerView(gameManager.Managers);

            GenerateManagerViews();

            var main = Application.Current.MainWindow as MainWindow;
            main!.CapitalChanged += UpdateButtonsState;

            UpdateButtonsState();
        }
        private void GenerateManagerViews()
        {
            ListContainer.Children.Clear();

            int index = 1;
            foreach (var manager in manager)
            {
                ListContainer.Children.Add(new ManagerItem(manager, index));
                index++;
            }

            // Bouton "Buy New Airplane" toujours en bas
            ListContainer.Children.Add(AddManagerButton);

            int nextIndex = manager.Count + 1;
            NextManagerPriceText.Text = $"${NumberFormatter.Format(GetManagerPrice(nextIndex))}";

        }
        private int managerCount = 0;
        private void AddAirplaneButton_Click(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow as MainWindow;

            int nextAirplaneIndex = manager.Count + 1;
            BigInteger price = GetManagerPrice(nextAirplaneIndex);

            // Déduire le prix
            main.Capital -= price;

            // Ajouter le nouvel avion
            manager.Add(new Manager());

            // Régénérer l’affichage
            GenerateManagerViews();
            UpdateButtonsState();
        }
        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;

            foreach (var manager in manager)
            {
                // multiplier speed
                manager.CanUpgradeMultiplierSpeed = main.Capital >= manager.MultiplierSpeedPrice;

                // multiplier
                manager.CanUpgradeMultiplier = main.Capital >= manager.MultiplierPrice;
            }

            // Raffraîchit l’écran
            foreach (var child in ListContainer.Children.OfType<ManagerItem>())
            {
                child.RefreshStat  e();
            }

            // --- NOUVEAU : met à jour le bouton Buy Airplane ---
            int nextIndex = manager.Count + 1;
            BigInteger price = GetManagerPrice(nextIndex);
            AddManagerButton.IsEnabled = main.Capital >= price;
            AddManagerButton.Opacity = AddManagerButton.IsEnabled ? 1.0 : 0.5;

            // Met à jour le texte à l'intérieur du bouton
            if (AddManagerButton.Content is StackPanel stack)
            {
                foreach (var child in stack.Children.OfType<TextBlock>())
                {
                    child.Foreground = AddManagerButton.IsEnabled ? Brushes.White : Brushes.Black;
                }
            }

            // Met à jour le prix affiché
            NextManagerPriceText.Text = $"${NumberFormatter.Format(price)}";
        }
        private BigInteger GetManagerPrice(int managerIndex)
        {
            // Exemple de progression exponentielle ou personnalisée
            // airplaneIndex = numéro du prochain avion (1 pour le premier, 2 pour le deuxième, etc.)
            switch (managerIndex)
            {
                case 1: return 0;        // déjà offert ou premier gratuit
                case 2: return 0;        // deuxième offert ?
                case 3: return 5000;
                case 4: return 15000;
                case 5: return 50000;
                default:
                    // Pour les suivants, prix = 3x prix du précédent (ou n’importe quelle formule)
                    return 3 * GetManagerPrice(managerIndex - 1);
            }
        }
    }
}
