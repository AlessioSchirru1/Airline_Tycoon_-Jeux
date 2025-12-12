using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window
    {
        public List<Airplane> Airplanes { get; private set; } = new List<Airplane>();

        // On garde la trace du contenu affiché
        private object currentContent = null;

        // Références uniques aux panneaux
        private AirplanesView airplanesView;
        private TicketsView ticketsView;
        private ManagerView managerView;

        public MainWindow()
        {
            InitializeComponent();

            Airplanes.Add(new Airplane());
            Airplanes.Add(new Airplane());

            // Instanciation unique des panels
            airplanesView = new AirplanesView(Airplanes);
            ticketsView = new TicketsView(); // à créer
            managerView = new ManagerView(); // à créer
        }

        private void AirplanesButton_Click( object sender, RoutedEventArgs e ) => ToggleContent(airplanesView);
        private void TicketsButton_Click( object sender, RoutedEventArgs e ) => ToggleContent(ticketsView);
        private void ManagerButton_Click( object sender, RoutedEventArgs e ) => ToggleContent(managerView);

        private void CloseButton_Click( object sender, RoutedEventArgs e )
        {
            ConfirmationWindow confirm = new ConfirmationWindow();
            confirm.Owner = this; // centre la fenêtre par rapport à MainWindow
            confirm.ShowDialog(); // ouvre en modal

            if(confirm.IsConfirmed)
            {
                Application.Current.Shutdown(); // ferme l'application
            }
        }

        // methode pour ouvrire les different types d'améliorations

        

        //private void AirplanesButton_Click( object sender, RoutedEventArgs e )
        //{
        //    ToggleContent(new AirplanesView(Airplanes));
        //}

        ////Exemple pour d'autres boutons (TicketButton, ManagerButton…)
        //private void TicketsButton_Click( object sender, RoutedEventArgs e )
        //{
        //    ToggleContent(new TicketsView()); // à créer
        //}

        //private void ManagerButton_Click( object sender, RoutedEventArgs e )
        //{
        //    ToggleContent(new ManagerView()); // à créer
        //}

        // Fonction toggle
        //private void ToggleContent( object newContent )
        //{
        //    if(currentContent == newContent)
        //    {
        //        // Si c'est déjà affiché → on cache
        //        ContentArea.Content = null;
        //        currentContent = null;
        //    }
        //    else
        //    {
        //        // Sinon on affiche le nouveau
        //        ContentArea.Content = newContent;
        //        currentContent = newContent;
        //    }
        //}

        private void ToggleContent( UserControl newContent )
        {
            if(ContentArea.Content == newContent)
                ContentArea.Content = null; // déjà affiché → cacher
            else
                ContentArea.Content = newContent; // afficher
        }

        private void Boutonparametre_Click(object sender, RoutedEventArgs e)
        {
            parametre confirm = new parametre();
            confirm.Owner = this; // centre la fenêtre par rapport à MainWindow
            confirm.ShowDialog(); // ouvre en modal
        }
    }
}