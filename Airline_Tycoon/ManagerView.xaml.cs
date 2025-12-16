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
        private List<Manager> managers;

        public ManagerView(List<Manager> managersList)
        {
            InitializeComponent();
            managers = managersList;

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
            foreach (var manager in managers)
            {
                ListContainer.Children.Add(new ManagerItem(manager, index));
                index++;
            }

            // Bouton "Buy New Airplane" toujours en bas
            ListContainer.Children.Add(AddManagerButton);

            int nextIndex = managers.Count + 1;
            NextManagerPriceText.Text = $"${NumberFormatter.Format(GetManagerPrice(nextIndex))}";

        }
        private int managerCount = 0;
        
        public void UpdateButtonsState()
        {
            var main = Application.Current.MainWindow as MainWindow;

            foreach (var manager in managers)
            {
                // multiplier speed
                manager.CanUpgradeMultiplierSpeed = main.Capital >= manager.MultiplierSpeedPrice;

                // multiplier
                manager.CanUpgradeMultiplier = main.Capital >= manager.MultiplierPrice;
            }

            // Raffraîchit l’écran
            foreach (var child in ListContainer.Children.OfType<ManagerItem>())
            {
                child.RefreshState();
            }

            // --- NOUVEAU : met à jour le bouton Buy Airplane ---
            int nextIndex = managers.Count + 1;
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
            AddManagerButton.IsEnabled = main.Capital >= GetManagerPrice(managers.Count + 1);
        }
        private BigInteger GetManagerPrice(int managerIndex)
        {
            // Exemple de progression exponentielle ou personnalisée
            // airplaneIndex = numéro du prochain avion (1 pour le premier, 2 pour le deuxième, etc.)
            switch (managerIndex)
            {
                case 1: return 5000;
                case 2: return 15000;
                case 3: return 50000;
                default:
                    // Pour les suivants, prix = 3x prix du précédent (ou n’importe quelle formule)
                    return 3 * GetManagerPrice(managerIndex - 1);
            }
        }

        private void AddManagerButton_Click( object sender, RoutedEventArgs e )
        {
            var main = Application.Current.MainWindow as MainWindow;
            if(main == null) return;

            int nextIndex = managers.Count + 1;
            BigInteger price = GetManagerPrice(nextIndex);

            if(main.Capital < price) return; // pas assez d’argent

            main.Capital -= price;

            // Ajouter le nouveau manager
            managers.Add(new Manager());

            // Régénérer l’affichage
            GenerateManagerViews();
            UpdateButtonsState();
        }
    }
}
