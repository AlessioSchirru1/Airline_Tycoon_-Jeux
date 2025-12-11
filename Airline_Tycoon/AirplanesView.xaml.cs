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
    /// Logique d'interaction pour AirplanesView.xaml
    /// </summary>
    public partial class AirplanesView :UserControl
    {
        private List<Airplane> airplanes;

        public AirplanesView( List<Airplane> airplaneList )
        {
            InitializeComponent();
            airplanes = airplaneList;

            // Ajouter 2 avions de base si la liste est vide
            if(airplanes.Count == 0)
            {
                airplanes.Add(new Airplane());
                airplanes.Add(new Airplane());
            }

            GenerateAirplaneViews();
        }

        private void GenerateAirplaneViews()
        {
            ListContainer.Children.Clear();

            int index = 1;
            foreach(var plane in airplanes)
            {
                ListContainer.Children.Add(new AirplaneItem(plane, index));
                index++;
            }

            // Bouton "Buy New Airplane" toujours en bas
            ListContainer.Children.Add(AddAirplaneButton);
        }

        private int airplaneCount = 0;

        private void AddAirplaneButton_Click( object sender, RoutedEventArgs e )
        {
            airplaneCount++;
            // Ajout d'un nouvel avion
            airplanes.Add(new Airplane());

            // Régénère l'affichage complet
            GenerateAirplaneViews();
        }



        


    }
}
