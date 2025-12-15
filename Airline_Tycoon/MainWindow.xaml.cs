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
        private MediaPlayer _lecteurMusique = new MediaPlayer();
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
            DemarrerMusique();


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

        private void boutonregle_Click(object sender, RoutedEventArgs e)
        {
            regle confirm = new regle();
            confirm.Owner = this;
            confirm.ShowDialog();
        }
        private void DemarrerMusique()
        {
            try
            {
                string chemin = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "musique", "musique.mp3");

                _lecteurMusique.Open(new Uri(chemin, UriKind.Absolute));

                _lecteurMusique.MediaEnded += (s, e) =>
                {
                    _lecteurMusique.Position = TimeSpan.Zero; // Retour au début
                    _lecteurMusique.Play();
                };

                _lecteurMusique.Volume = 0.5; // Volume par défaut à 50%
                _lecteurMusique.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lecture musique : " + ex.Message);
            }
        }
        public void ModifierVolume(double nouveauVolume)
        {
            _lecteurMusique.Volume = nouveauVolume;
        }
    }
}