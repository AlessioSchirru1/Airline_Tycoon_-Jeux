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
    /// Logique d'interaction pour ManagerView.xaml
    /// </summary>
    public partial class ManagerView : UserControl
    {
        private List<Manager> manager;
        public ManagerView(List<Manager> managerList)
        {
            InitializeComponent();
            manager = managerList;

            GenerateManagerViews();

            var main = Application.Current.MainWindow as MainWindow;
            main!.CapitalChanged += UpdateButtonsState;

            UpdateButtonsState();
        }
        private void GenerateManagerViews()
        {
            ListContainer.Children.Clear();

            int index = 1;
            foreach (var plane in manager)
            {
                ListContainer.Children.Add(new AirplaneItem(manager, index));
                index++;
            }

            // Bouton "Buy New Airplane" toujours en bas
            ListContainer.Children.Add(AddManagerButton);

            int nextIndex = airplanes.Count + 1;
            NextAirplanePriceText.Text = $"${NumberFormatter.Format(GetAirplanePrice(nextIndex))}";

        }
    }
}
