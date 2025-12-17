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


        public List<Manager> Managers { get; private set; } = new List<Manager>();

        private Dictionary<AirplaneData, AirplaneItem> airplaneControls = new();
        private Dictionary<AirplaneData, AirplaneMapItem> airplaneMapControls = new();



        private AirplanesView currentAirplanesView;
        private ManagerView currentManagerView;


        // Références uniques aux panneaux
        private AirplanesView airplanesView;
        private AirportsView airportsView;
        private ManagerView managerView;

        private GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            DemarrerMusique();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            // Initialiser GameManager en premier
            gameManager = new GameManager();

            // Créer les UserControls en passant les listes du GameManager
            airplanesView = new AirplanesView(gameManager.Airplanes, gameManager.Airports);
            airportsView = new AirportsView(gameManager.Airports);
            managerView = new ManagerView(gameManager.Managers);

            // Conteneur pour tous les UserControls
            ContentArea.Content = new Grid();
            var grid = ContentArea.Content as Grid;
            grid.Children.Add(airplanesView);
            grid.Children.Add(airportsView);
            grid.Children.Add(managerView);

            // Cacher tous sauf airplanesView
            airplanesView.Visibility = Visibility.Visible;
            airportsView.Visibility = Visibility.Collapsed;
            managerView.Visibility = Visibility.Collapsed;

            currentAirplanesView = airplanesView;
            currentManagerView = managerView;

            // Mettre à jour le capital
            Capital = gameManager.Capital;

            // ---- Ajouter les avions sur la carte ----
            foreach(var airplane in gameManager.Airplanes)
            {
                var airplaneMapControl = new AirplaneMapItem(airplane.Id);
                MapCanvas.Children.Add(airplaneMapControl);

                Canvas.SetLeft(airplaneMapControl, airplane.CurrentAirport.Position.X);
                Canvas.SetTop(airplaneMapControl, airplane.CurrentAirport.Position.Y);

                airplaneMapControls[airplane] = airplaneMapControl; // si tu veux continuer à manipuler les avions
            }

            // ---- Ajouter les aéroports sur la carte ----
            foreach(var airport in gameManager.Airports)
            {
                var mapItem = new AirportMapItem(airport);
                MapCanvas.Children.Add(mapItem);

                Canvas.SetLeft(mapItem, airport.Position.X);
                Canvas.SetTop(mapItem, airport.Position.Y);
            }
        }


        private void AirplanesButton_Click( object sender, RoutedEventArgs e )
        {
            ToggleContent(airplanesView);
            currentAirplanesView = airplanesView;
        }

        private void AirportButton_Click( object sender, RoutedEventArgs e )
        {
            ToggleContent(airportsView);
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
            // On cache tous les UserControls
            foreach(UserControl uc in new UserControl[] { airplanesView, airportsView, managerView })
            {
                uc.Visibility = Visibility.Collapsed;
            }

            // On affiche le nouveau
            newContent.Visibility = Visibility.Visible;
        }

        private void Boutonparametre_Click( object sender, RoutedEventArgs e )
        {
            parametre confirm = new parametre(this);
            confirm.Owner = this;
            confirm.ShowDialog();
        }

        public event Action? CapitalChanged;

        private BigInteger  _capital = 0;

        private void AddMoney_Click( object sender, RoutedEventArgs e )
        {
            Capital += BigInteger.Parse("100000000000000");
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

        private void boutonregle_Click( object sender, RoutedEventArgs e )
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

                _lecteurMusique.MediaEnded += ( s, e ) =>
                {
                    _lecteurMusique.Position = TimeSpan.Zero; // Retour au début
                    _lecteurMusique.Play();
                };

                _lecteurMusique.Volume = 0.5; // Volume par défaut à 50%
                _lecteurMusique.Play();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur lecture musique : " + ex.Message);
            }
        }
        public void ModifierVolume( double nouveauVolume )
        {
            _lecteurMusique.Volume = nouveauVolume;
        }

        public async Task MoveAirplaneToDestination( AirplaneData airplane )
        {
            if(!airplaneControls.ContainsKey(airplane)) return;

            var control = airplaneControls[airplane];
            Vector2 start = airplane.Position;
            Vector2 end = airplane.TargetAirport.Position;

            float speed = airplane.Speed; // pixels par frame
            Vector2 direction = end - start;
            float distance = direction.Length();

            if(distance == 0) return;

            direction /= distance; // normaliser

            float traveled = 0;

            while(traveled < distance)
            {
                // déplace l’avion
                start += direction * speed;
                traveled += speed;

                airplane.Position = start;

                Canvas.SetLeft(control, airplane.Position.X);
                Canvas.SetTop(control, airplane.Position.Y);

                await Task.Delay(16); // ~60 FPS
            }

            // Arrivé à destination
            airplane.Position = end;
            Canvas.SetLeft(control, end.X);
            Canvas.SetTop(control, end.Y);

            // On peut ici créditer le revenu, retirer les passagers, etc.
        }
    }
}