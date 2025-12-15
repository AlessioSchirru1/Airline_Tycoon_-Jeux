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
        private MainWindow _fenetrePrincipale;
        public bool IsConfirmed { get; private set; } = false;
        public parametre(MainWindow main)
        {
            InitializeComponent();
            cbVolumeson.Items.Add("100");
            cbVolumeson.Items.Add("90");
            cbVolumeson.Items.Add("80");
            cbVolumeson.Items.Add("70");
            cbVolumeson.Items.Add("60");
            cbVolumeson.Items.Add("50");
            cbVolumeson.Items.Add("40");
            cbVolumeson.Items.Add("30");
            cbVolumeson.Items.Add("20");
            cbVolumeson.Items.Add("10");

            // Astuce : Sélectionner le premier élément par défaut (évite une case vide)
            cbVolumeson.SelectedIndex = 0;
            _fenetrePrincipale = main;
        }
        private void cbVolumeson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // C'est ici que tu mettras le code pour changer le volume
            // Pour l'instant, tu peux laisser vide ou mettre un message de test
        }
        MediaPlayer monLecteur = new MediaPlayer();
        private void butonfermer_Click(object sender, RoutedEventArgs e)
        {
            if (cbVolumeson.SelectedItem != null)
            {
                string valeur = cbVolumeson.SelectedItem.ToString();
                MessageBox.Show("Valeur : " + valeur);
                if (double.TryParse(valeur, out double volumeEntier))
                {
                    // Convertit 50 en 0.5, 100 en 1.0, etc.
                    _fenetrePrincipale.ModifierVolume(volumeEntier / 100.0);
                }
            }

            IsConfirmed = false;
            this.Close();
        }
    }
}
