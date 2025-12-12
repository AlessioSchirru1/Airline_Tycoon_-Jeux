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
using System.Numerics;

namespace Airline_Tycoon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window
    {
        // Collections
        public List<Airplane> Airplanes { get; private set; } = new List<Airplane>();
        public List<Airport> Airports { get; private set; } = new List<Airport>();


        private AirplanesView currentAirplanesView;
        //airportsView = new AirportsView( Airports);


        // Références uniques aux panneaux
        private AirplanesView airplanesView;
        private TicketsView ticketsView;
        private ManagerView managerView;

        public MainWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            Airplanes.Add(new Airplane());
            Airplanes.Add(new Airplane());

            airplanesView = new AirplanesView(Airplanes);
            ticketsView = new TicketsView(); // à créer
            managerView = new ManagerView(); // à créer
            ContentArea.Content = airplanesView;

            currentAirplanesView = airplanesView;
        }

        private void AirplanesButton_Click( object sender, RoutedEventArgs e )
        {
            ToggleContent(airplanesView);
            currentAirplanesView = airplanesView;
        }

        private void TicketsButton_Click( object sender, RoutedEventArgs e )
        {
            ToggleContent(ticketsView);
            //currentAirportsView = airportsView;
        }
        private void ManagerButton_Click( object sender, RoutedEventArgs e ) => ToggleContent(managerView);

        private void CloseButton_Click( object sender, RoutedEventArgs e )
        {
            ConfirmationWindow confirm = new ConfirmationWindow();
            confirm.Owner = this;
            confirm.ShowDialog();

            if(confirm.IsConfirmed)
            {
                Application.Current.Shutdown();
            }
        }

        private void ToggleContent( UserControl newContent )
        {
            if(ContentArea.Content != newContent)
                ContentArea.Content = newContent;
        }

        private void Boutonparametre_Click( object sender, RoutedEventArgs e )
        {
            parametre confirm = new parametre();
            confirm.Owner = this;
            confirm.ShowDialog();
        }

        public event Action CapitalChanged;

        private BigInteger  _capital = 0;

        private void AddMoney_Click( object sender, RoutedEventArgs e )
        {
            Capital += BigInteger.Parse("5000");
            UpdateCapitalDisplay();
        }

        public void UpdateCapitalDisplay() => CapitalText.Text = $"${NumberFormatter.Format(Capital)}";
        

        public BigInteger Capital
        {
            get => _capital;
            set
            {
                _capital = value;
                CapitalText.Text = $"${NumberFormatter.Format(_capital)}";
                CapitalChanged?.Invoke();
            }
        }

        

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

        private void MainWindow_PreviewKeyDown( object sender, KeyEventArgs e )
        {
            if(e.Key == Key.Escape)
            {
                OpenQuitConfirmation();
                e.Handled = true;
            }
        }

        private void OpenQuitConfirmation()
        {
            ConfirmationWindow confirm = new ConfirmationWindow();
            confirm.Owner = this;
            confirm.ShowDialog();

            if(confirm.IsConfirmed)
                Application.Current.Shutdown();
        }

    }
}