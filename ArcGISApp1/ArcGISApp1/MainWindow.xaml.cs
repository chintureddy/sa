// language governing permissions and limitations under the License.

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.Tasks.Geoprocessing;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace ArcGISApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        // Used to store state of the geoprocessing task
        private bool _isExecutingGeoprocessing;

        // Used to show input and output
        private GraphicsOverlay _inputOverlay;
        private GraphicsOverlay _resultOverlay;

     
        public MainWindow()
        {
            InitializeComponent();
            // Create the UI, setup the control references and execute initialization 
            Initialize();
        }

        private void Initialize()
        {
            // Create a map with topographic basemap and an initial location
            Map myMap = new Map(BasemapType.Topographic, 45.3790902612337, 6.84905317262762, 13);

            // Hook into tapped event
            MyMapView.GeoViewTapped += OnMapViewTapped;

            // Create overlay that shows clicked location
            CreateOverlays();

            // Assign the map to the MapView
            MyMapView.Map = myMap;
        }

        private  void OnMapViewTapped(object sender, GeoViewInputEventArgs e)
        {
            if (_isExecutingGeoprocessing)
                return;

            SetBusy();

            // Clear previous location and results
            _inputOverlay.Graphics.Clear();
            _resultOverlay.Graphics.Clear();

            // Add marker to indicate analysis location
            Graphic inputGraphic = new Graphic(e.Location);
            _inputOverlay.Graphics.Add(inputGraphic);

            SetBusy(false);
        }

       

        private void CreateOverlays()
        {
            // Create renderer for input graphic
            SimpleRenderer inputRenderer = new SimpleRenderer()
            {
                Symbol = new SimpleMarkerSymbol()
                {
                    Size = 15,
                    Color = Colors.Yellow
                }
            };

            // Create overlay to where input graphic is shown
            _inputOverlay = new GraphicsOverlay()
            {
                Renderer = inputRenderer
            };

            // Create renderer for input graphic
            SimpleRenderer resultRenderer = new SimpleRenderer()
            {
                Symbol = new SimpleFillSymbol()
                {
                    Color = Color.FromArgb(0, 255, 255, 255)
                }
            };

            // Create overlay to where input graphic is shown
            _resultOverlay = new GraphicsOverlay()
            {
                Renderer = resultRenderer
            };

            // Add created overlays to the MapView
            Application.Current.Dispatcher.Invoke(() => MyMapView.GraphicsOverlays.Add(_inputOverlay));
            Application.Current.Dispatcher.Invoke(() => MyMapView.GraphicsOverlays.Add(_resultOverlay));
            //MyMapView.GraphicsOverlays.Add(_inputOverlay);
            //MyMapView.GraphicsOverlays.Add(_resultOverlay);
        }

        private void SetBusy(bool isBusy = true)
        {
            if (isBusy)
            {
                // Change UI to indicate that the geoprocessing is running
                _isExecutingGeoprocessing = true;
                busyOverlay.Visibility = Visibility.Visible;
               // progress.IsIndeterminate = true;
            }
            else
            {
                // Change UI to indicate that the geoprocessing is not running
                _isExecutingGeoprocessing = false;
                busyOverlay.Visibility = Visibility.Collapsed;
                //progress.IsIndeterminate = false;
            }
        }
        public static void UiInvoke(Action a)
        {
            Application.Current.Dispatcher.BeginInvoke(a);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    // Map initialization logic is contained in MapViewModel.cs
}

