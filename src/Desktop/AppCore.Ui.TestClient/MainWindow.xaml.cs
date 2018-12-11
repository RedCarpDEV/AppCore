using RCd.AppCore.Ui.Desktop.Theming;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace AppCore.Ui.TestClient
{

    public partial class MainWindow
    {

        #region Constructors

        public MainWindow()
        {
            Items = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };

            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region Properties

        private byte _red;
        public byte Red
        {
            get => _red;
            set
            {
                _red = value;
                SetAccent();
            }
        }

        private byte _green;
        public byte Green
        {
            get => _green;
            set
            {
                _green = value;
                SetAccent();
            }
        }

        private byte _blue;
        public byte Blue
        {
            get => _blue;
            set
            {
                _blue = value;
                SetAccent();
            }
        }

        public List<string> Items { get; }

        #endregion

        #region Methods

        private void SetAccent()
        {
            SkinManager.Current.SetResource("Accent.Default", new SolidColorBrush(Color.FromRgb(_red, _green, _blue)));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tr.ScaleX = 1;
            Tr.ScaleY = 1;
        }

        #endregion

    }

}