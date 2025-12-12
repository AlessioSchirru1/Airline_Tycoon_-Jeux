using NAudio.Wave;
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
    /// Logique d'interaction pour parametre.xaml
    /// </summary>
    public partial class parametre : Window
    {
        public parametre()
        {
            InitializeComponent();
            cbVolumeson.Items.Add("30");
            cbVolumeson.Items.Add("20");
            cbVolumeson.Items.Add("10");

            // Astuce : Sélectionner le premier élément par défaut (évite une case vide)
            cbVolumeson.SelectedIndex = 0;
        }
        private void cbVolumeson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // C'est ici que tu mettras le code pour changer le volume
            // Pour l'instant, tu peux laisser vide ou mettre un message de test
        }
    }
}
