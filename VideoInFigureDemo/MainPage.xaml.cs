using CompositionProToolkit;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoInFigureDemo {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private MediaPlayer _player;
        private Compositor _compositor;
        private SpriteVisual _visual;
        private MediaPlayerSurface _mediaSurface;
        private CompositionSurfaceBrush _videoBrush;

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            _player = new MediaPlayer();
            _player.IsLoopingEnabled = true;

            Ensure();

            _mediaSurface = _player.GetSurface(_compositor);
            var surface = _mediaSurface.CompositionSurface;

            _videoBrush = _compositor.CreateSurfaceBrush(surface);
            _videoBrush.Stretch = CompositionStretch.UniformToFill;
            _visual.Brush = _videoBrush;
        }

        private void Ensure() {
            if (_compositor == null) {
                _compositor = ElementCompositionPreview.GetElementVisual(Figure).Compositor;
                _visual = _compositor.CreateSpriteVisual();

                var _clip = _compositor.CreateGeometricClip("M188.703 65.624c2.964 16.426 4.668 35.091 5.326 47.894 5.195.326 11.169 1.018 18.21 2.17a4.51 4.51 0 0 1 2.92 1.821c.694.968.975 2.17.781 3.343a4.496 4.496 0 0 1-1.812 2.919 4.528 4.528 0 0 1-3.352.795c-6.409-1.048-11.804-1.685-16.436-2.009.011.973.01 1.85-.001 2.623.013.954.003 1.908-.031 2.861 5.248 1.168 11.33 2.94 18.634 5.524a4.515 4.515 0 0 1 2.596 2.301 4.484 4.484 0 0 1-.722 4.973 4.51 4.51 0 0 1-3.143 1.474 4.537 4.537 0 0 1-1.751-.269c-6.612-2.338-12.053-3.94-16.663-5.013C186.45 170.906 148.509 192 105.924 192c-42.298 0-78.988-20.81-85.961-54.283-3.986 1.036-8.555 2.442-13.885 4.327a4.517 4.517 0 0 1-3.479-.147 4.502 4.502 0 0 1-2.339-2.569 4.483 4.483 0 0 1 1.263-4.874c.447-.394.97-.697 1.535-.889 6.015-2.128 11.201-3.705 15.776-4.849a59.717 59.717 0 0 1 .023-5.966c-3.964.368-8.442.946-13.557 1.783a4.523 4.523 0 0 1-3.364-.784A4.485 4.485 0 0 1 .9 117.471a4.513 4.513 0 0 1 2.935-1.816c6.038-.988 11.301-1.642 15.964-2.019 1.207-12.39 3.64-31.124 6.686-48.012 4.07-22.548 9.842-32.362 13.87-39.21l.014-.025c1.49-2.534 5.04-3.205 7.256-1.267l30.329 26.535c8.832-2.972 18.253-4.622 27.969-4.622 10.552 0 20.819 1.946 30.398 5.419l31.242-27.333c2.214-1.938 5.765-1.266 7.255 1.267l.015.025c4.027 6.848 9.799 16.662 13.869 39.21Z");
                _visual.Clip = _clip;

                _visual.Size = new Vector2((float)FigureRoot.ActualWidth, (float)FigureRoot.ActualHeight);
                FigureRoot.SizeChanged += (s, a) => {
                    _visual.Size = new Vector2((float)a.NewSize.Width, (float)a.NewSize.Height);
                };

                ElementCompositionPreview.SetElementChildVisual(Figure, _visual);
            }
        }

        private async void SetSource(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            string src = UriSource.Text;
            if (Uri.IsWellFormedUriString(src, UriKind.Absolute)) {
                var source = MediaSource.CreateFromUri(new Uri(src));
                _player.Source = source;
                _player.Play();
            } else {
                await new MessageDialog("Wrong URL!").ShowAsync();
            }
        }
    }
}