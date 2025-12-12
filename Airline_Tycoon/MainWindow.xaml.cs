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
        private AirplanesView currentAirplanesView;

        // Références uniques aux panneaux
        private AirplanesView airplanesView;
        private TicketsView ticketsView;
        private ManagerView managerView;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                Airplanes.Add(new Airplane());
                Airplanes.Add(new Airplane());

                // Instanciation unique des panels
                airplanesView = new AirplanesView(Airplanes);
                ticketsView = new TicketsView(); // à créer
                managerView = new ManagerView(); // à créer

                //// Initialisation du champ non-nullable
                currentAirplanesView = airplanesView;
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AirplanesButton_Click( object sender, RoutedEventArgs e )
        {
            ToggleContent(airplanesView);
            currentAirplanesView = airplanesView;
        }

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

        private void ToggleContent( UserControl newContent )
        {
            if(ContentArea.Content == newContent)
                ContentArea.Content = null; // déjà affiché → cacher
            else
                ContentArea.Content = newContent; // afficher
        }

        private void Boutonparametre_Click( object sender, RoutedEventArgs e )
        {
            parametre confirm = new parametre();
            confirm.Owner = this; // centre la fenêtre par rapport à MainWindow
            confirm.ShowDialog(); // ouvre en modal
        }

        public event Action CapitalChanged;




        private void AddMoney_Click( object sender, RoutedEventArgs e )
        {
            Capital += 500;
            UpdateCapitalDisplay();
        }

        public void UpdateCapitalDisplay()
        {
            CapitalText.Text = $"${Capital}";
        }

        public int Capital
        {
            get => _capital;
            set
            {
                _capital = value;
                CapitalText.Text = $"${_capital}";
                CapitalChanged?.Invoke();
            }
        }

        private int _capital = 1000;

        public bool TrySpendMoney( int amount )
        {
            if(Capital >= amount)
            {
                Capital -= amount;
                return true;
            }

            return false;
        }

        public void RefreshAirplaneItems()
        {
            currentAirplanesView?.UpdateButtonsState();
        }
    }
}